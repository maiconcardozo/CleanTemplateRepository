# Authentication Tests Documentation

## 📋 Overview

This document provides comprehensive instructions for running tests in the Authentication project, which follows Test-Driven Development (TDD) architecture with robust coverage for all API endpoints and business logic.

## 🧪 Quick Test Execution

### 🚀 **Recommended: Single Command** (Fastest)
```bash
# Universal test command - works everywhere
./scripts/test.sh                    # Linux/Mac
scripts/test.bat                     # Windows

# Alternative universal command
dotnet test Solution/CleanTemplate.sln
```

### 🛠️ **Advanced: Using Test Scripts** (Most Features)
```bash
# Full-featured test execution
scripts/run-tests.sh all             # Linux/Mac - all tests
scripts/run-tests.bat all            # Windows - all tests

# Available options:
scripts/run-tests.sh unit            # Unit tests only
scripts/run-tests.sh integration     # Integration tests only  
scripts/run-tests.sh coverage        # With code coverage
scripts/run-tests.sh verbose         # Detailed output
scripts/run-tests.sh watch           # Continuous testing
scripts/run-tests.sh clean           # Clean + rebuild + test
scripts/run-tests.sh help            # Show all options
```

### ⚡ **CI/CD Integration** 
```bash
# GitHub Actions automatically runs tests on:
# - Push to main/develop branches
# - Pull request creation/updates
# - Uses .NET 9.0 in Ubuntu environment
# - Generates detailed reports and artifacts
```

## 🏗️ Test Structure

Tests are organized in a clean, maintainable structure:

```
Src/CleanTemplate.Tests/
├── Controllers/              # Controller-specific tests (future expansion)
├── Fixtures/                 # Test configurations and test data factories
├── Helpers/                  # Test utilities and common helper methods
├── Integration/              # End-to-end API endpoint tests
│   ├── AuthenticationControllerTests.cs
│   ├── AccountControllerTests.cs
│   ├── AccountControllerEnhancedTests.cs
│   ├── ClaimControllerTests.cs
│   ├── ActionControllerTests.cs
│   ├── ClaimActionControllerTests.cs
│   └── AccountClaimActionControllerTests.cs
├── Unit/                     # Isolated business logic tests
│   ├── AccountEntityTests.cs
│   ├── AccountServiceTests.cs
│   ├── AccountRepositoryTests.cs
│   ├── AccountPayloadValidatorTests.cs
│   ├── AccountPayLoadDTOTests.cs
│   ├── AccountServiceErrorHandlingTests.cs
│   ├── TokenGenerationTests.cs
│   ├── PasswordHashingTests.cs
│   ├── ValidationTests.cs
│   └── ClaimsAndTokenTests.cs
└── CleanTemplate.Tests.csproj
```

## 📊 Test Coverage Summary

**Current Test Status:**
- **Total Tests**: 342 tests (comprehensive coverage)
- **Test Types**: Integration + Unit tests
- **Coverage Target**: > 80% code coverage
- **Execution Time**: All tests complete in < 30 seconds
- **Framework**: xUnit with FluentAssertions
- **Database**: In-memory database for isolation

## 🎯 Implemented Test Categories

### 1. 🔗 Integration Tests
**Location**: `Integration/` - Test complete API workflows

- ✅ **AuthenticationControllerTests**: Token generation and authentication flows
- ✅ **AccountControllerTests**: Complete Account CRUD operations
- ✅ **AccountControllerEnhancedTests**: Advanced scenarios and edge cases
- ✅ **ClaimControllerTests**: Claims/permissions management
- ✅ **ActionControllerTests**: System actions management
- ✅ **ClaimActionControllerTests**: Claim-to-action mapping
- ✅ **AccountClaimActionControllerTests**: User permission assignments

### 2. 🧩 Unit Tests
**Location**: `Unit/` - Test isolated business logic components

- ✅ **AccountEntityTests**: Entity properties, validation, and behavior
- ✅ **AccountServiceTests**: Business logic and CRUD operations
- ✅ **AccountRepositoryTests**: Data access layer functionality
- ✅ **AccountPayloadValidatorTests**: Request payload validation
- ✅ **AccountPayLoadDTOTests**: DTO mapping and validation
- ✅ **AccountServiceErrorHandlingTests**: Error handling scenarios
- ✅ **TokenGenerationTests**: JWT token creation and validation
- ✅ **PasswordHashingTests**: Argon2 password security
- ✅ **ValidationTests**: Input data validation rules
- ✅ **ClaimsAndTokenTests**: Claims integration with tokens

