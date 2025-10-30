# ❓ Frequently Asked Questions (FAQ)

## General Questions

### What is the Authentication Service?

The Authentication Service is a .NET 8.0-based authentication and authorization system that implements JWT token-based authentication and a comprehensive RBAC (Role-Based Access Control) system. It follows Clean Architecture principles and provides secure user management with Argon2 password hashing.

### What technologies does it use?

- **.NET 8.0** - Main framework
- **ASP.NET Core** - RESTful API
- **Entity Framework Core** - ORM
- **JWT Bearer** - Token authentication
- **Argon2** - Password hashing
- **MySQL/MariaDB** - Database
- **FluentValidation** - Input validation
- **Swagger/OpenAPI** - API documentation

### Is it production-ready?

Yes, the service includes:
- ✅ Comprehensive test coverage (288+ tests)
- ✅ Security best practices (Argon2, JWT)
- ✅ Clean Architecture
- ✅ CI/CD pipeline
- ✅ Production deployment guide
- ✅ Error handling and validation

## Installation & Setup

### What are the minimum requirements?

**Required:**
- .NET 8.0 SDK
- MySQL 8.0+
- Git

**Recommended:**
- Visual Studio 2022 or VS Code
- MySQL Workbench

### How do I install it?

See the [Installation Guide](../getting-started/INSTALLATION.md) for detailed steps. Quick version:

```bash
git clone https://github.com/maiconcardozo/Authentication.git
cd Authentication
dotnet restore Solution/Authentication.sln
dotnet build Solution/Authentication.sln
cd Src/Authentication.API
dotnet run
```

### Do I need MySQL or can I use another database?

The project is configured for MySQL but can be adapted to use:
- PostgreSQL (change provider to Npgsql)
- SQL Server (change provider to Microsoft.EntityFrameworkCore.SqlServer)
- SQLite (change provider to Microsoft.EntityFrameworkCore.Sqlite)

You'll need to update connection strings and database provider in `Program.cs`.

### How do I change the database connection?

Edit `Src/Authentication.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AuthenticationDB;Uid=your_user;Pwd=your_password;"
  }
}
```

Then run migrations:
```bash
cd Src/Authentication.API
dotnet ef database update --context ApiContextDevelopment
```

## Authentication & Authorization

### How does JWT authentication work?

1. User sends credentials to `/Authentication/GenerateToken`
2. Server validates credentials and generates JWT token
3. Client stores token and sends it in `Authorization: Bearer <token>` header
4. Server validates token on protected endpoints
5. Token expires after configured time (default 60 minutes)

See [JWT Guide](../architecture/JWT.md) for details.

### How do I protect my endpoints?

Add the `[Authorize]` attribute to your controller or action:

```csharp
[Authorize]
[HttpGet]
public IActionResult GetSecureData()
{
    // Only accessible with valid JWT token
    return Ok("Secure data");
}
```

### How does the RBAC system work?

The RBAC system has four components:

1. **Claims** - Permissions (e.g., "user:manage")
2. **Actions** - Operations (e.g., "CreateUser")
3. **ClaimAction** - Maps claims to actions
4. **AccountClaimAction** - Assigns permissions to users

See [RBAC Guide](../api/RBAC.md) for complete guide.

### Can I use role-based authorization?

Yes! Create claims with type "Role":

```json
{
  "type": "Role",
  "value": "Administrator",
  "description": "Administrator role"
}
```

Then map it to multiple actions.

### How secure is password storage?

Passwords are hashed using **Argon2**, which is the most secure password hashing algorithm recommended by OWASP. It's resistant to:
- Brute force attacks
- Rainbow table attacks
- GPU-based attacks

See [Security Guide](../architecture/SECURITY.md) for details.

## Development

### How do I run the project in development mode?

```bash
cd Src/Authentication.API
dotnet run --configuration Debug
```

Or use watch mode for hot reload:
```bash
dotnet watch run --configuration Debug
```

### How do I run tests?

```bash
# Quick method
scripts/test.sh              # Linux/Mac
scripts/test.bat             # Windows

# Or directly
dotnet test Solution/Authentication.sln
```

See [Testing Guide](../guides/TESTING.md) for more options.

### How do I access the API documentation?

Start the application and navigate to:
- **Swagger UI**: https://localhost:7001

The documentation is interactive and allows you to test endpoints directly.

### How do I change the port?

Edit `Src/Authentication.API/Properties/launchSettings.json`:

```json
{
  "applicationUrl": "https://localhost:7001;http://localhost:7000"
}
```

### Can I contribute to the project?

Yes! See [Contributing Guide](../guides/CONTRIBUTING.md) for guidelines.

## API Usage

### How do I test the API?

Use any of these tools:
- **Swagger UI** (built-in at https://localhost:7001)
- **cURL** (command line)
- **Postman**
- **Insomnia**
- **Thunder Client** (VS Code extension)

### What response format does the API use?

The API uses JSON for requests and responses. Errors follow RFC 7807 Problem Details format:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.x.x",
  "title": "Error Title",
  "status": 400,
  "detail": "Detailed error message",
  "instance": "/endpoint/path"
}
```

### Does the API support CORS?

Yes. In development, all origins are allowed. For production, configure specific origins in `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production",
        builder => builder
            .WithOrigins("https://yourdomain.com")
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

### Does the API support internationalization?

Yes! The API supports English and Portuguese. Use the `culture` query parameter:

```bash
# English
https://localhost:7001/?culture=en

# Portuguese  
https://localhost:7001/?culture=pt-BR
```

### How do I handle pagination?

Currently, pagination is not implemented in list endpoints. This is a future enhancement. For now, all endpoints return complete lists.

## Deployment

### How do I deploy to production?

