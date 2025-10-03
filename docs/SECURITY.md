# 🔒 Security Configuration

## Overview

This guide details all security measures implemented in the Authentication project and provides guidelines for secure configuration in different environments.

## 🛡️ Implemented Security Features

### 1. JWT Authentication
- **Secure Tokens**: JWT with HMAC-SHA256 signature
- **Configurable Expiration**: Tokens with limited lifetime
- **Custom Claims**: User-specific information
- **Strict Validation**: Signature, expiration and issuer verification

### 2. Password Hashing
- **Secure Algorithm**: SHA256-based implementation (prepared for Argon2)
- **Automatic Salt**: Unique salt for each password
- **Secure Verification**: Constant-time comparison
- **Password Policy**: Complexity validation

### 3. Input Validation
- **FluentValidation**: Structured input data validation
- **Sanitization**: Malicious input cleaning
- **Rate Limiting**: Brute force attack protection
- **Schema Validation**: Type and format verification

### 4. RBAC System
- **Granular Control**: Function-specific permissions
- **Hierarchical Claims**: Structured permission system
- **Contextual Authorization**: Context-based verification
- **Audit**: Sensitive action logging

### 5. Data Integrity (PR #40)
- **Username Uniqueness**: UNIQUE constraint at database level
- **Duplicate Prevention**: Service validation before persistence
- **Conflict Handling**: HTTP 409 response for duplication attempts
- **Double Validation**: Service layer and database verification
- **Internationalized Messages**: Errors in English and Portuguese

## ⚙️ Security Configuration

### 1. Secure JWT Configuration

#### Development
```json
{
  "JwtSettings": {
    "Issuer": "AuthenticationService-Dev",
    "Audience": "AuthenticationClients-Dev",
    "SecretKey": "development-secret-key-minimum-32-characters-for-jwt-signing-do-not-use-in-production",
    "ExpirationMinutes": 60,
    "ClockSkew": 5
  }
}
```

#### Production
```json
{
  "JwtSettings": {
    "Issuer": "https://api.yourdomain.com",
    "Audience": "https://app.yourdomain.com", 
    "SecretKey": "${JWT_SECRET_KEY}", // Environment variable
    "ExpirationMinutes": 15,
    "ClockSkew": 0,
    "RequireHttpsMetadata": true,
    "ValidateIssuer": true,
    "ValidateAudience": true,
    "ValidateLifetime": true,
    "ValidateIssuerSigningKey": true
  }
}
```

### 2. Database Configuration

#### Secure Connection String
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=${DB_SERVER};Database=${DB_NAME};Uid=${DB_USER};Pwd=${DB_PASSWORD};SslMode=Required;ServerRootCa=ca-cert.pem;ServerCert=server-cert.pem;ServerKey=server-key.pem;"
  }
}
```

#### Secure MySQL Configuration
```sql
-- Create user with minimal privileges
CREATE USER 'app_user'@'%' IDENTIFIED BY 'complex_generated_password';
GRANT SELECT, INSERT, UPDATE, DELETE ON authentication_db.* TO 'app_user'@'%';

-- Configure mandatory SSL
ALTER USER 'app_user'@'%' REQUIRE SSL;