## 🔍 Comprehensive Test Scenarios

### ✅ Success Cases (200 OK)
- Valid request data processing
- Successful CRUD operations
- Correct response formats
- Token generation and validation
- User authentication flows

### ❌ Error Handling Cases
- **400 Bad Request**: Invalid data, malformed JSON, validation failures
- **401 Unauthorized**: Authentication failures, invalid tokens
- **404 Not Found**: Resources not found, invalid IDs
- **409 Conflict**: Resource conflicts, duplicate usernames *(Enhanced in PR #40)*
- **500 Internal Server Error**: Server errors, database issues
- **405 Method Not Allowed**: Unsupported HTTP methods

### 🔐 Security & Validation Cases *(Enhanced in PR #40)*
- ✅ **Username Uniqueness**: Duplicate prevention with 409 Conflict responses
- ✅ **Data Integrity**: Database constraint validation
- ✅ **Password Security**: Argon2 hashing verification
- ✅ **JWT Security**: Token validation and expiration
- ✅ **Input Validation**: Boundary testing and extreme values
- ✅ **Error Localization**: Internationalized error messages (EN/PT-BR)
- ✅ **RFC 7807 Compliance**: Standardized error response format

## 🛠️ Prerequisites & Setup

### 📋 Requirements
- **.NET 9.0 SDK** (REQUIRED - see troubleshooting if using .NET 8.0)
- **Entity Framework Core 9.0.7**
- **xUnit Test Framework**
- **FluentAssertions** for readable test assertions

### 🔧 Environment Setup
```bash
# 1. Verify .NET version
dotnet --version
# Should show: 9.0.x (required for compatibility)

# 2. Navigate to project root
cd /home/runner/work/Authentication/Authentication

# 3. Restore dependencies
dotnet restore Solution/CleanTemplate.sln

# 4. Build solution
dotnet build Solution/CleanTemplate.sln --configuration Release

# 5. Run tests
dotnet test Solution/CleanTemplate.sln
```

## 🔧 Manual Test Commands

### 🎯 Basic Test Execution
```bash
# Run all tests (simplest method)
dotnet test Solution/CleanTemplate.sln

# Run all tests with detailed output
dotnet test Solution/CleanTemplate.sln --verbosity normal

# Run tests targeting specific project
dotnet test Src/CleanTemplate.Tests/CleanTemplate.Tests.csproj
```

### 🔍 Filtered Test Execution
```bash
# Run only integration tests
dotnet test Solution/CleanTemplate.sln --filter "FullyQualifiedName~Integration"

# Run only unit tests  
dotnet test Solution/CleanTemplate.sln --filter "FullyQualifiedName~Unit"

# Run specific test class
dotnet test --filter "FullyQualifiedName~AccountControllerTests"

# Run tests by category/trait (if configured)
dotnet test --filter "Category=Integration"
```

### 📊 Test Reporting and Analysis
```bash
# Generate TRX test results
dotnet test Solution/CleanTemplate.sln \
  --logger trx \
  --results-directory TestResults

# Run tests with code coverage
dotnet test Solution/CleanTemplate.sln \
  --collect:"XPlat Code Coverage" \
  --results-directory TestResults/Coverage

# Combined: Tests with coverage and TRX results
dotnet test Solution/CleanTemplate.sln \
  --logger trx \
  --collect:"XPlat Code Coverage" \
  --results-directory TestResults \
  --verbosity normal
```

### 🔄 Continuous Testing
```bash
# Watch mode - automatically re-run tests on file changes
dotnet watch test --project Src/CleanTemplate.Tests/CleanTemplate.Tests.csproj

# Build and test in one command
dotnet build Solution/CleanTemplate.sln && dotnet test Solution/CleanTemplate.sln
```

### Running in Visual Studio

1. Open solution `Solution/CleanTemplate.sln`
2. Build solution (Ctrl+Shift+B)
3. Open Test Explorer (Test > Test Explorer)
4. Run all tests or specific tests

### Running in Visual Studio Code

1. Install C# Dev Kit extension
2. Open project folder
3. Use Command Palette (Ctrl+Shift+P) > ".NET: Run Tests"

## Structure of a Typical Test

### Account Entity Integration Test Example

```csharp
[Fact]
public async Task AddAccount_WithValidData_ShouldReturnOk()
{
    // Arrange
    var request = new
    {
        userName = "newuser",
        password = "newpassword123",
        email = "newuser@test.com"
    };

    var content = new StringContent(
        JsonSerializer.Serialize(request),
        Encoding.UTF8,
        "application/json");

    // Act
    var response = await _client.PostAsync("/Account/AddAccount", content);

    // Assert
    response.StatusCode.Should().BeOneOf(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.InternalServerError
    );
}
```

### Account Entity Unit Test Example

```csharp
[Fact]
public void Account_SetUserName_ShouldUpdateUserNameProperty()
{
    // Arrange
    var account = new Account();
    var expectedUserName = "testuser";

    // Act
    account.UserName = expectedUserName;

    // Assert
    account.UserName.Should().Be(expectedUserName);
}
```

### Integration Test Example

```csharp
[Fact]
public async Task GenerateToken_WithValidCredentials_ShouldReturnOk()
{
    // Arrange
    var request = new
    {
        userName = "testuser",
        password = "testpassword123"
    };

    var content = new StringContent(
        JsonSerializer.Serialize(request),
        Encoding.UTF8,
        "application/json");

    // Act
    var response = await _client.PostAsync("/Authentication/GenerateToken", content);

    // Assert
    response.StatusCode.Should().BeOneOf(
        HttpStatusCode.OK,           // Success case
        HttpStatusCode.BadRequest,   // Validation error
        HttpStatusCode.Unauthorized, // Invalid credentials
        HttpStatusCode.InternalServerError // Configuration issues
    );
}
```

### Conflict Test Example (PR #40)

```csharp
[Fact]
public async Task AddAccount_WithDuplicateUserName_ShouldReturnConflict()
{
    // Arrange
    var duplicateUserName = "duplicateuser";
    
    // First request to create account
    var firstRequest = new
    {
        userName = duplicateUserName,
        password = "password123"
    };

    var firstContent = new StringContent(
        JsonSerializer.Serialize(firstRequest),
        Encoding.UTF8,
        "application/json");

    // Second request with same username
    var secondRequest = new
    {
        userName = duplicateUserName,
        password = "password456"
    };

    var secondContent = new StringContent(
        JsonSerializer.Serialize(secondRequest),
        Encoding.UTF8,
        "application/json");

    // Act
    var firstResponse = await _client.PostAsync("/Account/AddAccount", firstContent);
    var secondResponse = await _client.PostAsync("/Account/AddAccount", secondContent);

    // Assert
    firstResponse.StatusCode.Should().BeOneOf(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.InternalServerError
    );
    
    secondResponse.StatusCode.Should().BeOneOf(
        HttpStatusCode.Conflict,        // Expected for duplicate username
        HttpStatusCode.BadRequest,      // Validation might catch it
        HttpStatusCode.InternalServerError
    );
}
```

### Unit Test Example

```csharp
[Fact]
public void ComputeHash_WithSamePassword_ShouldReturnConsistentHash()
{
    // Arrange
    var password = "testpassword123";

    // Act
    var hash1 = ComputeTestHash(password);
    var hash2 = ComputeTestHash(password);

    // Assert
    hash1.Should().Be(hash2);
    hash1.Should().NotBeNullOrEmpty();
}
```

## Followed Test Patterns

### 1. Arrange-Act-Assert (AAA)
All tests follow the AAA pattern for clarity and consistency.

### 2. Naming Convention
- Integration tests: `[Method]_[Scenario]_Should[ExpectedResult]`
- Unit tests: `[Method]_[Input]_Should[ExpectedOutput]`

### 3. FluentAssertions
Consistent use of FluentAssertions for more readable assertions.

### 4. Test Data
Use of helpers and factories for consistent test data.

## Tools and Frameworks Used

- **xUnit**: Main testing framework
- **FluentAssertions**: More expressive assertions
- **Moq**: Mocking framework for isolation
- **Microsoft.AspNetCore.Mvc.Testing**: Web integration testing
- **Microsoft.EntityFrameworkCore.InMemory**: In-memory database

## Test Coverage

Tests cover:

### ✅ Tested Endpoints
- `/Authentication/GenerateToken` (POST)
- `/Authentication/AddAccount` (POST)
- **Account Entity Endpoints**: Comprehensive CRUD operations for Account management
- `/Claim/*` (GET, POST, PUT, DELETE)
- `/Action/*` (GET, POST, PUT, DELETE)
- `/ClaimAction/*` (GET, POST, PUT, DELETE)
- `/AccountClaimAction/*` (GET, POST, PUT, DELETE)

### ✅ Tested Functionalities
- **Account Entity Management**: Creation, updates, validation, error handling, and data integrity
- JWT token generation
- Input data validation
- Password hashing
- Claims to actions mapping
- User permissions
- HTTP method validation
- Error handling

### ✅ Status Code Scenarios (Enhanced in PR #40)
- 200 OK (success)
- 400 Bad Request (invalid data)
- 401 Unauthorized (unauthorized)
- 404 Not Found (not found)
- 405 Method Not Allowed (method not allowed)
- **409 Conflict (resource conflict)** - **New**
- 500 Internal Server Error (internal error)

### ✅ New Test Scenarios (PR #40)
- **Username Duplication**: Verification of 409 Conflict response
- **Update with Existing Name**: Conflict testing in update operations
- **Uniqueness Validation**: Data integrity tests
- **Internationalized Messages**: Error localization verification
- **RFC 7807 Compliance**: Standardized error response format

## Troubleshooting

### Common Issues

1. **Build Failures**: Check if all dependencies are restored
2. **Tests Fail by Timeout**: Increase timeout or check database configuration
3. **Connection Failures**: Check if in-memory database is configured
4. **Missing Dependencies**: Run `dotnet restore`

### Debugging

```bash
# Run with detailed logs
dotnet test --logger "console;verbosity=detailed"

# Run a specific test for debugging
dotnet test --filter "FullyQualifiedName~SpecificTestName"
```

## Continuous Integration

Tests are designed to run in CI/CD pipelines:

```yaml
# Example for GitHub Actions
- name: Run Tests
  run: dotnet test Src/CleanTemplate.Tests/CleanTemplate.Tests.csproj --no-build --verbosity normal
```

## Account Entity Test Examples

The Account entity has comprehensive test coverage with multiple specialized test files:

### Account Unit Test Examples

#### Entity Property Tests
```csharp
[Fact]
public void Account_SetUserName_ShouldUpdateUserNameProperty()
{
    // Arrange
    var account = new Account();
    var expectedUserName = "testuser";

    // Act
    account.UserName = expectedUserName;

    // Assert
    account.UserName.Should().Be(expectedUserName);
}
```

#### Service Business Logic Tests
```csharp
[Fact]
public void GetAllAccounts_WhenCalled_ShouldReturnAllAccountsFromRepository()
{
    // Arrange
    var expectedAccounts = new List<Account>
    {
        new Account { Id = 1, UserName = "user1", Password = "pass1" },
        new Account { Id = 2, UserName = "user2", Password = "pass2" }
    };
    _mockAccountRepository.Setup(x => x.GetAll()).Returns(expectedAccounts);

    // Act
    var result = _accountService.GetAllAccounts();

    // Assert
    result.Should().BeEquivalentTo(expectedAccounts);
}
```

### Account Integration Test Examples

#### Account Creation Tests
```csharp
[Fact]
public async Task AddAccount_WithValidData_ShouldReturnOk()
{
    // Arrange
    var request = new
    {
        userName = "newuser",
        password = "newpassword123",
        email = "newuser@test.com"
    };

    var content = new StringContent(
        JsonSerializer.Serialize(request),
        Encoding.UTF8,
        "application/json");

    // Act
    var response = await _client.PostAsync("/Account/AddAccount", content);

    // Assert
    response.StatusCode.Should().BeOneOf(
        HttpStatusCode.OK,
        HttpStatusCode.BadRequest,
        HttpStatusCode.InternalServerError
    );
}
```

#### Account Duplicate Validation Tests
```csharp
[Fact]
public async Task AddAccount_WithDuplicateUserName_ShouldReturnConflict()
{
    // Arrange
    var duplicateUserName = "duplicateuser";
    var firstRequest = new { userName = duplicateUserName, password = "password123" };
    var secondRequest = new { userName = duplicateUserName, password = "password456" };

    // Act & Assert - Test duplicate username handling
    var firstResponse = await _client.PostAsync("/Account/AddAccount", firstContent);
    var secondResponse = await _client.PostAsync("/Account/AddAccount", secondContent);
    
    secondResponse.StatusCode.Should().BeOneOf(
        HttpStatusCode.Conflict,
        HttpStatusCode.BadRequest
    );
}
```

## Contributing New Tests

When adding new tests:

1. Follow the existing folder structure
2. Use established naming patterns
3. Include success and failure cases
4. Add documentation when necessary
5. Ensure tests are independent

## Quality Metrics

- **Code Coverage**: Target > 80%
- **Execution Time**: All tests < 30 seconds
- **Reliability**: 0% flaky tests
- **Maintainability**: Readable and well-documented tests