See the [Deployment Guide](../guides/DEPLOYMENT.md) for detailed instructions covering:
- Docker deployment
- IIS deployment
- Linux systemd deployment
- Cloud deployment (Azure, AWS)

### What environment variables should I set?

Key environment variables for production:

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection="production-connection-string"
JwtSettings__SecretKey="your-very-secure-secret-key"
```

### Should I use HTTPS in production?

**Yes, absolutely!** Always use HTTPS in production to protect:
- JWT tokens
- User credentials
- Sensitive data

Configure HTTPS in your reverse proxy (nginx, Apache) or hosting platform.

### How do I configure JWT for production?

Use a strong secret key (64+ characters) and shorter token lifetime:

```json
{
  "JwtSettings": {
    "SecretKey": "your-very-strong-secret-key-at-least-64-characters-long",
    "ExpirationMinutes": 30,
    "Issuer": "YourProductionIssuer",
    "Audience": "YourProductionAudience"
  }
}
```

Store the secret key as an environment variable, not in appsettings.json.

## Troubleshooting

### API won't start - "Unable to connect to database"

Check:
1. MySQL is running
2. Connection string is correct in `appsettings.Development.json`
3. Database exists or run migrations: `dotnet ef database update`
4. User has proper permissions

### Tests are failing

```bash
# Clean and rebuild
scripts/run-tests.sh clean    # Linux/Mac
scripts/run-tests.bat clean   # Windows
```

If still failing, check the [Test Execution Status](../status/TEST_EXECUTION_STATUS.md).

### "401 Unauthorized" error

Common causes:
1. No JWT token provided
2. Token expired (lifetime = 60 minutes)
3. Token invalid or malformed
4. Endpoint requires authentication but token not sent

Solution: Get a new token via `/Authentication/GenerateToken` and include it:
```bash
Authorization: Bearer <your-token>
```

### "409 Conflict" when creating user

This means the username already exists. Usernames must be unique. Choose a different username.

### CORS error in browser

If you see:
```
Access to fetch has been blocked by CORS policy
```

Solution:
1. Ensure API is running on HTTPS
2. Check CORS configuration in `Program.cs`
3. Verify frontend origin is allowed

### Token not being validated

Check:
1. JWT settings in `appsettings.json` match between token generation and validation
2. `SecretKey` is the same
3. `Issuer` and `Audience` match
4. Token hasn't expired

### Migration errors

```bash
# For "Unable to create an object of type" error
cd Src/Authentication.API
dotnet ef database update --context ApiContextDevelopment --verbose
```

See [Troubleshooting Guide](TROUBLESHOOTING.md) for more solutions.

## Performance

### How many requests can it handle?

Performance depends on your infrastructure. The service is built with .NET 8.0 which provides excellent performance. Typical scenarios:
- Development: Hundreds of requests/second
- Production (properly configured): Thousands of requests/second

### Should I cache JWT tokens?

Yes! Clients should:
1. Store JWT token securely (not in localStorage for web apps)
2. Reuse the token until it expires
3. Only request new token when current expires

### How can I improve performance?

1. Use connection pooling for database
2. Implement caching for frequently accessed data
3. Use async/await throughout (already implemented)
4. Deploy behind reverse proxy with caching
5. Use database indexes (already configured)

## Security

### Is it secure for production use?

Yes, when properly configured:
- ✅ Argon2 password hashing
- ✅ JWT token authentication
- ✅ HTTPS support
- ✅ Input validation
- ✅ SQL injection protection (EF Core)
- ✅ XSS protection

Always:
- Use HTTPS in production
- Use strong JWT secret keys
- Keep .NET and packages updated
- Follow security best practices

### How do I report a security issue?

Open a private security advisory on GitHub or contact the maintainer directly. Do not create public issues for security vulnerabilities.

### What about rate limiting?

Rate limiting is not currently implemented. For production, implement:
- API gateway with rate limiting
- Middleware-based rate limiting (AspNetCoreRateLimit package)
- Reverse proxy rate limiting (nginx, Cloudflare)

### How do I rotate JWT secret keys?

1. Generate new secret key
2. Update `JwtSettings:SecretKey` in configuration
3. Restart application
4. All users must re-authenticate (old tokens become invalid)

For zero-downtime rotation, implement dual-key validation temporarily.

## Integration

### Can I integrate with existing applications?

Yes! The service provides a RESTful API that can be integrated with:
- Web applications (React, Angular, Vue)
- Mobile apps (iOS, Android, Flutter)
- Desktop applications
- Other backend services

See [Examples Guide](../api/EXAMPLES.md) for integration examples.

### Does it support OAuth2 / OpenID Connect?

Not currently. The service uses JWT token authentication. OAuth2/OpenID Connect support could be added as a future enhancement.

### Can I use it with a frontend framework?

Yes! The API is framework-agnostic. Examples provided for:
- JavaScript/React
- C# client applications

See [Examples Guide](../api/EXAMPLES.md).

## Additional Resources

### Where can I find more documentation?

- [Installation Guide](../getting-started/INSTALLATION.md)
- [Quick Start Guide](../getting-started/QUICK_START.md)
- [API Reference](../api/API.md)
- [Development Guide](../guides/DEVELOPMENT.md)
- [Architecture Guide](../architecture/ARCHITECTURE.md)

### Where can I get help?

- Check [Troubleshooting Guide](TROUBLESHOOTING.md)
- Open an [issue on GitHub](https://github.com/maiconcardozo/Authentication/issues)
- Check existing GitHub issues
- Review the documentation

### How do I stay updated?

- Watch the GitHub repository
- Check [CHANGELOG](../status/CHANGELOG.md) for updates
- Follow GitHub releases

---

**Didn't find your question?** Open an issue on GitHub or check the [Troubleshooting Guide](TROUBLESHOOTING.md).
