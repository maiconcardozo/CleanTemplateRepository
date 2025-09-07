# 🏗️ Architecture Documentation Template

## Overview

This document serves as a template for documenting your .NET application architecture. The template follows **Clean Architecture** principles, ensuring separation of concerns, testability, and maintainability. Built on **.NET 9.0** for modern development features and enhanced performance.

## Architecture Layers

### 1. Domain Layer (`YourProject.Domain`)

The innermost layer containing business entities and core business rules.

```
YourProject.Domain/
├── Entities/                 # Domain entities
│   ├── YourEntity.cs        # Your business entities
│   └── ValueObjects/        # Value objects
│
├── Interfaces/              # Domain interfaces
│   ├── IRepository.cs       # Repository contracts
│   └── IDomainService.cs    # Domain service contracts
│
└── Services/                # Domain services
    └── YourDomainService.cs # Business logic
```

**Key Characteristics:**
- No dependencies on external layers
- Contains business rules and entities
- Framework agnostic
- Highly testable

### 2. Application Layer (`YourProject.Application`)

Contains business logic and use cases that orchestrate domain entities.

```
YourProject.Application/
├── Interfaces/              # Service contracts
│   ├── IYourService.cs     # Application service interfaces
│   └── IUseCase.cs         # Use case interfaces
│
├── Services/                # Service implementations
│   ├── YourService.cs      # Application services
│   └── UseCases/           # Use case implementations
│
├── DTOs/                    # Data transfer objects
│   ├── YourRequestDTO.cs   # Request models
│   └── YourResponseDTO.cs  # Response models
│
└── Validators/              # Input validation
    ├── YourValidator.cs    # FluentValidation validators
    └── ValidationRules/    # Custom validation rules
```

**Key Characteristics:**
- Orchestrates domain entities
- Contains application-specific business rules
- Depends only on Domain layer
- Framework agnostic

### 3. Infrastructure Layer (`YourProject.Infrastructure`)

Implements external concerns like data access, external services, and frameworks.

```
YourProject.Infrastructure/
├── Data/                    # Data access
│   ├── Contexts/           # EF DbContexts
│   ├── Configurations/     # Entity configurations
│   └── Repositories/       # Repository implementations
│
├── Services/               # External services
│   ├── EmailService.cs    # Email implementation
│   └── FileService.cs     # File handling
│
└── Extensions/             # Infrastructure extensions
    └── ServiceExtensions.cs # DI configuration
```

**Key Characteristics:**
- Implements infrastructure concerns
- Contains EF Core configurations
- Implements repository patterns
- Handles external service integrations

### 4. Presentation Layer (`YourProject.API`)

The outermost layer that handles HTTP requests and responses.

```
YourProject.API/
├── Controllers/             # API Controllers
│   ├── YourController.cs   # REST endpoints
│   └── BaseController.cs   # Shared controller logic
│
├── Middleware/             # Custom middleware
│   ├── ErrorHandling.cs   # Global error handling
│   └── Authentication.cs  # Auth middleware
│
├── Extensions/             # API extensions
│   └── ServiceExtensions.cs # API service configuration
│
└── Models/                 # API models
    ├── Requests/           # Request models
    └── Responses/          # Response models
```

**Key Characteristics:**
- Handles HTTP requests/responses
- Contains controllers and middleware
- Depends on Application layer
- Framework specific (ASP.NET Core)

## Technology Stack Template

### Core Framework
- **.NET 9.0** - Main framework
- **ASP.NET Core 9.0** - Web API framework
- **C# 13** - Programming language

### Data Access
- **Entity Framework Core 9.0** - ORM
- **Your Database** - Primary database (SQL Server, PostgreSQL, MySQL, etc.)

### Validation & Mapping
- **FluentValidation** - Input validation
- **AutoMapper** - Object mapping

### Testing Framework
- **xUnit** - Testing framework
- **FluentAssertions** - Assertion library
- **Moq** - Mocking framework

### API Documentation
- **Swagger/OpenAPI** - API documentation
- **Swashbuckle** - Swagger implementation

## Design Patterns

