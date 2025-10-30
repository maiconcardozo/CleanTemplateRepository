# 🔐 Authentication - .NET Authentication Service

<!-- Build and CI/CD Status Badges -->
[![CI/CD Pipeline](https://github.com/maiconcardozo/Authentication/actions/workflows/ci.yml/badge.svg)](https://github.com/maiconcardozo/Authentication/actions/workflows/ci.yml)

<!-- Technology and Framework Badges -->
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![C#](https://img.shields.io/badge/C%23-12.0-239120.svg?logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0.8-blue.svg?logo=nuget)](https://docs.microsoft.com/en-us/ef/core/)
[![JWT](https://img.shields.io/badge/JWT-8.14.0-green.svg?logo=json-web-tokens)](https://jwt.io/)

<!-- Quality -->
[![Lines of Code](https://img.shields.io/badge/Lines%20of%20Code-15.2k-informational?logo=visual-studio-code)](https://github.com/maiconcardozo/Authentication)
[![Code Coverage](https://img.shields.io/badge/Coverage-80%25+-brightgreen?logo=codecov)](https://github.com/maiconcardozo/Authentication)
[![Tests](https://img.shields.io/badge/Tests-288%20passing-brightgreen?logo=checkmarx)](https://github.com/maiconcardozo/Authentication)
[![Technical Debt](https://img.shields.io/badge/Tech%20Debt-Low-brightgreen?logo=sonarcloud)](https://github.com/maiconcardozo/Authentication)
[![License](https://img.shields.io/badge/License-MIT-blue.svg?logo=open-source-initiative)](LICENSE)
[![Last Commit](https://img.shields.io/badge/Last%20Commit-recent-brightgreen?logo=git)](https://github.com/maiconcardozo/Authentication/commits/main)
[![Contributors](https://img.shields.io/badge/Contributors-1-blue?logo=github)](https://github.com/maiconcardozo/Authentication/graphs/contributors)
[![Repository Size](https://img.shields.io/badge/Repository%20Size-~2.5MB-informational?logo=git)](https://github.com/maiconcardozo/Authentication)

## 📋 About

A production-ready .NET 8.0 authentication service implementing JWT token-based authentication and comprehensive Role-Based Access Control (RBAC). Built with Clean Architecture principles, it provides secure user management with Argon2 password hashing and a complete RESTful API.

### ✨ Key Features

- 🔑 **JWT Authentication** - Secure token-based authentication
- 👥 **User Management** - Complete account management with duplicate prevention
- 🛡️ **RBAC System** - Fine-grained permissions with Claims, Actions, and mappings
- 🔒 **Argon2 Security** - Industry-standard password hashing
- 🌐 **RESTful API** - Complete CRUD operations with proper HTTP status codes
- 🌍 **Internationalization** - English and Portuguese support
- ✅ **Validation** - FluentValidation for input validation
- 📖 **Swagger/OpenAPI** - Interactive API documentation

## 🛠️ Technologies

- **.NET 8.0** - LTS framework
- **ASP.NET Core** - RESTful API
- **Entity Framework Core** - ORM
- **JWT Bearer** - Authentication
- **Argon2** - Password hashing
- **MySQL/MariaDB** - Database
- **FluentValidation** - Validation
- **Swagger/OpenAPI** - API docs
- **xUnit** - Testing framework

## 🚀 Getting Started

**New to the project? Start here:**

- **[Installation Guide](docs/getting-started/INSTALLATION.md)** - Step-by-step installation instructions
- **[Quick Start Guide](docs/getting-started/QUICK_START.md)** - 5-minute setup from zero to running API
- **[Configuration Guide](docs/getting-started/CONFIGURATION.md)** - Complete configuration reference

## 📖 User Guides

**Comprehensive guides for using and developing with the service:**

- **[Development Guide](docs/guides/DEVELOPMENT.md)** - Development workflow and best practices
- **[Testing Guide](docs/guides/TESTING.md)** - Unit and integration testing strategies
- **[Deployment Guide](docs/guides/DEPLOYMENT.md)** - Production deployment strategies
- **[Contributing Guide](docs/guides/CONTRIBUTING.md)** - How to contribute to the project

## 🌐 API Documentation

**Everything about the API:**

- **[API Reference](docs/api/API.md)** - Complete API documentation
- **[Endpoints Reference](docs/api/ENDPOINTS.md)** - Detailed endpoint documentation with examples
- **[RBAC Guide](docs/api/RBAC.md)** - Role-Based Access Control implementation guide
- **[Integration Examples](docs/api/EXAMPLES.md)** - Real-world integration examples
- **[Audit Fields Documentation](docs/api/AUDIT_FIELDS.md)** - CreatedBy/UpdatedBy field behavior

## 📬 Postman Collection

**Quick Start with Postman**: A complete ready-to-use Postman collection is available with all endpoints organized by controller, including automatic token management and example payloads.

- **[Download Collection](docs/api/Authentication_API.postman_collection.json)** - Import directly into Postman
- **[Collection Guide](docs/api/POSTMAN_COLLECTION.md)** - Complete usage guide

**The collection includes:**
- ✅ All 40+ API endpoints organized by controller
- ✅ Automatic JWT token extraction and injection
- ✅ Pre-configured request bodies with examples
- ✅ Environment variable support for different deployments

**Quick setup:**
1. Download the [Postman collection](docs/api/Authentication_API.postman_collection.json)
2. Import into Postman (Import button → Select file)
3. Run **Authentication > Generate Token** first
4. Token is automatically saved and used in all subsequent requests

**Collection structure:**
- 🔐 **Authentication** - JWT token generation
- 👤 **Account** - User account management
- 🏷️ **Claim** - Permission claims management
- ⚡ **Action** - Available actions/permissions
- 🔗 **ClaimAction** - Claim-to-Action associations
- 👥 **AccountClaimAction** - User permissions
- 🏢 **Application** - Multi-tenant application management
- 🔐 **ApplicationClaim** - Application-to-Claim associations

**Need help?** Check the [full Postman guide](docs/api/POSTMAN_COLLECTION.md) for detailed instructions, examples, and troubleshooting.

## 🏗️ Architecture & Design

**Understanding the system architecture:**

- **[Domain Model and Use Cases](docs/modeling/DOMAIN_AND_USE_CASES.md)** - Complete domain model, entity relationships, and use case flows
- **[Architecture Guide](docs/architecture/ARCHITECTURE.md)** - Clean Architecture patterns and design decisions
- **[JWT Authentication](docs/architecture/JWT.md)** - Token-based authentication implementation
- **[Security Guide](docs/architecture/SECURITY.md)** - Argon2 hashing and security best practices
- **[Application Discrimination](docs/architecture/APPLICATION_DISCRIMINATION.md)** - Application identification and discrimination
- **[Swagger Configuration](docs/api/swagger-configuration.md)** - API documentation configuration

## 🔄 CI/CD

**Continuous Integration and Deployment:**

- **[Pipeline Documentation](docs/ci-cd/PIPELINE.md)** - CI/CD pipeline overview
- **[CI/CD Fixes](docs/ci-cd/FIXES.md)** - CI/CD issues and resolutions

## 📚 Reference

**Additional resources:**

- **[Troubleshooting Guide](docs/reference/TROUBLESHOOTING.md)** - Common issues and solutions
- **[FAQ](docs/reference/FAQ.md)** - Frequently Asked Questions
- **[Code Documentation Standards](docs/reference/CODE_DOCUMENTATION.md)** - XML comments and inline documentation guidelines
- **[Framework Downgrade Prevention](docs/reference/framework-downgrade-prevention.md)** - Preventing framework version downgrades
- **[Reorganization Notes](docs/reference/REORGANIZATION_NOTES.md)** - Documentation reorganization history

## 🧪 Testing

**Testing documentation:**

- **[Testing Guide](docs/guides/TESTING.md)** - Testing strategies and execution
- **[Detailed Test Documentation](docs/tests/DETAILED_TEST_DOCUMENTATION.md)** - Comprehensive test documentation

## 📊 Project Status

**Current project status and history:**

- **[Project Status](docs/status/PROJECT_STATUS.md)** - Current project status and build/test infrastructure
- **[Implementation Summary](docs/status/IMPLEMENTATION_SUMMARY.md)** - Overview of implemented features
- **[Testing Summary](docs/status/TESTING_SUMMARY.md)** - Testing infrastructure and coverage
- **[Test Execution Status](docs/status/TEST_EXECUTION_STATUS.md)** - Detailed test execution results
- **[Changelog](docs/status/CHANGELOG.md)** - Version history and changes

## 📋 Documentation by Audience

### For New Users

1. [Installation Guide](docs/getting-started/INSTALLATION.md)
2. [Quick Start Guide](docs/getting-started/QUICK_START.md)
3. [FAQ](docs/reference/FAQ.md)

### For Developers

1. [Quick Start Guide](docs/getting-started/QUICK_START.md)
2. [Development Guide](docs/guides/DEVELOPMENT.md)
3. [API Reference](docs/api/API.md)
4. [Architecture Guide](docs/architecture/ARCHITECTURE.md)
5. [Testing Guide](docs/guides/TESTING.md)

### For API Users

1. [API Reference](docs/api/API.md)
2. [Endpoints Reference](docs/api/ENDPOINTS.md)
3. [Postman Collection](docs/api/POSTMAN_COLLECTION.md)
4. [Integration Examples](docs/api/EXAMPLES.md)
5. [RBAC Guide](docs/api/RBAC.md)

### For System Administrators

1. [Installation Guide](docs/getting-started/INSTALLATION.md)
2. [Configuration Guide](docs/getting-started/CONFIGURATION.md)
3. [Deployment Guide](docs/guides/DEPLOYMENT.md)
4. [Security Guide](docs/architecture/SECURITY.md)
5. [Troubleshooting Guide](docs/reference/TROUBLESHOOTING.md)

## 🔍 Quick Links

### Most Common Tasks

- **Install the service**: [Installation Guide](docs/getting-started/INSTALLATION.md)
- **Configure database**: [Configuration Guide](docs/getting-started/CONFIGURATION.md#database-configuration)
- **Generate JWT token**: [Endpoints Reference](docs/api/ENDPOINTS.md#-generate-authentication-token)
- **Test with Postman**: [Postman Collection](docs/api/POSTMAN_COLLECTION.md)
- **Set up RBAC**: [RBAC Guide](docs/api/RBAC.md#step-by-step-setup-guide)
- **Run tests**: [Testing Guide](docs/guides/TESTING.md#-quick-test-execution)
- **Deploy to production**: [Deployment Guide](docs/guides/DEPLOYMENT.md)
- **Troubleshoot issues**: [Troubleshooting Guide](docs/reference/TROUBLESHOOTING.md)

### Key Features Documentation

- **JWT Authentication**: [JWT Guide](docs/architecture/JWT.md)
- **Password Security**: [Security Guide](docs/architecture/SECURITY.md#password-hashing)
- **Role-Based Access Control**: [RBAC Guide](docs/api/RBAC.md)
- **API Validation**: [Configuration Guide](docs/getting-started/CONFIGURATION.md#password-requirements)
- **Internationalization**: [Configuration Guide](docs/getting-started/CONFIGURATION.md#internationalization-i18n)

## 📁 Documentation Structure

```
docs/
├── getting-started/          # Installation and setup
│   ├── INSTALLATION.md       # Installation instructions
│   ├── QUICK_START.md        # Quick start guide
│   └── CONFIGURATION.md      # Configuration reference
│
├── guides/                   # User guides
│   ├── DEVELOPMENT.md        # Development guide
│   ├── TESTING.md            # Testing guide
│   ├── DEPLOYMENT.md         # Deployment guide
│   └── CONTRIBUTING.md       # Contributing guide
│
├── api/                      # API documentation
│   ├── API.md                # Complete API reference
│   ├── ENDPOINTS.md          # Endpoint details
│   ├── RBAC.md               # RBAC guide
│   ├── EXAMPLES.md           # Integration examples
│   ├── POSTMAN_COLLECTION.md # Postman collection guide
│   └── Authentication_API.postman_collection.json # Postman collection
│
├── architecture/             # Architecture documentation
│   ├── ARCHITECTURE.md       # Architecture overview
│   ├── JWT.md                # JWT implementation
│   ├── SECURITY.md           # Security guide
│   └── APPLICATION_DISCRIMINATION.md # Application discrimination
│
├── ci-cd/                    # CI/CD documentation
│   ├── PIPELINE.md           # Pipeline documentation
│   └── FIXES.md              # CI/CD fixes
│
├── reference/                # Reference documentation
│   ├── TROUBLESHOOTING.md    # Troubleshooting
│   ├── FAQ.md                # FAQ
│   ├── CODE_DOCUMENTATION.md # Code documentation standards
│   ├── framework-downgrade-prevention.md # Framework downgrade prevention
│   └── REORGANIZATION_NOTES.md # Documentation reorganization notes
│
├── status/                   # Project status
│   ├── PROJECT_STATUS.md     # Current status
│   ├── IMPLEMENTATION_SUMMARY.md # Implementation overview
│   ├── TESTING_SUMMARY.md    # Testing overview
│   ├── TEST_EXECUTION_STATUS.md # Test results
│   └── CHANGELOG.md          # Version history
│
└── tests/                    # Testing documentation
    └── DETAILED_TEST_DOCUMENTATION.md # Detailed test documentation
```

## 💡 Tips for Using This Documentation

1. **Use the search**: Press `Ctrl+F` / `Cmd+F` to search within pages
2. **Follow the links**: Documentation is heavily cross-referenced
3. **Check the FAQ**: Many common questions are answered in the [FAQ](docs/reference/FAQ.md)
4. **Start with Quick Start**: The [Quick Start Guide](docs/getting-started/QUICK_START.md) gets you up and running quickly
5. **Use Postman**: Download the [Postman collection](docs/api/Authentication_API.postman_collection.json) for easy API testing
6. **Reference when needed**: Use the [API Reference](docs/api/API.md) and [Troubleshooting](docs/reference/TROUBLESHOOTING.md) as needed

## 🤝 Contributing

Contributions are welcome! Please see our guidelines:

- **[Contributing Guide](CONTRIBUTING.md)** - Quick contributing overview
- **[Detailed Contributing Guide](docs/guides/CONTRIBUTING.md)** - Complete contributing guidelines
- **[Code of Conduct](CODE_OF_CONDUCT.md)** - Community standards and expectations

Found an issue or want to improve the documentation?
- Report documentation issues on [GitHub Issues](https://github.com/maiconcardozo/Authentication/issues)
- Submit documentation improvements via Pull Request
- Check our documentation style guidelines in the [Contributing Guide](docs/guides/CONTRIBUTING.md)

## 📄 License

This project is licensed under the [MIT License](LICENSE).

## 👨‍💻 Author

**Maicon Cardozo**
- GitHub: [@maiconcardozo](https://github.com/maiconcardozo)

## 📞 Support

Need help? We have several resources:

- **[Support Guide](SUPPORT.md)** - How to get help
- **[Security Policy](SECURITY.md)** - Report security vulnerabilities
- 📖 Check the [FAQ](docs/reference/FAQ.md)
- 🔧 Review the [Troubleshooting Guide](docs/reference/TROUBLESHOOTING.md)
- 🐛 Report issues: [GitHub Issues](https://github.com/maiconcardozo/Authentication/issues)
- 💬 Ask questions: [GitHub Discussions](https://github.com/maiconcardozo/Authentication/discussions)

## 📖 External Resources

- [.NET 8.0 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [JWT.io](https://jwt.io/) - JWT documentation and debugger
- [OWASP Security Guidelines](https://owasp.org/)
- [Postman Documentation](https://learning.postman.com/docs/getting-started/introduction/)

---

⭐ If this project was useful to you, please consider giving it a star!
