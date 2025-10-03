# 🤝 Contributing Guide

Thank you for your interest in contributing to the CleanTemplate service! This guide will help you get started with contributing to the project.

## 📋 Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Setup](#development-setup)
- [Contributing Process](#contributing-process)
- [Coding Standards](#coding-standards)
- [Testing Guidelines](#testing-guidelines)
- [Documentation](#documentation)
- [Review Process](#review-process)

## 📜 Code of Conduct

This project adheres to a Code of Conduct. By participating, you are expected to uphold this code. Please report unacceptable behavior to the project maintainers.

### Our Standards

- **Be respectful**: Treat everyone with respect and kindness
- **Be inclusive**: Welcome newcomers and help them learn
- **Be constructive**: Provide helpful feedback and suggestions
- **Be professional**: Keep discussions focused and professional

## 🚀 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Git](https://git-scm.com/)
- [MySQL 8.0+](https://dev.mysql.com/downloads/mysql/)
- IDE: [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- **Foundation Repository**: Required dependency from [Foundation repository](https://github.com/maiconcardozo/Foundation)

### Types of Contributions

We welcome various types of contributions:

- 🐛 **Bug Reports**: Report issues you've found
- 🚀 **Feature Requests**: Suggest new features
- 📖 **Documentation**: Improve or add documentation
- 💻 **Code Contributions**: Fix bugs or implement features
- 🧪 **Testing**: Add or improve tests
- 🎨 **UI/UX**: Improve user experience

## 🛠️ Development Setup

### 1. Fork and Clone

```bash
# First, clone the Foundation repository (required dependency)
git clone https://github.com/maiconcardozo/Foundation.git

# Fork the CleanTemplateRepository repository on GitHub
# Then clone your fork alongside the Foundation repository
git clone https://github.com/YOUR_USERNAME/CleanTemplateRepository.git

# The folder structure should be:
# Parent Directory/
# ├── Foundation/
# │   └── Src/
# │       └── Foundation.Base/
# └── CleanTemplateRepository/
#     └── Src/
#         ├── CleanTemplate.API/
#         └── CleanTemplate.Application/

cd CleanTemplateRepository

# Add upstream remote
git remote add upstream https://github.com/maiconcardozo/CleanTemplateRepository.git
```

### 2. Environment Setup

```bash
# Install dependencies
dotnet restore Solution/CleanTemplate.sln

# Setup database
mysql -u root -p
CREATE DATABASE CleanTemplateDB_Dev;
exit

# Run migrations
cd Src/CleanTemplate.API
dotnet ef database update --context ApiContextDevelopment
```

### 3. Configuration

Create `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CleanTemplateDB_Dev;Uid=root;Pwd=your_password;"
  },
  "JwtSettings": {
    "Issuer": "CleanTemplateService",
    "Audience": "CleanTemplateClients",
    "SecretKey": "development-secret-key-minimum-32-characters-long",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 4. Verify Setup

```bash
# Build the solution
dotnet build Solution/CleanTemplate.sln

# Run tests
dotnet test Solution/CleanTemplate.sln

# Start the application
cd Src/CleanTemplate.API
dotnet run
```

Visit `https://localhost:7001` to verify the application is running.

## 🔄 Contributing Process

### 1. Create an Issue

Before starting work, create an issue or comment on an existing one:

- **Bug Reports**: Use the bug report template
- **Feature Requests**: Use the feature request template
- **Questions**: Use the question template

### 2. Create a Branch

```bash
# Update your fork
git checkout main
git pull upstream main

# Create a feature branch
git checkout -b feature/your-feature-name
# or
git checkout -b bugfix/issue-number-description
```

### Branch Naming Convention

- `feature/description`: New features
- `bugfix/issue-number-description`: Bug fixes
- `docs/description`: Documentation updates
- `refactor/description`: Code refactoring
- `test/description`: Test improvements

### 3. Make Changes

- Follow the coding standards (see below)
- Write tests for your changes
- Update documentation if needed
- Keep commits small and focused

### 4. Commit Your Changes

```bash
# Stage your changes
git add .

# Commit with a descriptive message
git commit -m "Add user authentication endpoint

- Implement JWT token generation
- Add password validation
- Include comprehensive error handling
- Add unit tests for authentication flow

Fixes #123"
```

### Commit Message Format

Use conventional commits format:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, etc.)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

**Examples:**
```
feat(auth): add JWT token refresh endpoint

fix(validation): handle null input in password validator

docs(readme): update installation instructions

test(auth): add integration tests for login flow
```

### 5. Push and Create Pull Request

```bash
# Push your branch
git push origin feature/your-feature-name

# Create a pull request through GitHub UI
```

### Pull Request Guidelines

- Use the PR template
- Provide clear description of changes
- Reference related issues
- Include screenshots for UI changes
- Ensure all checks pass

## 📝 Coding Standards

### C# Coding Style

Follow Microsoft's C# coding conventions:

#### Naming Conventions

```csharp
// Classes, methods, properties - PascalCase
public class AccountService
{
    public string UserName { get; set; }
    
    public async Task<Account> GetAccountAsync(int id)
    {
        // Implementation
    }
}

// Fields, parameters, local variables - camelCase
private readonly IAccountRepository _accountRepository;

public void ProcessAccount(string userName)
{
    var processedAccount = userName.ToLower();
}

// Constants - PascalCase
public const string DefaultUserName = "guest";

// Interfaces - IPascalCase
public interface IAccountService
{
    // Interface members
}
```

#### Code Organization

```csharp
// File organization
using System;                          // System namespaces first
using System.Collections.Generic;
using System.Threading.Tasks;
                                       // Blank line
using Microsoft.AspNetCore.Mvc;       // Microsoft namespaces
using Microsoft.EntityFrameworkCore;
                                       // Blank line
using CleanTemplate.Application.Domain;     // Project namespaces
using Foundation.Base.Util;

namespace CleanTemplate.API.Controllers // Namespace matches folder structure
{
    /// <summary>
    /// XML documentation for public members
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CleanEntityController : ControllerBase
    {
        // Private fields first
        private readonly IAccountService _accountService;
        
        // Constructor
        public CleanEntityController(IAccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }
        
        // Public methods
        // Private methods last
    }
}
```

#### Code Quality

```csharp
// Use async/await properly
public async Task<IActionResult> GetAccountAsync(int id)
{
    if (id <= 0)
        return BadRequest("Invalid account ID");
        
    var account = await _accountService.GetAccountAsync(id);
    return account != null ? Ok(account) : NotFound();
}

// Null checking
public void ProcessAccount(Account account)
{
    ArgumentNullException.ThrowIfNull(account);
    
    // Or for older .NET versions:
    // if (account == null) throw new ArgumentNullException(nameof(account));
}

// Use meaningful variable names
var authenticatedAccount = await AuthenticateUserAsync(credentials);
var hasValidPermissions = CheckUserPermissions(authenticatedAccount);

// Prefer explicit types when unclear
Dictionary<string, List<Claim>> userClaims = new();
```

### Project Structure

```
Src/
├── CleanTemplate.API/              # Web API Layer
│   ├── Controllers/                # API Controllers
│   ├── Middleware/                # Custom middleware
│   ├── Extensions/                # Extension methods
│   ├── Filters/                   # Action filters
│   └── Program.cs                 # Application entry point
│
├── CleanTemplate.Application/           # Domain & Business Logic
│   ├── Domain/                    # Domain entities and interfaces
│   │   ├── Implementation/        # Concrete entities
│   │   └── Interface/            # Domain interfaces
│   ├── Services/                  # Business services
│   │   ├── Interface/            # Service contracts
│   │   └── Implementation/       # Service implementations
│   ├── Repository/                # Data access layer
│   │   ├── Interface/            # Repository contracts
│   │   └── Implementation/       # Repository implementations
│   ├── DTO/                      # Data transfer objects
│   ├── Mapping/                  # Object mapping profiles
│   ├── Validation/               # Validation logic
│   └── Extensions/               # Extension methods
│
└── Tests/                         # Test projects
    ├── CleanTemplate.API.Tests/  # API tests
    ├── CleanTemplate.Application.Tests/ # Domain tests
    └── Authentication.Integration.Tests/ # Integration tests
```

## 🧪 Testing Guidelines

### Test Structure

Follow the AAA pattern (Arrange, Act, Assert):

```csharp
[Test]
public async Task GetAccountAsync_WithValidId_ReturnsAccount()
{
    // Arrange
    var accountId = 1;
    var expectedAccount = new Account { Id = accountId, UserName = "testuser" };
    _mockRepository.Setup(r => r.GetByIdAsync(accountId))
                  .ReturnsAsync(expectedAccount);
    
    // Act
    var result = await _accountService.GetAccountAsync(accountId);
    
    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Id, Is.EqualTo(accountId));
    Assert.That(result.UserName, Is.EqualTo("testuser"));
}
```

### Test Categories

```csharp
// Unit tests - test individual components
[TestFixture]
[Category("Unit")]
public class AccountServiceTests
{
    // Test implementations
}

// Integration tests - test component interactions
[TestFixture]
[Category("Integration")]
public class AccountRepositoryIntegrationTests
{
    // Test implementations
}

// End-to-end tests - test complete workflows
[TestFixture]
[Category("E2E")]
public class CleanTemplateWorkflowTests
{
    // Test implementations
}
```

### Test Naming

```csharp
// Method_Scenario_ExpectedResult
[Test]
public void ValidatePassword_WithValidPassword_ReturnsTrue()

[Test]
public void ValidatePassword_WithNullPassword_ThrowsArgumentNullException()

[Test]
public async Task AuthenticateAsync_WithInvalidCredentials_ReturnsNull()
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific category
dotnet test --filter Category=Unit

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test
dotnet test --filter "Name~GetAccountAsync_WithValidId_ReturnsAccount"
```

## 📖 Documentation

### Code Documentation

Use XML documentation for public APIs:

```csharp
/// <summary>
/// Authenticates a user with the provided credentials.
/// </summary>
/// <param name="credentials">The user credentials containing username and password.</param>
/// <returns>A JWT token if authentication is successful; otherwise, null.</returns>
/// <exception cref="ArgumentNullException">Thrown when credentials is null.</exception>
/// <exception cref="ValidationException">Thrown when credentials are invalid.</exception>
public async Task<string?> AuthenticateAsync(UserCredentials credentials)
{
    // Implementation
}
```

### README Updates

When adding features, update the README:

- Add new endpoints to API section
- Update feature list
- Include configuration examples
- Add usage examples

### API Documentation

Update Swagger documentation:

```csharp
[HttpPost("authenticate")]
[SwaggerOperation(
    Summary = "Authenticate user",
    Description = "Authenticates a user and returns a JWT token",
    Tags = new[] { "Authentication" }
)]
[SwaggerResponse(200, "Authentication successful", typeof(TokenResponse))]
[SwaggerResponse(400, "Invalid credentials", typeof(ValidationProblemDetails))]
[SwaggerResponse(401, "Authentication failed")]
public async Task<IActionResult> AuthenticateAsync([FromBody] AuthRequest request)
{
    // Implementation
}
```

## 🔍 Review Process

### Before Submitting

- [ ] Code builds without warnings
- [ ] All tests pass
- [ ] Code follows style guidelines
- [ ] Documentation is updated
- [ ] No sensitive information in code
- [ ] Performance impact considered

### Pull Request Checklist

- [ ] Descriptive title and description
- [ ] References related issues
- [ ] Includes tests for new functionality
- [ ] Documentation updated
- [ ] Breaking changes documented
- [ ] Screenshots for UI changes

### Review Criteria

Reviewers will check:

1. **Functionality**: Does the code work as intended?
2. **Quality**: Is the code well-written and maintainable?
3. **Testing**: Are there adequate tests?
4. **Documentation**: Is documentation updated?
5. **Security**: Are there any security concerns?
6. **Performance**: Is performance acceptable?

### Addressing Feedback

- Respond to all review comments
- Make requested changes promptly
- Ask questions if feedback is unclear
- Update the PR description if scope changes

## 🏷️ Release Process

### Versioning

We follow [Semantic Versioning](https://semver.org/):

- **MAJOR** (1.0.0): Breaking changes
- **MINOR** (1.1.0): New features, backward compatible
- **PATCH** (1.1.1): Bug fixes, backward compatible

### Release Types

- **Alpha**: Early development versions (1.0.0-alpha.1)
- **Beta**: Feature-complete, testing phase (1.0.0-beta.1)
- **Release Candidate**: Production-ready candidate (1.0.0-rc.1)
- **Stable**: Production release (1.0.0)

## 📞 Getting Help

### Communication Channels

- **GitHub Issues**: Bug reports and feature requests
- **GitHub Discussions**: Questions and general discussion
- **Email**: maintainers@authentication-service.com

### Resources

- [Architecture Documentation](./docs/ARCHITECTURE.md)
- [API Documentation](./docs/API.md)
- [Deployment Guide](./docs/DEPLOYMENT.md)
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core Docs](https://docs.microsoft.com/en-us/ef/core/)

## 🎉 Recognition

Contributors are recognized in:

- **CONTRIBUTORS.md**: List of all contributors
- **Release Notes**: Major contributors mentioned
- **GitHub**: Contributor graphs and statistics

Thank you for contributing to the CleanTemplate service! 🚀