### Repository Pattern
Abstracts data access logic and provides a uniform interface for accessing data.

### Unit of Work Pattern
Manages transactions and coordinates changes across multiple repositories.

### Dependency Injection
Uses built-in .NET DI container for loose coupling and testability.

### CQRS (Optional)
Separates read and write operations for complex scenarios.

## Security Architecture

### Authentication
- **JWT Tokens** - Token-based authentication
- **Identity Framework** - User management
- **Custom Authentication** - Project-specific auth logic

### Authorization
- **Role-based Authorization** - Role permissions
- **Policy-based Authorization** - Custom policies
- **Resource-based Authorization** - Context-specific permissions

### Data Protection
- **Password Hashing** - Secure password storage (bcrypt, Argon2)
- **Data Encryption** - Sensitive data encryption
- **HTTPS** - Secure communication

## Configuration Architecture

### Environment Configuration
- **appsettings.json** - Base configuration
- **appsettings.{Environment}.json** - Environment-specific settings
- **Environment Variables** - Runtime configuration
- **Azure Key Vault** - Secret management (if applicable)

### Dependency Injection Configuration
```csharp
// Extension method for clean DI setup
public static IServiceCollection AddYourProjectServices(
    this IServiceCollection services, 
    string connectionString)
{
    // Configure your services here
    return services;
}
```

## Performance Considerations

### Database Optimization
- **Indexing Strategy** - Optimized database indexes
- **Query Optimization** - Efficient LINQ queries
- **Connection Pooling** - Database connection management

### Caching Strategy
- **Memory Caching** - In-memory cache for frequently accessed data
- **Distributed Caching** - Redis for scalable caching
- **Response Caching** - HTTP response caching

### Async/Await Pattern
- **Asynchronous Operations** - Non-blocking I/O operations
- **Task-based APIs** - Scalable async patterns

---

*This template should be customized for your specific project. Remove this section and update the content with your actual architecture details.*
    ├── AccountService.cs   # User management logic
    ├── AuthenticationService.cs # Authentication/token logic
    ├── ClaimService.cs     # Permission management
    ├── ActionService.cs    # Action management
    ├── ClaimActionService.cs # Permission mapping
    └── AccountClaimActionService.cs # User permission assignment
```

**Key Features:**
- Business logic implementation
- Transaction management
- Validation orchestration
- Domain entity coordination

### 3. Infrastructure Layer (`Authentication.Login/Repository` & `Authentication.Login/Infrastructure`)

Handles data persistence, external services, and framework-specific implementations.

```
Authentication.Login/
├── Repository/
│   ├── Interface/          # Repository contracts
│   │   ├── IAccountRepository.cs
│   │   ├── IClaimRepository.cs
│   │   ├── IActionRepository.cs
│   │   ├── IClaimActionRepository.cs
│   │   └── IAccountClaimActionRepository.cs
│   │
│   └── Implementation/     # Data access implementations
│       ├── AccountRepository.cs
│       ├── ClaimRepository.cs
│       ├── ActionRepository.cs
│       ├── ClaimActionRepository.cs
│       └── AccountClaimActionRepository.cs
│
├── Infrastructure/
│   ├── Data/              # Database configurations
│   │   ├── LoginContext.cs
│   │   └── Implementation/
│   │       ├── AccountMap.cs
│   │       ├── ClaimMap.cs
│   │       ├── ActionMap.cs
│   │       ├── ClaimActionMap.cs
│   │       └── AccountClaimActionMap.cs
│   │
│   └── Implementation/    # Entity mappings
│
└── UnitOfWork/           # Transaction management
    ├── Interface/
    │   └── ILoginUnitOfWork.cs
    └── Implementation/
        └── LoginUnitOfWork.cs
