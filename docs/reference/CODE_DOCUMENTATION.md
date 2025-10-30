# 📖 Code Documentation Standards

This document defines code documentation standards for the Authentication project, including XML comments, inline comments, and practical examples.

## 🎯 Overview

The Authentication project uses comprehensive code documentation to ensure that:
- **New developers** can quickly understand the code
- **Maintenance** is facilitated with clear explanations
- **APIs** are self-documented via Swagger
- **Complex logic** is explained with inline comments

## 📝 XML Documentation Comments

### 🏛️ Controllers

Controllers should have complete XML documentation for automatic Swagger generation:

```csharp
/// <summary>
/// Controller responsible for handling authentication operations.
/// Provides endpoints for JWT token generation and user authentication.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    /// <summary>
    /// Generates a JWT token for authenticated users.
    /// Validates user credentials and returns a JWT token with user claims if successful.
    /// </summary>
    /// <param name="authenticationDTO">User credentials (username and password)</param>
    /// <param name="serviceProvider">Service provider for dependency injection</param>
    /// <returns>
    /// Returns a JWT token with user information on success (200),
    /// validation errors (400), unauthorized access (401), or internal server error (500)
    /// </returns>
    /// <response code="200">Token generated successfully</response>
    /// <response code="400">Invalid request data or validation errors</response>
    /// <response code="401">Invalid credentials</response>
    /// <response code="500">Internal server error</response>
    [HttpPost(AuthenticationRoutes.GenerateToken)]
    public async Task<IActionResult> GenerateToken([FromBody] AccountPayLoadDTO authenticationDTO, [FromServices] IServiceProvider serviceProvider)
    {
        // Implementation...
    }
}
```

### 🔧 Services and Interfaces

Interfaces and services should document purpose, parameters, returns, and exceptions:

```csharp
/// <summary>
/// Service interface for account management operations.
/// Provides comprehensive account CRUD operations, authentication, and token generation.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Authenticates user credentials and returns account if valid.
    /// Verifies username and password combination using Argon2 hash verification.
    /// </summary>
    /// <param name="account">Account with username and plain text password</param>
    /// <returns>Authenticated account entity</returns>
    /// <exception cref="InvalidOperationException">Thrown when account is not found</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when password is invalid</exception>
    Account GetAccountByUserNameAndPassword(Account account);

    /// <summary>
    /// Creates a new user account in the system.
    /// Automatically hashes the password with Argon2 and validates username uniqueness.
    /// </summary>
    /// <param name="account">Account entity to create</param>
    /// <exception cref="ConflictException">Thrown when username already exists</exception>
    void AddAccount(Account account);
}
```

### 🏗️ Domain Entities

Domain entities should explain their purpose and important properties:

```csharp
/// <summary>
/// Represents a user account entity in the authentication system.
/// Inherits from Entity base class providing audit fields and implements IAccount interface.
/// </summary>
public class Account : Entity, IAccount
{
    /// <summary>
    /// Gets or sets the unique username for this account.
    /// Must be unique across the system and is used for authentication.
    /// Cannot contain spaces or be null/empty.
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the password for this account.
    /// Automatically hashed using Argon2 algorithm when stored.
    /// Plain text passwords are only used during authentication validation.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
```

### 🛠️ Validators

Validators should explain the business rules applied:

```csharp
/// <summary>
/// FluentValidation validator for AccountPayLoadDTO objects.
/// Defines validation rules for user account creation and authentication requests.
/// Ensures username and password meet security and business requirements.
/// </summary>
public class AccountPayloadValidator : AbstractValidator<AccountPayLoadDTO>
{
    /// <summary>
    /// Initializes validation rules for account payload.
    /// Validates username and password format, length, and content restrictions.
    /// </summary>
    public AccountPayloadValidator()
    {
        // Validation rules...
    }
}
```

## 💬 Inline Comments (Explanatory Comments)

### 🔐 Security Logic

Explain security implementations, hashing, and validations:

