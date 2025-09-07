# CleanTemplateRepository - .NET Project Template

[![CI/CD Pipeline](https://github.com/maiconcardozo/CleanTemplateRepository/actions/workflows/ci.yml/badge.svg)](https://github.com/maiconcardozo/CleanTemplateRepository/actions/workflows/ci.yml)
[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![Template](https://img.shields.io/badge/Template-Clean%20Architecture-green.svg)](https://github.com/maiconcardozo/CleanTemplateRepository)

## 📋 Overview

**CleanTemplateRepository** is a standardized .NET project template designed to accelerate development of new applications following Clean Architecture and Domain-Driven Design (DDD) principles. This template provides a complete foundation with essential configuration files, documentation structure, and development tools.

### 🚀 Template Features

- **Clean Architecture Structure**: Organized layers following DDD principles
- **Essential Configuration**: Pre-configured .editorconfig, .gitignore, code quality rules
- **Build & Test Scripts**: Cross-platform scripts for development workflow
- **Documentation Framework**: Complete documentation structure with examples
- **CI/CD Ready**: GitHub Actions workflow templates
- **Code Quality Tools**: Integrated linting, analysis, and formatting rules
- **Internationalization Support**: Ready for multi-language applications

## 🏗️ Template Structure

The template is organized in well-defined directories following industry best practices:

```
CleanTemplateRepository/
├── Src/                             # Source code directory
│   └── [Your Project Files Here]   # Your application projects go here
│
├── Solution/                        # Solution configuration
│   └── [Your Solution Files Here]  # Your .sln files and solution configs
│
├── docs/                            # Documentation
│   ├── status/                      # Project status reports
│   ├── ARCHITECTURE.md              # Architecture documentation
│   ├── CODE_DOCUMENTATION.md       # Code documentation standards
│   └── DETAILED_TEST_DOCUMENTATION.md # Testing documentation
│
├── scripts/                         # Build & test scripts
│   ├── build.sh / build.bat         # Cross-platform build scripts
│   ├── run-tests.sh / run-tests.bat # Test execution scripts
│   └── README.md                    # Scripts documentation
│
├── .github/                         # GitHub configuration
│   └── workflows/                   # CI/CD workflows
│
├── .editorconfig                    # Editor configuration
├── .gitignore                       # Git ignore rules
├── code-quality.ruleset             # Code quality rules
├── Directory.Build.props            # MSBuild properties
├── global.json                      # .NET SDK version
├── CONTRIBUTING.md                  # Contribution guidelines
├── CHANGELOG.md                     # Version history
└── LICENSE                          # License information
```

### 📁 Repository Organization

The repository follows a clean, organized structure:

- **`/Src/`** - Source code (your projects will go here)
- **`/Solution/`** - Solution files and configuration
- **`/docs/`** - All documentation including architecture and coding standards
- **`/scripts/`** - Build, test, and utility scripts
- **`/.github/`** - GitHub workflows and templates
- **Root level** - Essential configuration files only

## 🚀 Getting Started

### 📋 Prerequisites

- **.NET 9.0 SDK** or later
- **Git** for version control
- **Your preferred IDE** (Visual Studio, VS Code, JetBrains Rider)

### 🔧 Quick Setup

1. **Use this template** to create a new repository
2. **Clone your new repository**:
   ```bash
   git clone https://github.com/yourusername/your-new-project.git
   cd your-new-project
   ```

3. **Install .NET 9.0** if needed:
   ```bash
   chmod +x dotnet-install.sh
   ./dotnet-install.sh --version 9.0.101
   ```

4. **Create your first project**:
   ```bash
   mkdir -p Src/YourProject.API
   cd Src/YourProject.API
   dotnet new webapi
   ```

5. **Create a solution**:
   ```bash
   mkdir -p Solution
   cd Solution
   dotnet new sln --name YourProject
   dotnet sln add ../Src/YourProject.API/YourProject.API.csproj
   ```

## 🛠️ Template Configuration

This template includes pre-configured tools and settings:

### 📝 Configuration Files

- **`.editorconfig`** - Consistent code formatting across IDEs
- **`.gitignore`** - Optimized for .NET projects with common exclusions
- **`code-quality.ruleset`** - Code quality and style enforcement rules
- **`Directory.Build.props`** - Shared MSBuild properties and package versions
- **`global.json`** - .NET SDK version pinning for consistent builds

### 🔧 Available Scripts

The template includes ready-to-use scripts for common development tasks:

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
scripts/run-tests.sh all         # Run all tests
scripts/run-tests.sh unit        # Run unit tests only
scripts/run-tests.sh integration # Run integration tests only
scripts/run-tests.sh coverage    # Run with code coverage
scripts/run-tests.sh verbose     # Run with detailed output
scripts/run-tests.sh watch       # Run in watch mode
scripts/run-tests.sh clean       # Clean, rebuild, then test
```

*Note: Windows users should use `.bat` extensions instead of `.sh`*

## 📚 Documentation

The template includes comprehensive documentation:

- **`docs/ARCHITECTURE.md`** - Architecture patterns and guidelines
- **`docs/CODE_DOCUMENTATION.md`** - Code documentation standards
- **`docs/DETAILED_TEST_DOCUMENTATION.md`** - Testing patterns and examples
- **`CONTRIBUTING.md`** - Contribution guidelines
- **`CHANGELOG.md`** - Version history template

## ⚙️ Template Customization

### 1. Update Project Names
- Replace placeholder names in scripts with your project name
- Update solution and project file names
- Modify README.md title and description

### 2. Configure CI/CD
- Update GitHub Actions workflows in `.github/workflows/`
- Adjust badge URLs in README.md
- Configure deployment targets

### 3. Customize Code Quality Rules
- Modify `code-quality.ruleset` for your team standards
- Update `Directory.Build.props` package versions
- Adjust `.editorconfig` formatting preferences

## 💻 Recommended IDEs
- **Visual Studio 2022** (17.14+) with .NET workload
- **Visual Studio Code** with C# Dev Kit extension
- **JetBrains Rider** 2024.1+

## ⚠️ .NET 9.0 Framework Requirements

**IMPORTANT**: This template is configured for .NET 9.0 for optimal performance and latest features.

### Installation:
1. **Download .NET 9.0 SDK**: https://dotnet.microsoft.com/download/dotnet/9.0
2. **Use included installer**: `./dotnet-install.sh --version 9.0.101`
3. **Verify Installation**: `dotnet --version` should show 9.0.x

## 🤝 Contributing

Contributions are welcome! Please read the [contribution guide](CONTRIBUTING.md) before submitting pull requests.

## 📄 License

This project is licensed under the [MIT License](LICENSE).

## 👨‍💻 Author

**Maicon Cardozo**
- GitHub: [@maiconcardozo](https://github.com/maiconcardozo)

## 📞 Support

For questions, suggestions, or to report issues:
- Open an [issue](https://github.com/maiconcardozo/CleanTemplateRepository/issues)
- Contact through GitHub

---

⭐ If this template was useful to you, please consider giving it a star!