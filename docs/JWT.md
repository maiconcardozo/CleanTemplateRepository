# 🔐 JWT Authentication Guide

## Overview

This guide details the JWT (JSON Web Token) authentication implementation in the Authentication project, including configuration, usage, and security best practices.

## 🏗️ JWT Architecture

### Authentication Flow

```mermaid
sequenceDiagram
    participant C as Client
    participant A as API
    participant D as Database
    participant J as JWT Service

    C->>A: POST /Authentication/GenerateToken
    A->>D: Validate credentials
    D-->>A: Valid user
    A->>J: Generate JWT token
    J-->>A: Signed token
    A-->>C: Token + user information
    
    Note over C: Client stores token

    C->>A: GET /Claim/GetClaims (with Bearer token)
    A->>J: Validate token
    J-->>A: Valid token + claims
    A->>D: Execute operation
    D-->>A: Requested data
    A-->>C: Authorized response
```

## ⚙️ JWT Configuration

### 1. Configuration in appsettings.json

```json
{
  "JwtSettings": {
    "Issuer": "AuthenticationService",
    "Audience": "AuthenticationClients",
    "SecretKey": "your-secret-key-at-least-32-characters-for-hmac256",
    "ExpirationMinutes": 60,
    "ClockSkew": 5,
    "ValidateIssuer": true,
    "ValidateAudience": true,
    "ValidateLifetime": true,
    "ValidateIssuerSigningKey": true
  }
}
```

### 2. Configuration in Program.cs

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; // Always true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = jwtSettings.GetValue<bool>("ValidateIssuer"),
        ValidateAudience = jwtSettings.GetValue<bool>("ValidateAudience"),
        ValidateLifetime = jwtSettings.GetValue<bool>("ValidateLifetime"),
        ValidateIssuerSigningKey = jwtSettings.GetValue<bool>("ValidateIssuerSigningKey"),
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.FromMinutes(jwtSettings.GetValue<int>("ClockSkew"))
    };

    // Events for logging and debugging
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError("JWT authentication failed: {Error}", context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("JWT token validated for user: {User}", 
                context.Principal?.Identity?.Name ?? "Unknown");
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Authentication middleware
app.UseAuthentication();
app.UseAuthorization();
```

## 🛠️ Implementation

### 1. JwtSettings Entity

```csharp
using Authentication.Login.Domain.Interface;

namespace Authentication.Login.Domain.Implementation
{
    public class JwtSettings : IJwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 60;
        public int ClockSkew { get; set; } = 5;
        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateLifetime { get; set; } = true;
        public bool ValidateIssuerSigningKey { get; set; } = true;
    }
}
```

### 2. IJwtSettings Interface

```csharp
namespace Authentication.Login.Domain.Interface
{
    public interface IJwtSettings
    {
        string Issuer { get; }
        string Audience { get; }
        string SecretKey { get; }
        int ExpirationMinutes { get; }
        int ClockSkew { get; }
        bool ValidateIssuer { get; }
        bool ValidateAudience { get; }
        bool ValidateLifetime { get; }
        bool ValidateIssuerSigningKey { get; }
    }
}
```

### 3. Token Generation Service

```csharp
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Authentication.Login.Domain.Interface;
using Authentication.Login.Domain.Implementation;

public class TokenService
{
    public Token GenerateToken(Account account, IJwtSettings jwtSettings)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
        
        // Create custom claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.Name, account.UserName),
            new Claim(ClaimTypes.Email, account.Email ?? ""),
            new Claim("AccountId", account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, 
                new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), 
                ClaimValueTypes.Integer64)
        };

        // Add user permission claims
        foreach (var permission in account.GetUserPermissions())
        {
            claims.Add(new Claim("permission", permission));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new Token
        {
            AccessToken = tokenString,
            ExpiresIn = jwtSettings.ExpirationMinutes * 60, // in seconds
            TokenType = "Bearer",
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes)
        };
    }
}
```

## 🔒 Usage in Controllers

### 1. Protected Controller

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
[Authorize] // Protects entire controller
public class ClaimController : ControllerBase
{
    [HttpGet("GetClaims")]
    public async Task<IActionResult> GetClaims()
    {
        // Token already validated by middleware
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = User.FindFirst(ClaimTypes.Name)?.Value;
        
        // Endpoint logic
        return Ok(claims);
    }

    [HttpGet("Public")]
    [AllowAnonymous] // Allows access without token
    public IActionResult PublicEndpoint()
    {
        return Ok("This endpoint is public");
    }
}
```

### 2. Claims-Based Authorization

```csharp
[HttpDelete("DeleteClaim/{id}")]
[Authorize(Policy = "RequireAdminPermission")]
public async Task<IActionResult> DeleteClaim(int id)
{
    // Only users with admin permission can access
    // ...
}

// In Program.cs, configure policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminPermission", policy =>
        policy.RequireClaim("permission", "admin:delete"));
});
```

## 📝 Usage Examples

### 1. Generate Token

```bash
curl -X POST "https://localhost:7001/Authentication/GenerateToken" \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "admin",
    "password": "password123"
  }'
```