```

**Key Features:**
- Database access through Entity Framework Core
- Repository pattern implementation
- Unit of Work for transaction management
- Entity Framework configurations

### 4. Presentation Layer (`Authentication.API`)

The outermost layer handling HTTP requests, responses, and API concerns.

```
Authentication.API/
├── Controllers/            # API endpoints
│   ├── AuthenticationController.cs    # Authentication endpoints
│   ├── ClaimController.cs             # Claims management
│   ├── ActionController.cs            # Actions management
│   ├── ClaimActionController.cs       # Claim-action mappings
│   └── AccountClaimActionController.cs # User permissions
│
├── Middleware/            # HTTP pipeline components
│   ├── ExceptionHandlingMiddleware.cs
│   └── SwaggerAuthMiddleware.cs
│
├── Swagger/               # API documentation
│   ├── Examples/
│   ├── Filters/
│   └── Routes/           # Route constants for all endpoints
│
├── Data/                  # API-specific contexts
│   ├── ApiContextDevelopment.cs
│   └── ApiContextProduction.cs
│
└── Program.cs             # Application startup
```

**Key Features:**
- RESTful API endpoints
- Request/Response handling
- Authentication middleware
- API documentation with Swagger
- Dependency injection configuration

## Foundation Layer (`Foundation.Base`)

**External Dependency:** The Foundation.Base library is maintained in a separate repository at [https://github.com/maiconcardozo/Foundation](https://github.com/maiconcardozo/Foundation). You must clone both repositories to the same parent directory for the project references to work correctly.

**Repository Structure Required:**
```
Parent Directory/
├── Foundation/
│   └── Src/
│       └── Foundation.Base/
│           └── Foundation.Base.csproj
└── Authentication/
    └── Src/
        ├── Authentication.API/
        └── Authentication.Login/
```

Shared library providing common patterns and utilities across all layers.

```
Foundation.Base/
├── Domain/
│   └── Implementation/
│       └── Entity.cs       # Base entity class
│
├── Repository/
│   ├── Interface/
│   │   └── IEntityRepository.cs
│   └── Implementation/
│       └── EntityRepository.cs
│
├── UnitOfWork/
│   ├── Interface/
│   │   ├── IUnitOfWork.cs
│   │   └── IBaseUnitOfWork.cs
│   └── Implementation/
│       ├── UnitOfWork.cs
│       └── BaseUnitOfWork.cs
│
├── Infrastructure/
│   └── Data/
│       ├── EntityConfiguration.cs
│       └── EntityMap.cs
│
├── Util/
│   ├── StringHelper.cs
│   ├── HashUtil.cs
│   └── ValidationHelper.cs
│
└── Resource/
    └── ResourceFoundation.cs
```

## RBAC (Role-Based Access Control) Architecture

### Entity Relationships

The authentication service implements a comprehensive RBAC system with the following entities and relationships:

```
┌─────────────┐    ┌──────────────────┐    ┌─────────────┐
│   Account   │    │ AccountClaimAction│   │ ClaimAction │
│             │    │                  │   │             │
│ - Id        │◄───┤ - AccountId      │──►│ - Id        │
│ - UserName  │    │ - ClaimActionId  │   │ - ClaimId   │
│ - Password  │    │ - CreatedAt      │   │ - ActionId  │
│ - CreatedAt │    │ - UpdatedAt      │   │ - CreatedAt │
│ - UpdatedAt │    └──────────────────┘   │ - UpdatedAt │
└─────────────┘                           └─────────────┘
                                                 │
                    ┌─────────────┐              │
                    │   Claim     │◄─────────────┘
                    │             │
                    │ - Id        │
                    │ - Type      │
                    │ - Value     │   ┌─────────────┐
                    │ - Description│   │   Action    │
                    │ - CreatedAt │   │             │
                    │ - UpdatedAt │   │ - Id        │
                    └─────────────┘   │ - Name      │
                                      │ - CreatedAt │
                                      │ - UpdatedAt │
                                      └─────────────┘