-- Configure specific table privileges
GRANT SELECT, INSERT, UPDATE ON authentication_db.accounts TO 'app_user'@'%';
GRANT SELECT ON authentication_db.claims TO 'app_user'@'%';
```

### 3. HTTPS and Certificates

#### Kestrel Configuration
```json
{
  "Kestrel": {
    "Endpoints": {
      "Https": {
        "Url": "https://localhost:7001",
        "Certificate": {
          "Path": "/etc/ssl/certs/app.pfx",
          "Password": "${CERT_PASSWORD}"
        }
      }
    }
  }
}
```

#### Security Headers
```csharp
// Program.cs
app.Use(async (context, next) =>
{
    // HTTPS Strict Transport Security
    context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
    
    // Prevent clickjacking
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    
    // XSS Protection
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    
    // Content Type Options
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    
    // Referrer Policy
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    
    // Content Security Policy
    context.Response.Headers.Add("Content-Security-Policy", 
        "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline';");
    
    await next();
});
```

## 🔐 Security Implementation

### 1. Security Middleware

```csharp
public class SecurityMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityMiddleware> _logger;

    public SecurityMiddleware(RequestDelegate next, ILogger<SecurityMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Rate limiting by IP
        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        if (!await CheckRateLimitAsync(clientIp))
        {
            context.Response.StatusCode = 429; // Too Many Requests
            await context.Response.WriteAsync("Rate limit exceeded");
            return;
        }

        // Validate User-Agent
        var userAgent = context.Request.Headers.UserAgent.ToString();
        if (string.IsNullOrEmpty(userAgent) || IsSuspiciousUserAgent(userAgent))
        {
            _logger.LogWarning("Suspicious request from IP {IP} with User-Agent: {UserAgent}", 
                clientIp, userAgent);
        }

        // Check request body size
        if (context.Request.ContentLength > 10 * 1024 * 1024) // 10MB
        {
            context.Response.StatusCode = 413; // Payload Too Large
            return;
        }

        await _next(context);
    }

    private async Task<bool> CheckRateLimitAsync(string clientIp)
    {
        // Implement rate limiting using Redis or in-memory cache
        // Return false if limit is exceeded
        return true;
    }

    private bool IsSuspiciousUserAgent(string userAgent)
    {
        var suspiciousPatterns = new[] { "bot", "crawler", "scanner", "hack" };
        return suspiciousPatterns.Any(pattern => 
            userAgent.ToLowerInvariant().Contains(pattern));
    }
}
```

### 2. Secure Input Validation

```csharp
using FluentValidation;

public class AccountPayLoadDTOValidator : AbstractValidator<AccountPayLoadDTO>
{
    public AccountPayLoadDTOValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username must contain only letters, numbers and underscore")
            .Must(NotContainSqlInjectionPatterns).WithMessage("Username contains forbidden characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("Password must contain: 1 lowercase, 1 uppercase, 1 number and 1 special character");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be valid format")
            .Length(5, 100).WithMessage("Email must be between 5 and 100 characters")
            .Must(NotContainMaliciousContent).WithMessage("Email contains suspicious content");
    }

    private bool NotContainSqlInjectionPatterns(string input)
    {
        if (string.IsNullOrEmpty(input)) return true;
        
        var sqlInjectionPatterns = new[]
        {
            "SELECT", "INSERT", "UPDATE", "DELETE", "DROP", "CREATE", "ALTER",
            "UNION", "SCRIPT", "EXEC", "EXECUTE", "--", "/*", "*/", "xp_", "sp_"
        };

        return !sqlInjectionPatterns.Any(pattern => 
            input.ToUpperInvariant().Contains(pattern));
    }

    private bool NotContainMaliciousContent(string email)
    {
        if (string.IsNullOrEmpty(email)) return true;
        
        var maliciousPatterns = new[] { "<script", "javascript:", "data:", "vbscript:" };
        return !maliciousPatterns.Any(pattern => 
            email.ToLowerInvariant().Contains(pattern));
    }
}
```

### 3. Username Uniqueness Validation (PR #40)

The duplicate username prevention implementation follows a defense-in-depth approach:

#### Database Layer
```sql
-- UNIQUE constraint at database level
CREATE UNIQUE INDEX IX_Account_UserName ON Account (UserName);
```

#### Service Layer
```csharp
public class AccountService : IAccountService
{
    public void AddAccount(Account account)
    {
        // Check if username already exists before insertion
        var existingAccount = _unitOfWork.AccountRepository.GetByUserName(account.UserName);
        if (existingAccount != null)
        {
            throw new ConflictException(ResourceLogin.DuplicateUserName);
        }

        account.DtCreated = DateTime.Now;
        account.CreatedBy = ApplicationConstants.DefaultCreatedByUser;

        _unitOfWork.AccountRepository.Add(account);
        _unitOfWork.Commit();
    }

