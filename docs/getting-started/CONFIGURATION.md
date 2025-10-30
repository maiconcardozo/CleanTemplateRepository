# ⚙️ Configuration Guide

## Overview

This guide covers all configuration options for the Authentication Service, including database, JWT, security, and environment-specific settings.

## Database Configuration

### Development Environment

Update the connection string in `Src/Authentication.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AuthenticationDB;Uid=your_user;Pwd=your_password;"
  }
}
```

### Production Environment

For production, use environment variables or a secure configuration provider:

```bash
# Using environment variables
export ConnectionStrings__DefaultConnection="Server=prod-server;Database=AuthenticationDB;Uid=prod_user;Pwd=secure_password;"
```

Or update `appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=AuthenticationDB;Uid=prod_user;Pwd=secure_password;"
  }
}
```

### Database Contexts

The project includes two database contexts:
- **ApiContextDevelopment**: For development environment
- **ApiContextProduction**: For production environment

Initialize the database:

```bash
# Development
cd Src/Authentication.API
dotnet ef database update --context ApiContextDevelopment

# Production
dotnet ef database update --context ApiContextProduction
```

## JWT Configuration

### Development JWT Settings

Configure JWT in `appsettings.Development.json`:

```json
{
  "JwtSettings": {
    "Issuer": "Authentication",
    "Audience": "AuthenticationClients",
    "SecretKey": "your-secret-key-minimum-32-characters",
    "ExpirationMinutes": 60
  }
}
```

**Important Notes:**
- `SecretKey` must be at least 32 characters long
- `ExpirationMinutes` controls token lifetime (60 = 1 hour)
- `Issuer` and `Audience` should match your application configuration

### Production JWT Settings

For production, use stronger settings:

```json
{
  "JwtSettings": {
    "Issuer": "YourProductionIssuer",
    "Audience": "YourProductionAudience",
    "SecretKey": "your-very-strong-secret-key-at-least-64-characters-long",
    "ExpirationMinutes": 30
  }
}
```

**Production Best Practices:**
- Use environment variables for `SecretKey`
- Shorter token lifetime (15-30 minutes)
- Implement refresh token mechanism
- Use HTTPS only

### Programmatic JWT Configuration

The service configures JWT in `Program.cs`:

```csharp
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    });
```

## Security Configuration

### Password Hashing

The service uses Argon2 for password hashing. This is configured automatically and doesn't require manual configuration.

```csharp
// Password hashing is handled internally
string passwordHash = StringHelper.ComputeArgon2Hash("myPassword123");
bool isValid = StringHelper.VerifyArgon2Hash("myPassword123", passwordHash);
```

### Password Requirements

Configure password validation in `CreateUserRequestValidator`:

```csharp
RuleFor(x => x.Password)
    .NotEmpty()
    .MinimumLength(8)
    .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])")
    .WithMessage("Password must contain: 1 lowercase, 1 uppercase, 1 number, 1 special character");
```

## CORS Configuration

### Development CORS

In `Program.cs`, CORS is configured to allow all origins in development:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

### Production CORS

For production, restrict CORS to specific origins:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production",
        builder => builder
            .WithOrigins("https://yourdomain.com", "https://www.yourdomain.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
```

## Internationalization (i18n)

### Supported Languages

The API supports English and Portuguese:

- **English**: Default language
- **Portuguese**: `pt-BR`

### Language Selection

Users can select language via query parameter:

```bash
# English
https://localhost:7001/?culture=en

# Portuguese
https://localhost:7001/?culture=pt-BR
```

The selected language is stored in a cookie and persists across requests.

### Configure Localization

Localization is configured in `Program.cs`:

```csharp
var supportedCultures = new[] { "en", "pt-BR" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
```

## Swagger Configuration

### Development Swagger

Swagger is enabled by default in development:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication API V1");
        c.RoutePrefix = string.Empty; // Serve at root
    });
}
```

### Swagger Authentication

Configure Swagger to support JWT authentication:

```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
```

## Logging Configuration

### Development Logging

Configure detailed logging in development:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

### Production Logging

Use more restrictive logging in production:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

### Programmatic Logging Configuration

```csharp
if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
    builder.Logging.SetMinimumLevel(LogLevel.Debug);
}
else
{
    builder.Logging.SetMinimumLevel(LogLevel.Warning);
}
```

## Environment-Specific Configuration

### Development Environment

```bash
export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ENVIRONMENT=Development
```

Features enabled in development:
- Detailed error pages
- Swagger UI
- Debug logging
- CORS allow all

### Production Environment

```bash
export ASPNETCORE_ENVIRONMENT=Production
export DOTNET_ENVIRONMENT=Production
```

Features in production:
- Error handling middleware
- HTTPS enforcement
- Restricted CORS
- Warning-level logging

## Health Check Configuration

Configure health checks in `Program.cs`:

```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApiContextDevelopment>();

app.MapHealthChecks("/health");
```

Access health check at: `https://localhost:7001/health`

## API Endpoint Configuration

### Default Port Configuration

Configure ports in `Src/Authentication.API/Properties/launchSettings.json`:

```json
{
  "profiles": {
    "https": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "https://localhost:7001;http://localhost:7000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

## Entity Framework Configuration

### Migration Configuration

The project uses EF Core Migrations. Configuration files are in:
- `Src/Authentication.Login/Infrastructure/Implementation/`

### Create New Migration

```bash
cd Src/Authentication.API
dotnet ef migrations add MigrationName --context ApiContextDevelopment
```

### Apply Migration

```bash
dotnet ef database update --context ApiContextDevelopment
```

## Configuration Files Reference

| File | Purpose | Environment |
|------|---------|-------------|
| `appsettings.json` | Base configuration | All |
| `appsettings.Development.json` | Development overrides | Development |
| `appsettings.Production.json` | Production overrides | Production |
| `launchSettings.json` | Debug launch settings | Development |

## Next Steps

After configuration:

1. **Test Configuration**: Run the application and verify settings
2. **Quick Start**: Follow [Quick Start Guide](QUICK_START.md)
3. **Development**: Read [Development Guide](../guides/DEVELOPMENT.md)
4. **Security**: Review [Security Guide](../architecture/SECURITY.md)

## Related Documentation

- [Installation Guide](INSTALLATION.md) - Installation instructions
- [Deployment Guide](../guides/DEPLOYMENT.md) - Production deployment
- [Security Guide](../architecture/SECURITY.md) - Security best practices
- [Troubleshooting](../reference/TROUBLESHOOTING.md) - Common issues