**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJhZG1pbiIsImVtYWlsIjoiYWRtaW5AZXhhbXBsZS5jb20iLCJBY2NvdW50SWQiOiIxIiwicGVybWlzc2lvbiI6WyJ1c2VyOnJlYWQiLCJ1c2VyOndyaXRlIiwiYWRtaW46ZGVsZXRlIl0sImp0aSI6IjAwNGMxNGNmLWZlM2EtNGE5NC04YzRjLWQwMzY4YzIyZDQzZSIsImlhdCI6MTcwNTQyMzIwMCwibmJmIjoxNzA1NDIzMjAwLCJleHAiOjE3MDU0MjY4MDAsImlzcyI6IkF1dGhlbnRpY2F0aW9uU2VydmljZSIsImF1ZCI6IkF1dGhlbnRpY2F0aW9uQ2xpZW50cyJ9.signature",
  "expiresIn": 3600,
  "tokenType": "Bearer",
  "userName": "admin",
  "issuedAt": "2024-01-16T10:00:00Z",
  "expiresAt": "2024-01-16T11:00:00Z"
}
```

### 2. Use Token in Requests

```bash
curl -X GET "https://localhost:7001/Claim/GetClaims" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### 3. Decode Token (Client)

```javascript
// JavaScript example
function decodeJWT(token) {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
        atob(base64).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join('')
    );
    
    return JSON.parse(jsonPayload);
}

const payload = decodeJWT(token);
console.log('User ID:', payload.nameid);
console.log('Username:', payload.unique_name);
console.log('Permissions:', payload.permission);
```

## 🛡️ Security and Best Practices

### 1. Production Configuration

```json
{
  "JwtSettings": {
    "Issuer": "https://api.yourdomain.com",
    "Audience": "https://app.yourdomain.com",
    "SecretKey": "complex-generated-key-256-bits-or-more-characters-for-production",
    "ExpirationMinutes": 15,
    "ClockSkew": 0
  }
}
```

### 2. Key Rotation (Recommended)

```csharp
// Implement periodic key rotation
public class JwtKeyRotationService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtKeyRotationService> _logger;

    public async Task RotateKeyAsync()
    {
        // Generate new key
        var newKey = GenerateSecureKey();
        
        // Update configuration
        await UpdateSecretKeyAsync(newKey);
        
        // Log rotation
        _logger.LogInformation("JWT secret key rotated at {Time}", DateTime.UtcNow);
    }

    private string GenerateSecureKey()
    {
        using var rng = RandomNumberGenerator.Create();
        var keyBytes = new byte[64]; // 512 bits
        rng.GetBytes(keyBytes);
        return Convert.ToBase64String(keyBytes);
    }
}
```

### 3. Additional Validation

```csharp
// Custom middleware for extra validations
public class JwtValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtValidationMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            // Validate if user is still active in database
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!await IsUserActiveAsync(userId))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid token: user deactivated");
                return;
            }

            // Validate IP if necessary
            var clientIp = context.Connection.RemoteIpAddress?.ToString();
            if (!await IsIpAllowedAsync(userId, clientIp))
            {
                _logger.LogWarning("Access denied for IP {IP} of user {UserId}", clientIp, userId);
                context.Response.StatusCode = 403;
                return;
            }
        }

        await _next(context);
    }
}
```

### 4. Rate Limiting per Token

```csharp
// Implement rate limiting per user
services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        httpContext => RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});
```

## 🔍 Debugging and Troubleshooting

### 1. JWT Logs

```csharp
// In Program.cs
options.Events = new JwtBearerEvents
{
    OnMessageReceived = context =>
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogDebug("JWT token received: {Token}", context.Token);
        return Task.CompletedTask;
    },
    OnTokenValidated = context =>
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
        var claims = context.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
        logger.LogDebug("Token validated with claims: {Claims}", string.Join(", ", claims));
        return Task.CompletedTask;
    },
    OnAuthenticationFailed = context =>
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError("Authentication failed: {Error}", context.Exception.Message);
        return Task.CompletedTask;
    }
};
```

### 2. Common Issues

#### Expired Token
```bash
# 401 response with message:
{
  "error": "Token expired",
  "details": "JWT token expired at 2024-01-16T11:00:00Z"
}
```

#### Invalid Secret Key
```bash
# Error log:
Microsoft.IdentityModel.Tokens.SecurityTokenInvalidSignatureException: 
IDX10503: Signature validation failed. Keys tried: 'System.Text.StringBuilder'.
```

#### Missing Claims
```csharp
// Check if claim exists before using
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
if (string.IsNullOrEmpty(userId))
{
    return Unauthorized("Invalid token: user not identified");
}
```

## 📚 Additional Resources

- [JWT.io](https://jwt.io/) - JWT token decoder
- [RFC 7519](https://tools.ietf.org/html/rfc7519) - JWT specification
- [Microsoft JWT Documentation](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/introduction)

## 🧪 Tests

### Token Generation Test

```csharp
[Test]
public void GenerateToken_ValidAccount_ReturnsValidToken()
{
    // Arrange
    var account = new Account { Id = 1, UserName = "testuser" };
    var jwtSettings = new JwtSettings
    {
        SecretKey = "test-secret-key-with-minimum-32-characters-length",
        Issuer = "TestIssuer",
        Audience = "TestAudience",
        ExpirationMinutes = 60
    };

    // Act
    var token = _tokenService.GenerateToken(account, jwtSettings);

    // Assert
    Assert.That(token.AccessToken, Is.Not.Null);
    Assert.That(token.TokenType, Is.EqualTo("Bearer"));
    Assert.That(token.ExpiresIn, Is.EqualTo(3600));
}
```

---

**Important**: Always use HTTPS in production and keep secret keys secure. Never expose keys in logs or public source code.