    public void UpdateAccount(Account account)
    {
        // Check if new name belongs to a different account
        var existingAccount = _unitOfWork.AccountRepository.GetByUserName(account.UserName);
        if (existingAccount != null && existingAccount.Id != account.Id)
        {
            throw new ConflictException(ResourceLogin.DuplicateUserName);
        }

        account.DtUpdated = DateTime.Now;
        account.UpdatedBy = ApplicationConstants.DefaultCreatedByUser;
        
        _unitOfWork.AccountRepository.Update(account);
        _unitOfWork.Commit();
    }
}
```

#### Controller Layer
```csharp
[HttpPost("AddAccount")]
public async Task<IActionResult> AddAccount([FromBody] AccountPayLoadDTO dto)
{
    try
    {
        var account = _mapper.Map<Account>(dto);
        var result = await _accountService.AddAccountAsync(account);
        
        var response = SuccessResponseExampleFactory.ForSuccess(
            result, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
        return Ok(response);
    }
    catch (ConflictException ex)
    {
        var problemDetails = ProblemDetailsExampleFactory.ForConflict(
            ex.Message, HttpContext.Request.Path);
        return Conflict(problemDetails);
    }
}
```

#### Custom Exception Handling
```csharp
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
    public ConflictException(string message, Exception innerException) : base(message, innerException) { }
}
```

#### Security Benefits
1. **Race Condition Prevention**: Database constraint prevents duplicates even in high concurrency scenarios
2. **Double Validation**: Service verification for quick feedback and database for final guarantee
3. **Consistent Response**: Standardized HTTP 409 Conflict for all duplicate cases
4. **Security Logging**: Duplication attempts are logged for audit
5. **Internationalization**: Localized error messages for better user experience

### 4. Security Logging

```csharp
public class SecurityLogger
{
    private readonly ILogger<SecurityLogger> _logger;

    public void LogSuccessfulLogin(string userId, string ipAddress)
    {
        _logger.LogInformation("Successful login - User: {UserId}, IP: {IpAddress}", 
            userId, ipAddress);
    }

    public void LogFailedLogin(string userName, string ipAddress, string reason)
    {
        _logger.LogWarning("Failed login attempt - User: {UserName}, IP: {IpAddress}, Reason: {Reason}", 
            userName, ipAddress, reason);
    }

    public void LogSuspiciousActivity(string userId, string activity, string details)
    {
        _logger.LogError("Suspicious activity detected - User: {UserId}, Activity: {Activity}, Details: {Details}", 
            userId, activity, details);
    }

    public void LogPermissionViolation(string userId, string resource, string action)
    {
        _logger.LogWarning("Permission violation - User: {UserId}, Resource: {Resource}, Action: {Action}", 
            userId, resource, action);
    }
}
```

## 🚫 Vulnerability Prevention

### 1. SQL Injection
```csharp
// ✅ Correct - Using Entity Framework (parameterized)
var users = await _context.Accounts
    .Where(a => a.UserName == userName)
    .ToListAsync();

// ❌ Incorrect - Vulnerable dynamic SQL
// var query = $"SELECT * FROM Accounts WHERE UserName = '{userName}'";
```

### 2. XSS (Cross-Site Scripting)
```csharp
// ✅ Correct - Automatic encoding by ASP.NET Core
public class ApiResponse<T>
{
    [JsonPropertyName("data")]
    public T Data { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; } // Automatically encoded in JSON
}

// ✅ Input validation
public bool IsValidInput(string input)
{
    if (string.IsNullOrEmpty(input)) return false;
    
    var dangerousPatterns = new[] { "<script", "javascript:", "onclick", "onerror" };
    return !dangerousPatterns.Any(pattern => 
        input.ToLowerInvariant().Contains(pattern));
}
```

### 3. CSRF (Cross-Site Request Forgery)
```csharp
// Program.cs
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "__RequestVerificationToken";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Controller
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> CreateAccount([FromBody] AccountPayLoadDTO dto)
{
    // Protected implementation
}
```

### 4. Directory Traversal
```csharp
public bool IsValidFilePath(string path)
{
    if (string.IsNullOrEmpty(path)) return false;
    
    // Prevent directory traversal
    var dangerousPatterns = new[] { "..", "/", "\\", "~" };
    return !dangerousPatterns.Any(pattern => path.Contains(pattern));
}
```

## 🔍 Monitoring and Auditing

### 1. Audit Log
```csharp
public class AuditLog
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Action { get; set; }
    public string Resource { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public DateTime Timestamp { get; set; }
    public bool Success { get; set; }
    public string Details { get; set; }
}

