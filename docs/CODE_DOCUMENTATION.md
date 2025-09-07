# 📖 Code Documentation Standards

Este documento define os padrões de documentação de código para o projeto Authentication, incluindo XML comments, inline comments e exemplos práticos.

## 🎯 Visão Geral

O projeto Authentication utiliza documentação de código abrangente para garantir que:
- **Novos desenvolvedores** possam entender rapidamente o código
- **Manutenção** seja facilitada com explicações claras
- **APIs** sejam autodocumentadas via Swagger
- **Lógica complexa** seja explicada com comentários inline

## 📝 XML Documentation Comments

### 🏛️ Controllers

Os controllers devem ter documentação XML completa para geração automática do Swagger:

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

### 🔧 Services e Interfaces

Interfaces e services devem documentar o propósito, parâmetros, retornos e exceções:

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

Entidades de domínio devem explicar seu propósito e propriedades importantes:

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

Validadores devem explicar as regras de negócio aplicadas:

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

## 💬 Inline Comments (Comentários Explicativos)

### 🔐 Lógica de Segurança

Explicar implementações de segurança, hashing e validações:

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

### 🔑 Geração de Tokens JWT

Explicar cada etapa da criação de tokens:

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

### ⚙️ Configuração Complexa

Explicar configurações e middleware complexos:

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

### 🧪 Validação e Regras de Negócio

Explicar validações complexas:

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

## 📋 Padrões e Convenções

### ✅ Boas Práticas

1. **Seja Específico**: Explique o "porquê", não apenas o "o quê"
2. **Use Exemplos**: Inclua exemplos de uso quando apropriado
3. **Documente Exceções**: Sempre documente exceções que podem ser lançadas
4. **Explique Lógica de Negócio**: Comentários detalhados para regras complexas
5. **Mantenha Atualizado**: Atualize comentários quando o código mudar

### ❌ Evite

1. **Comentários Óbvios**: `// Incrementa i` para `i++`
2. **Comentários Desatualizados**: Comentários que não refletem o código atual
3. **Comentários Desnecessários**: Em código auto-explicativo
4. **Explicar Sintaxe**: Focar na lógica, não na sintaxe da linguagem

### 🏷️ Tags XML Recomendadas

- `<summary>`: Descrição principal do elemento
- `<param>`: Descrição de parâmetros
- `<returns>`: Descrição do valor de retorno
- `<exception>`: Exceções que podem ser lançadas
- `<example>`: Exemplos de uso
- `<remarks>`: Informações adicionais
- `<see>`: Referências a outros elementos
- `<seealso>`: Referências relacionadas

### 📏 Formatação

```csharp
/// <summary>
/// Descrição concisa em uma linha.
/// Descrição mais detalhada em múltiplas linhas se necessário.
/// </summary>
/// <param name="parameter1">Descrição do primeiro parâmetro</param>
/// <param name="parameter2">Descrição do segundo parâmetro</param>
/// <returns>Descrição do que é retornado</returns>
/// <exception cref="ArgumentNullException">Quando parameter1 é null</exception>
/// <exception cref="InvalidOperationException">Quando operação não é válida</exception>
/// <example>
/// <code>
/// var result = MyMethod("value1", "value2");
/// Console.WriteLine(result);
/// </code>
/// </example>
public string MyMethod(string parameter1, string parameter2)
{
    // Comentário inline explicando lógica específica
    // que não é óbvia pelo código
    if (parameter1 == null)
        throw new ArgumentNullException(nameof(parameter1));
    
    // Explica o algoritmo ou regra de negócio
    // Por exemplo: concatena parâmetros com separador padrão
    return $"{parameter1}_{parameter2}";
}
```

## 🛠️ Ferramentas e Integração

### Visual Studio
- IntelliSense automático para XML comments
- Geração automática de templates com `///`
- Validação de referências em tempo real

### Swagger/OpenAPI
- XML comments são automaticamente convertidos em documentação da API
- `<summary>` vira descrição do endpoint
- `<param>` documenta parâmetros da API
- `<response>` documenta códigos de status HTTP

### SonarQube/Análise de Código
- Verifica cobertura de documentação
- Identifica métodos públicos sem documentação
- Valida qualidade dos comentários

## 📊 Métricas de Qualidade

### Cobertura de Documentação
- **Controllers**: 100% dos métodos públicos documentados
- **Services**: 100% das interfaces e implementações principais
- **Domain Entities**: 100% das propriedades públicas
- **Complex Logic**: 80%+ dos blocos complexos comentados

### Qualidade dos Comentários
- Explicam o "porquê", não apenas o "o quê"
- Incluem exemplos quando apropriado
- Documentam todos os parâmetros e retornos
- Listam todas as exceções possíveis

---

💡 **Lembre-se**: Boa documentação é um investimento no futuro do projeto. Ela facilita manutenção, onboarding de novos desenvolvedores e reduz bugs.