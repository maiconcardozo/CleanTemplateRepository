# Changelog

All notable changes to the Authentication project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Fixed
- **Documentation Updates**: Fixed critical inconsistencies in documentation
  - Updated .NET version references from 8.0 to 9.0 across all documentation files
  - Fixed repository URLs from `maiconcardozo/Authentication` to `maiconcardozo/CleanTemplateRepository`
  - Corrected script paths in README and documentation files
  - Updated project references and examples to match current repository structure
- **CI/CD Pipeline**: Removed problematic test result publishing step that was causing "HttpError: Resource not accessible by integration" errors
  - Removed `dorny/test-reporter@v1` action that required special GitHub permissions
  - Test execution and artifact uploads continue to work normally
  - Test results are still available as downloadable artifacts

### Added
- **REST API Standards Compliance (PR #40)**: Complete implementation of proper HTTP status codes
  - **409 Conflict**: For duplicate username attempts in account creation/update
  - **404 Not Found**: For non-existent resources
  - **401 Unauthorized**: For authentication failures
  - **400 Bad Request**: For validation errors
  - **500 Internal Server Error**: For unexpected system errors
- **Database Schema Enhancement**: UNIQUE constraint on Account.UserName column
- **Enhanced Service Layer Validation**: Username uniqueness checks before database operations
- **ConflictException**: New exception class for proper conflict handling
- **Internationalized Error Messages**: English and Portuguese support for duplicate username errors
- **Enhanced Swagger Documentation**: Comprehensive status code examples and error responses
- **Comprehensive Integration Tests**: Covering duplicate scenarios and status code compliance
- **Complete RBAC System**: Full Role-Based Access Control implementation
  - Claims management with CRUD operations
  - Actions management with CRUD operations
  - ClaimAction mapping system
  - AccountClaimAction user permission assignments
- **New API Controllers**: 
  - `ClaimController` - Manage permission claims
  - `ActionController` - Manage system actions
  - `ClaimActionController` - Map claims to actions
  - `AccountClaimActionController` - Assign permissions to users
- **Comprehensive DTO Structure**: Request/Response DTOs for all entities
- **Enhanced API Documentation**: Complete Swagger documentation for all endpoints
- **Route Constants**: Organized route definitions for all endpoints
- Complete Foundation.Base library implementation
- Docker deployment support
- Health check endpoints
- JWT authentication with proper security
- Input validation with FluentValidation
- Repository and Unit of Work patterns
- Entity Framework Core 9.0 support

### Fixed
- **Code Standardization**: Fixed naming conventions across codebase
  - Renamed "Implemetation" folder to "Implementation" throughout project
  - Fixed "IClaimepository" to "IClaimRepository" interface name
  - Standardized method names (removed "Lst" prefix, used clear naming)
  - Fixed parameter naming conventions (camelCase)
  - Cleaned up duplicate/commented imports
- **Documentation Updates**: Updated README and architecture docs to reflect current structure
- **API Response Format**: Standardized data wrapper for successful responses (PR #40)
- **Error Handling**: Enhanced with proper HTTP status codes and RFC 7807 compliance (PR #40)
- MySQL database integration
- CORS configuration
- Exception handling middleware
- Logging and monitoring support

### Changed
- Updated to .NET 9.0 framework with Entity Framework Core 9.0
- Updated all packages to latest compatible versions
- Improved project structure following Clean Architecture
- Enhanced security with proper password hashing
- Standardized API response formats
- Improved error handling and validation

### Fixed
- Foundation.Base dependency issues
- MySQL migration compatibility
- Entity Framework configuration
- Project reference paths
- Package version conflicts
- Build and compilation errors

### Security
- **Enhanced Data Integrity (PR #40)**: Database-level username uniqueness constraints
- **Improved Error Handling**: Proper status codes prevent information leakage
- **Input Validation Enhancement**: Duplicate username prevention at service layer
- Implemented secure password hashing
- Added JWT token validation
- Enhanced input validation
- Secure configuration management
- Protected sensitive endpoints

## [1.0.0] - 2024-07-23

### Added
- Initial release of Authentication service
- Basic JWT token generation
- User account management
- MySQL database support
- Swagger documentation
- Docker configuration

### Features
- User authentication with JWT tokens
- Account creation and management
- Role-based access control with claims
- RESTful API design
- Cross-platform deployment support

### Documentation
- README with quick start guide
- API documentation with examples
- Architecture documentation
- Deployment guide for multiple platforms
- Contributing guidelines
- Comprehensive improvement documentation

### Technical Improvements
- Clean Architecture implementation
- Generic repository pattern
- Unit of Work for transaction management
- Dependency injection throughout
- Async/await best practices
- Comprehensive error handling
- Input validation and sanitization
- Security best practices implementation