```csharp
public void AddAccount(Account account)
{
    // Business rule: Ensure username uniqueness across the system
    // This prevents duplicate user registrations and maintains data integrity
    var existingAccount = _unitOfWork.AccountRepository.GetByUserName(account.UserName);
    if (existingAccount != null)
    {
        throw new ConflictException(ResourceLogin.DuplicateUserName);
    }

    // Set audit fields for tracking when and by whom the account was created
    account.DtCreated = DateTime.Now;
    account.CreatedBy = ApplicationConstants.DefaultCreatedByUser;

    // Security: Hash the plain text password using Argon2 algorithm
    // This ensures passwords are never stored in plain text in the database
    // Argon2 is a memory-hard function resistant to GPU-based attacks
    account.Password = StringHelper.ComputeArgon2Hash(account.Password);

    // Execute the database operation within a transaction
    // This ensures data consistency and allows rollback if any error occurs
    _unitOfWork.ExecuteInTransaction(() =>
    {
        _unitOfWork.AccountRepository.Add(account);
    });
}
```

### 🔑 JWT Token Generation

Explain each step of token creation:

```csharp
public Token? GenerateToken(Account account, IJwtSettings jwtSettings)
{
    // Step 1: Authenticate the user credentials
    // This validates username exists and password matches the stored Argon2 hash
    var isAccountValid = GetAccountByUserNameAndPassword(account);

    // Step 2: Retrieve user permissions from RBAC system
    // Gets all claim-action mappings associated with this user account
    // This enables role-based access control for different system resources
    var accountClaimActions = _unitOfWork.AccountClaimActionRepository
        .GetByIdAccount(isAccountValid.Id)
        .ToList();

    // Step 3: Build JWT claims collection
    // Start with the standard username claim for user identification
    var claims = new List<System.Security.Claims.Claim>
    {
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, isAccountValid.UserName)
    };

    // Step 4: Add permission claims in "Resource:Action" format
    // Example: "PlanoSaude:Inserir" means user can insert into PlanoSaude resource
    // This creates fine-grained permissions for different system operations
    claims.AddRange(accountClaimActions.Select(aca =>
        new System.Security.Claims.Claim(
            ApplicationConstants.ClaimTypes.Permission,
            $"{aca.ClaimAction.Claim.Value}:{aca.ClaimAction.Action.Name}"
        )
    ));

    // Step 5: Create JWT signing credentials using HMAC-SHA256
    // Secret key must be at least 256 bits (32 characters) for security
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    
    // Step 6: Build the JWT security token with all components
    var jwtSecurityToken = new JwtSecurityToken(
        issuer: jwtSettings.Issuer,           // Token issuer for validation
        audience: jwtSettings.Audience,       // Intended token audience
        claims: claims,                       // User identity and permissions
        expires: DateTime.Now.AddHours(ApplicationConstants.DefaultTokenExpirationHours), // Token lifetime
        signingCredentials: creds);           // Digital signature for integrity

    // Step 7: Serialize the JWT to a string format
    var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

    // Step 8: Create response token object with metadata
    var token = new Token
    {
        AccessToken = tokenString,
        Expiration = DateTime.Now.AddHours(ApplicationConstants.DefaultTokenExpirationHours),
        UserName = isAccountValid.UserName
    };

    return token;
}
```

### ⚙️ Complex Configuration

Explain complex configurations and middleware:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Environment configuration - defaults to Production for security
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? ApplicationConstants.Environment.Production;

// Load environment-specific configuration files
// This allows different settings for Development, Production, etc.
var appsettings = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

// Configure internationalization (i18n) support
// Supports English (en) and Portuguese Brazil (pt-BR) languages
// Resource files are located in the "Resource" directory
builder.Services.AddLocalization(options => options.ResourcesPath = "Resource");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "pt-BR" };
    options.SetDefaultCulture(supportedCultures[0])           // Default to English
           .AddSupportedCultures(supportedCultures)          // UI cultures for formatting
           .AddSupportedUICultures(supportedCultures);       // Localization cultures for text
});

