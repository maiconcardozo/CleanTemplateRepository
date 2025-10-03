# 🚀 Development Guide

## Overview

This guide provides all the necessary information to set up the development environment and start working on the Authentication project. The project follows a **development-first** approach, prioritizing the developer experience.

## 🛠️ Environment Setup

### Prerequisites

- **.NET 9.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0) **REQUIRED - Never use 8.0**
- **Visual Studio 2022** (17.8+) or **VS Code** with C# Dev Kit
- **MySQL 8.0+** - [Download](https://dev.mysql.com/downloads/mysql/)
- **Git** - [Download](https://git-scm.com/)

### 1. Repository Clone

```bash
git clone https://github.com/maiconcardozo/CleanTemplateRepository.git
cd CleanTemplateRepository
```

### 2. Database Configuration

```bash
# Install MySQL (Ubuntu/Debian)
sudo apt update
sudo apt install mysql-server

# Start MySQL
sudo systemctl start mysql
sudo systemctl enable mysql

# Connect and create database
mysql -u root -p
CREATE DATABASE AuthenticationDB_Dev;
CREATE USER 'dev_user'@'localhost' IDENTIFIED BY 'dev_password';
GRANT ALL PRIVILEGES ON AuthenticationDB_Dev.* TO 'dev_user'@'localhost';
FLUSH PRIVILEGES;
exit;
```

### 3. Application Configuration

Create the `appsettings.Development.json` file in the API project:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AuthenticationDB_Dev;Uid=dev_user;Pwd=dev_password;"
  },
  "JwtSettings": {
    "Issuer": "AuthenticationService",
    "Audience": "AuthenticationClients", 
    "SecretKey": "development-secret-key-minimum-32-characters-long-for-jwt-signing",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

### 4. Restore Dependencies and Run

```bash
# Restore NuGet packages
dotnet restore Solution/CleanTemplate.sln

# Run migrations
cd Src/CleanTemplate.API
dotnet ef database update --context ApiContextDevelopment

# Build and run
dotnet run --project Src/CleanTemplate.API
```

## 🏃‍♂️ Development Workflow

### Quick Development

```bash
# Run in watch mode (automatically recompiles)
cd Src/CleanTemplate.API
dotnet watch run

# In another terminal, run tests automatically
dotnet watch test Src/CleanTemplate.Tests/CleanTemplate.Tests.csproj
```

### Functionality Verification

1. **API running**: https://localhost:7001
2. **Swagger UI**: https://localhost:7001 (automatic redirect)
3. **Health Check**: https://localhost:7001/health

### Quick API Test

```bash
# Create test account
curl -X POST "https://localhost:7001/Authentication/AddAccount" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "userName": "dev_user",
    "password": "DevPassword123!",
    "email": "dev@example.com"
  }'

# Generate JWT token
curl -X POST "https://localhost:7001/Authentication/GenerateToken" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "userName": "dev_user", 
    "password": "DevPassword123!"
  }'
```

## 🧪 Running Tests

### Unit Tests (Recommended for development)

```bash
# Run only unit tests
dotnet test --filter "FullyQualifiedName~Unit"

# Run with watch mode
dotnet watch test --filter "FullyQualifiedName~Unit"
```

### Integration Tests

```bash
# Run integration tests
dotnet test --filter "FullyQualifiedName~Integration"
```

### Code Coverage

```bash
# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# View report (after installing reportgenerator)
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report"
```

## 🔧 Development Tools

### Visual Studio Code

Recommended extensions:

```json
{
  "recommendations": [
    "ms-dotnettools.csharp",
    "ms-dotnettools.csdevkit",
    "ms-mssql.mssql",
    "humao.rest-client"
  ]
}
```

### Entity Framework Tools

```bash
# Install EF Tools globally
dotnet tool install --global dotnet-ef

# Create new migration
dotnet ef migrations add MigrationName --context ApiContextDevelopment

# Apply migrations
dotnet ef database update --context ApiContextDevelopment

# Revert migration
dotnet ef database update PreviousMigrationName --context ApiContextDevelopment
```

## 🐛 Debug and Troubleshooting

### Common Issues

#### 1. Database Connection Error
```bash
# Check if MySQL is running
sudo systemctl status mysql

# Test connection
mysql -u dev_user -p -h localhost AuthenticationDB_Dev
```

#### 2. Build Error
```bash
# Clean and rebuild
dotnet clean Solution/CleanTemplate.sln
dotnet restore Solution/CleanTemplate.sln  
dotnet build Solution/CleanTemplate.sln
```

#### 3. HTTPS Certificate Error
```bash
# Trust development certificate
dotnet dev-certs https --trust
```

### Debug in Visual Studio

1. Set `CleanTemplate.API` as startup project
2. Configure breakpoints
3. Press F5 to start debugging
4. Use Swagger UI to test endpoints

### Development Logs

```bash
# View logs in real time
cd Src/CleanTemplate.API
dotnet run --verbosity detailed

# Logs are displayed in console in development mode
```

## 📝 Code Conventions

### Commits

```bash
# Commit pattern
git commit -m "feat(auth): add JWT token refresh endpoint"
git commit -m "fix(validation): handle null input in password validator"
git commit -m "docs(readme): update installation instructions"
```

### Branches

```bash
# For new feature
git checkout -b feature/feature-name

# For bug fix
git checkout -b bugfix/bug-description

# For refactoring
git checkout -b refactor/refactoring-description
```

## 🚀 Productivity

### Useful Scripts

Create a `dev-scripts.sh` file:

```bash
#!/bin/bash

# Start development
dev-start() {
    echo "🚀 Starting development environment..."
    sudo systemctl start mysql
    cd Src/CleanTemplate.API
    dotnet watch run
}

# Run tests
dev-test() {
    echo "🧪 Running tests..."
    dotnet watch test --filter "FullyQualifiedName~Unit"
}

# Database reset
dev-reset-db() {
    echo "🗃️ Resetting database..."
    cd Src/CleanTemplate.API
    dotnet ef database drop --context ApiContextDevelopment --force
    dotnet ef database update --context ApiContextDevelopment
}
```

### IDE Configuration

#### Visual Studio 2022

1. **Solution Items**: Add `appsettings.Development.json` to Solution Items
2. **Startup Project**: Set `CleanTemplate.API` as startup project
3. **Debug Profile**: Use "https" profile for development

#### VS Code

Configure `.vscode/launch.json`:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Launch (web)",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/Src/CleanTemplate.API/bin/Debug/net9.0/CleanTemplate.API.dll",
      "args": [],
      "cwd": "${workspaceFolder}/Src/CleanTemplate.API",
      "stopAtEntry": false,
      "serverReadyAction": {
        "action": "openExternally",
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
      },
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  ]
}
```

## 📚 Additional Resources

- [API Documentation](API.md)
- [System Architecture](ARCHITECTURE.md)
- [Testing Guide](TESTING.md)
- [Deployment Guide](DEPLOYMENT.md)

## 🤝 Contributing

1. Fork the repository
2. Create branch for your feature
3. Implement following conventions
4. Run tests
5. Commit with descriptive message
6. Open Pull Request

---

**Tip**: Keep this guide updated as the project evolves. It's the starting point for all new developers!