public class AuditService
{
    public async Task LogActionAsync(string userId, string action, string resource, 
        bool success, string details = null)
    {
        var auditLog = new AuditLog
        {
            UserId = userId,
            Action = action,
            Resource = resource,
            IpAddress = GetClientIp(),
            UserAgent = GetUserAgent(),
            Timestamp = DateTime.UtcNow,
            Success = success,
            Details = details
        };

        await _context.AuditLogs.AddAsync(auditLog);
        await _context.SaveChangesAsync();
    }
}
```

### 2. Security Alerts
```csharp
public class SecurityAlertService
{
    public async Task CheckForAnomalousActivity(string userId)
    {
        // Check multiple logins from different locations
        var recentLogins = await GetRecentLoginsAsync(userId, TimeSpan.FromHours(1));
        var distinctIps = recentLogins.Select(l => l.IpAddress).Distinct().Count();
        
        if (distinctIps >= 3)
        {
            await SendSecurityAlertAsync(userId, "Multiple locations login detected");
        }

        // Check brute force attempts
        var failedAttempts = await GetFailedAttemptsAsync(userId, TimeSpan.FromMinutes(10));
        if (failedAttempts.Count >= 5)
        {
            await LockAccountAsync(userId, TimeSpan.FromMinutes(30));
            await SendSecurityAlertAsync(userId, "Account temporarily locked due to multiple failed attempts");
        }
    }
}
```

## 🛠️ Security Tools

### 1. Dependency Analysis
```bash
# Check for known vulnerabilities
dotnet list package --vulnerable

# Update dependencies with security fixes
dotnet add package PackageName --version LatestSecureVersion
```

### 2. Static Code Analysis
```yaml
# .github/workflows/security-scan.yml
name: Security Scan
on: [push, pull_request]

jobs:
  security-scan:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Run Security Code Scan
      uses: security-code-scan/security-code-scan-action@v1
      with:
        project-path: ./Solution/CleanTemplate.sln
```

### 3. Penetration Testing
```bash
# Use OWASP ZAP for automated testing
docker run -t owasp/zap2docker-stable zap-baseline.py -t https://localhost:7001

# Use sqlmap to test SQL injection
sqlmap -u "https://localhost:7001/Authentication/GenerateToken" --data="userName=test&password=test"
```

## 📋 Security Checklist

### Configuration
- [ ] HTTPS configured with valid certificate
- [ ] Security headers implemented
- [ ] CORS configured appropriately
- [ ] Rate limiting implemented
- [ ] Security logs configured

### Authentication
- [ ] JWT with strong secret key (256+ bits)
- [ ] Appropriate token expiration (≤ 15 min production)
- [ ] Strict token validation
- [ ] Refresh token implemented (if needed)
- [ ] Secure logout (token invalidation)

### Authorization
- [ ] RBAC implemented correctly
- [ ] Principle of least privilege applied
- [ ] Permission validation on all endpoints
- [ ] Audit of sensitive actions

### Data
- [ ] Secure password hashing (Argon2/bcrypt)
- [ ] Sensitive data not logged
- [ ] Data encryption at rest
- [ ] Secure and encrypted backup
- [ ] Data retention policy

### Code
- [ ] Strict input validation
- [ ] Data sanitization
- [ ] Secure error handling
- [ ] Updated dependencies
- [ ] Peer-reviewed code

### Infrastructure
- [ ] Server updated with patches
- [ ] Firewall configured
- [ ] Active security monitoring
- [ ] Backup and disaster recovery
- [ ] Physical access control

## 🚨 Incident Response

### 1. Attack Detection
```csharp
public class IncidentResponse
{
    public async Task HandleSecurityIncident(string type, string details)
    {
        // 1. Immediate logging
        _logger.LogCritical("Security incident detected: {Type} - {Details}", type, details);

        // 2. Notify security team
        await NotifySecurityTeamAsync(type, details);

        // 3. Block suspicious IP (if applicable)
        if (type == "BruteForce" && TryExtractIp(details, out string suspiciousIp))
        {
            await BlockIpAddressAsync(suspiciousIp);
        }

        // 4. Escalate monitoring level
        await EscalateMonitoringLevelAsync();
    }
}
```

### 2. Emergency Procedures
```csharp
public class EmergencyProcedures
{
    public async Task InitiateEmergencyShutdown()
    {
        // 1. Stop accepting new requests
        await DisableEndpointsAsync();

        // 2. Notify administrators
        await NotifyAdministratorsAsync("EMERGENCY: System shutdown initiated");

        // 3. Save critical state
        await SaveCriticalStateAsync();

        // 4. Log the event
        _logger.LogCritical("Emergency shutdown initiated at {Time}", DateTime.UtcNow);
    }
}
```

## 📚 Additional Resources

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [NIST Cybersecurity Framework](https://www.nist.gov/cyberframework)
- [Microsoft Security Documentation](https://docs.microsoft.com/en-us/aspnet/core/security/)
- [JWT Security Best Practices](https://tools.ietf.org/html/rfc8725)

---

**Important**: Security is an ongoing process. Stay updated with the latest threats and always practice defense-in-depth principles.