// Configure CORS (Cross-Origin Resource Sharing) policy
// Allows all origins, headers, and methods for development flexibility
// Note: In production, consider restricting to specific domains for security
builder.Services.AddCors(options =>
{
    options.AddPolicy(ApplicationConstants.Cors.AllowAllPolicy, policy =>
    {
        policy.AllowAnyOrigin()      // Allow requests from any domain
              .AllowAnyHeader()      // Allow any HTTP headers
              .AllowAnyMethod();     // Allow any HTTP methods (GET, POST, etc.)
    });
});
```

### 🧪 Validation and Business Rules

Explain complex validations:

```csharp
public Account GetAccountByUserNameAndPassword(Account account)
{
    // First, retrieve the account by username from the database
    var dbAccount = _unitOfWork.AccountRepository.GetByUserName(account.UserName);

    // If no account exists with this username, throw exception
    if (dbAccount == null)
        throw new InvalidOperationException(ResourceLogin.AccountNotFound);

    // Verify the provided plain text password against the stored Argon2 hash
    // StringHelper.VerifyArgon2Hash compares the plain text password with the hashed version
    if (StringHelper.VerifyArgon2Hash(account.Password, dbAccount.Password))
        return dbAccount;

    // If password verification fails, throw unauthorized access exception
    throw new UnauthorizedAccessException(ResourceLogin.InvalidPassword);
}
```

## 📋 Standards and Conventions

### ✅ Best Practices

1. **Be Specific**: Explain the "why", not just the "what"
2. **Use Examples**: Include usage examples when appropriate
3. **Document Exceptions**: Always document exceptions that can be thrown
4. **Explain Business Logic**: Detailed comments for complex rules
5. **Keep Updated**: Update comments when code changes

### ❌ Avoid

1. **Obvious Comments**: `// Increment i` for `i++`
2. **Outdated Comments**: Comments that don't reflect current code
3. **Unnecessary Comments**: In self-explanatory code
4. **Explaining Syntax**: Focus on logic, not language syntax

### 🏷️ Recommended XML Tags

- `<summary>`: Main element description
- `<param>`: Parameter description
- `<returns>`: Return value description
- `<exception>`: Exceptions that can be thrown
- `<example>`: Usage examples
- `<remarks>`: Additional information
- `<see>`: References to other elements
- `<seealso>`: Related references

### 📏 Formatting

```csharp
/// <summary>
/// Concise description in one line.
/// More detailed description in multiple lines if needed.
/// </summary>
/// <param name="parameter1">Description of first parameter</param>
/// <param name="parameter2">Description of second parameter</param>
/// <returns>Description of what is returned</returns>
/// <exception cref="ArgumentNullException">When parameter1 is null</exception>
/// <exception cref="InvalidOperationException">When operation is invalid</exception>
/// <example>
/// <code>
/// var result = MyMethod("value1", "value2");
/// Console.WriteLine(result);
/// </code>
/// </example>
public string MyMethod(string parameter1, string parameter2)
{
    // Inline comment explaining specific logic
    // that is not obvious from the code
    if (parameter1 == null)
        throw new ArgumentNullException(nameof(parameter1));
    
    // Explains the algorithm or business rule
    // For example: concatenates parameters with default separator
    return $"{parameter1}_{parameter2}";
}
```

## 🛠️ Tools and Integration

### Visual Studio
- Automatic IntelliSense for XML comments
- Automatic template generation with `///`
- Real-time reference validation

### Swagger/OpenAPI
- XML comments are automatically converted to API documentation
- `<summary>` becomes endpoint description
- `<param>` documents API parameters
- `<response>` documents HTTP status codes

### SonarQube/Code Analysis
- Checks documentation coverage
- Identifies public methods without documentation
- Validates comment quality

## 📊 Quality Metrics

### Documentation Coverage
- **Controllers**: 100% of public methods documented
- **Services**: 100% of interfaces and main implementations
- **Domain Entities**: 100% of public properties
- **Complex Logic**: 80%+ of complex blocks commented

### Comment Quality
- Explain the "why", not just the "what"
- Include examples when appropriate
- Document all parameters and returns
- List all possible exceptions

---

💡 **Remember**: Good documentation is an investment in the project's future. It facilitates maintenance, onboarding of new developers, and reduces bugs.