```

### Entity Descriptions

1. **Account**: Represents user accounts with basic authentication information
2. **Claim**: Defines permissions, roles, or features (e.g., "user:read", "admin", "feature:reports")
3. **Action**: Defines system operations (e.g., "CreateUser", "DeletePost", "ViewReports")
4. **ClaimAction**: Maps which actions are allowed for each claim
5. **AccountClaimAction**: Assigns specific permissions to user accounts

### Permission Flow

1. **Define Claims**: Create permission claims (roles, features, permissions)
2. **Define Actions**: Create system actions that can be performed
3. **Map Claims to Actions**: Define which actions each claim can perform
4. **Assign to Users**: Grant specific claim-action combinations to user accounts

### Example Permission Scenario

```
Claim: "editor"
↓
ClaimAction: "editor" can perform "CreatePost", "EditPost", "DeleteOwnPost"
↓
AccountClaimAction: User "john.doe" has "editor" permissions
↓
Result: John can create, edit posts, and delete his own posts
```

## Security Architecture

### Authentication Flow

1. **User Credentials**: Client sends username/password
2. **Validation**: Server validates credentials against database
3. **Token Generation**: JWT token created with user claims
4. **Token Response**: Token returned to client
5. **Protected Requests**: Client includes token in Authorization header
6. **Token Validation**: Server validates token for each protected request

### Security Layers

```
┌─────────────────────────┐
│     HTTPS/TLS           │ ← Transport Security
├─────────────────────────┤
│     CORS Policy         │ ← Cross-Origin Protection
├─────────────────────────┤
│     Input Validation    │ ← Data Validation
├─────────────────────────┤
│     JWT Authentication  │ ← Token-based Auth
├─────────────────────────┤
│     Authorization       │ ← Claim-based Access
├─────────────────────────┤
│     Password Hashing    │ ← Secure Storage
└─────────────────────────┘
```

## Configuration Architecture

### Environment-Based Configuration

```
appsettings.json                 ← Base configuration
├── appsettings.Development.json ← Development overrides
├── appsettings.Production.json  ← Production overrides
└── appsettings.Testing.json     ← Testing overrides
```

### Configuration Sections

- **ConnectionStrings**: Database connections
- **JwtSettings**: JWT token configuration
- **Logging**: Logging configuration
- **CORS**: Cross-origin settings

## Performance Considerations

### Scalability Patterns

1. **Horizontal Scaling**: Multiple API instances behind load balancer
2. **Database Optimization**: Connection pooling and query optimization
3. **Caching**: In-memory caching for frequently accessed data
4. **Async Operations**: Non-blocking I/O operations

### Monitoring Points

- **Response Times**: API endpoint performance
- **Database Queries**: Query execution time and frequency
- **Error Rates**: Application and system errors
- **Memory Usage**: Application memory consumption
- **CPU Utilization**: Processor usage patterns

## Technology Stack

### Core Framework
- **.NET 9.0**: Base framework providing runtime and BCL (REQUIRED - never downgrade to 8.0)
- **ASP.NET Core 9.0.7**: Web framework for REST API development
- **Entity Framework Core 9.0.7**: Object-relational mapper for data access

### Authentication & Security
- **JWT Bearer Tokens**: Stateless authentication mechanism
- **System.IdentityModel.Tokens.Jwt 8.14.0**: JWT implementation
- **Microsoft.AspNetCore.Authentication.JwtBearer 9.0.7**: JWT middleware

### Database & Data Access
- **MySQL 8.0+**: Primary database system
- **Pomelo.EntityFrameworkCore.MySql 9.0.0-rc.1**: MySQL provider for EF Core
- **MySqlConnector 2.4.0**: High-performance MySQL connector

### Validation & Mapping
- **FluentValidation 12.0.0**: Input validation library
- **AutoMapper 15.0.1**: Object-object mapping

### API Documentation
- **Swashbuckle.AspNetCore 6.8.1**: OpenAPI/Swagger documentation generation
- **Microsoft.AspNetCore.OpenApi 9.0.7**: OpenAPI support

### Testing Framework
- **xUnit 2.9.3**: Primary testing framework
- **Moq 4.20.72**: Mocking framework
- **FluentAssertions 8.6.0**: Fluent testing assertions
- **Microsoft.AspNetCore.Mvc.Testing 9.0.7**: Integration testing support

### External Dependencies
- **Foundation.Base 1.0.5**: Base library providing common patterns and utilities
- **Resource Usage**: CPU, memory, and database connections