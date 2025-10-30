# 🚀 Installation Guide

## Prerequisites

### Required Software
- **.NET 8.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **MySQL 8.0+** - [Download](https://dev.mysql.com/downloads/mysql/)
- **Git** - [Download](https://git-scm.com/)

### Recommended Tools
- **Visual Studio 2022** (17.14+) with .NET workload - [Download](https://visualstudio.microsoft.com/)
- **Visual Studio Code** with C# Dev Kit extension - [Download](https://code.visualstudio.com/)
- **JetBrains Rider** 2024.1+
- **MySQL Workbench** for database management - [Download](https://dev.mysql.com/downloads/workbench/)

## Development Installation

### 1. Clone the Repository

```bash
git clone https://github.com/maiconcardozo/Authentication.git
cd Authentication
```

### 2. Verify .NET 8.0 Installation

```bash
dotnet --version
# Should output: 8.0.x
```

If you don't have .NET 8.0, download and install it from https://dotnet.microsoft.com/download/dotnet/8.0

### 3. Restore Dependencies

```bash
dotnet restore Solution/Authentication.sln
```

### 4. Build the Project

```bash
# Build in Debug mode (development)
dotnet build Solution/Authentication.sln --configuration Debug

# Or use the convenience script
scripts/build.sh debug         # Linux/Mac
scripts/build.bat debug        # Windows
```

### 5. Configure Database

Update the connection string in `Src/Authentication.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AuthenticationDB;Uid=your_user;Pwd=your_password;"
  }
}
```

### 6. Initialize Database

```bash
cd Src/Authentication.API
dotnet ef database update --context ApiContextDevelopment
```

### 7. Configure JWT Settings

Update JWT settings in `appsettings.Development.json`:

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

### 8. Run the Application

```bash
cd Src/Authentication.API
dotnet run --configuration Debug
```

The API will start at: **https://localhost:7001**

### 9. Verify Installation

Access the Swagger documentation at: https://localhost:7001

You should see the API documentation with two main sections:
- **Authentication API** - Login and token generation
- **Access Control API** - RBAC management

## Production Installation

### Prerequisites
- .NET 8.0 SDK or higher (REQUIRED)
- MySQL 8.0+ or higher
- Entity Framework Core 8.0.8

### Building for Production

```bash
git clone https://github.com/maiconcardozo/Authentication.git
cd Authentication
dotnet build Solution/Authentication.sln --configuration Release
```

### Production Deployment

For detailed production deployment instructions, see:
- [Deployment Guide](../guides/DEPLOYMENT.md)

## Verification Scripts

The project includes convenience scripts to verify your installation:

### Complete Verification (Recommended)

```bash
# Linux/Mac
scripts/build.sh verify

# Windows
scripts/build.bat verify
```

### Single Command Test

```bash
# Linux/Mac
scripts/test.sh

# Windows
scripts/test.bat
```

### Manual Verification Steps

```bash
# 1. Verify compilation
dotnet build Solution/Authentication.sln --configuration Release

# 2. Run tests
dotnet test Solution/Authentication.sln

# 3. Start the application
cd Src/Authentication.API
dotnet run

# 4. Access API documentation
# Open browser: https://localhost:7001
```

## Expected Results

After successful installation:
- ✅ **Build**: Should complete without errors
- ✅ **Tests**: Should pass (288 tests)
- ✅ **API**: Accessible at https://localhost:7001
- ✅ **Documentation**: Swagger UI available with complete API documentation
- ✅ **Health Check**: https://localhost:7001/health returns 200 OK

## Troubleshooting Installation Issues

### Issue: .NET SDK Not Found

```bash
# Verify .NET installation
dotnet --list-sdks
# Should show: 8.0.x

# If not installed, download from:
# https://dotnet.microsoft.com/download/dotnet/8.0
```

### Issue: Build Errors

```bash
# Clean and rebuild
dotnet clean Solution/Authentication.sln
dotnet restore Solution/Authentication.sln
dotnet build Solution/Authentication.sln
```

### Issue: Database Connection Errors

- Verify MySQL is running
- Check connection string in `appsettings.Development.json`
- Ensure database user has proper permissions
- Try running migrations again: `dotnet ef database update --context ApiContextDevelopment --verbose`

### Issue: Port Already in Use

Edit `Src/Authentication.API/Properties/launchSettings.json` to change the port:

```json
{
  "applicationUrl": "https://localhost:7001;http://localhost:7000"
}
```

For more troubleshooting information, see:
- [Troubleshooting Guide](../reference/TROUBLESHOOTING.md)

## Next Steps

After successful installation:

1. **Quick Start**: Follow the [Quick Start Guide](QUICK_START.md) for first usage
2. **Development**: Read the [Development Guide](../guides/DEVELOPMENT.md) for development workflow
3. **API Usage**: Explore the [API Documentation](../api/API.md)
4. **Examples**: Check [Practical Examples](../api/EXAMPLES.md)

## Available Helper Scripts

**Build Scripts:**
```bash
scripts/build.sh debug         # Compile in Debug mode (default)
scripts/build.sh release       # Compile in Release mode  
scripts/build.sh clean         # Clean and rebuild
scripts/build.sh restore       # Restore dependencies only
scripts/build.sh verify        # Complete verification (build + tests)
scripts/build.sh help          # Show all options
```

**Test Scripts:**
```bash
scripts/run-tests.sh all       # Run all tests
scripts/run-tests.sh unit      # Run unit tests only
scripts/run-tests.sh integration  # Run integration tests only
scripts/run-tests.sh coverage  # Run with code coverage
scripts/run-tests.sh verbose   # Run with detailed output
scripts/run-tests.sh watch     # Run in watch mode
scripts/run-tests.sh clean     # Clean, rebuild, then test
```

*Note: Windows users should use `.bat` extensions instead of `.sh`*
