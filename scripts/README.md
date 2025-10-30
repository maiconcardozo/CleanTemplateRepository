# 🛠️ Scripts Directory

This directory contains all build and testing scripts for the Authentication project.

## 📋 Available Scripts

### 🧪 **Test Execution Scripts**

#### `test.sh` / `test.bat` - **Universal Test Runner** ⭐ **RECOMMENDED**
**Purpose**: Single command to build and run all tests
```bash
./scripts/test.sh              # Linux/Mac
scripts/test.bat               # Windows
```
**Features**: .NET version checking, automatic dependency restoration, enhanced error handling

#### `run-tests.sh` / `run-tests.bat` - **Advanced Test Runner**
**Purpose**: Feature-rich test execution with multiple options
```bash
scripts/run-tests.sh all         # Run all tests (default)
scripts/run-tests.sh unit        # Unit tests only
scripts/run-tests.sh integration # Integration tests only
scripts/run-tests.sh coverage    # With code coverage
scripts/run-tests.sh verbose     # Detailed output
scripts/run-tests.sh watch       # Continuous testing
scripts/run-tests.sh clean       # Clean + rebuild + test
scripts/run-tests.sh help        # Show all options
```

### 🏗️ **Build Scripts**

#### `build.sh` / `build.bat` - **Build Automation**
```bash
scripts/build.sh debug          # Debug build (default)
scripts/build.sh release        # Release build
scripts/build.sh clean          # Clean and rebuild
scripts/build.sh restore        # Restore dependencies only
scripts/build.sh verify         # Complete verification (build + tests)
scripts/build.sh help           # Show all options
```

### 🔍 **Validation Scripts**

#### `validate-test-infrastructure.sh` - **Test Infrastructure Validator** ⭐ **NEW**
**Purpose**: Comprehensive validation of test infrastructure without requiring .NET 8.0
```bash
./scripts/validate-test-infrastructure.sh
```
**Features**: Project structure validation, CI/CD checks, documentation completeness, colored output

### 🔧 **Quality & Verification Scripts**

#### `test-code-quality.sh` - **Code Quality Validator**
```bash
./scripts/test-code-quality.sh
```
**Purpose**: Validates code quality rules and SOLID principles

#### `verify_swagger_separation.sh` - **API Documentation Validator**  
```bash
./scripts/verify_swagger_separation.sh
```
**Purpose**: Verifies Swagger/OpenAPI documentation separation

#### `validate-badges.sh` - **README Badge Validator** ⭐ **NEW**
```bash
./scripts/validate-badges.sh
```
**Purpose**: Validates and suggests updates for README badges including test count, lines of code, and badge status verification

## 🎯 **Recommended Usage Patterns**

### For Development (Most Common)
```bash
# Quick test run during development
./scripts/test.sh

# Continuous testing while coding
scripts/run-tests.sh watch

# Full verification before commit
scripts/build.sh verify
```

### For CI/CD
```bash
# Infrastructure validation (works without .NET 8.0)
./scripts/validate-test-infrastructure.sh

# Complete build and test
./scripts/test.sh
```

### For Troubleshooting
```bash
# Validate infrastructure first
./scripts/validate-test-infrastructure.sh

# Clean build if issues found
scripts/build.sh clean

# Detailed test run for debugging
scripts/run-tests.sh verbose
```

## 📋 Requirements

- **.NET 8.0 SDK** (for execution scripts)
- **MySQL 8.0+** (for integration tests)
- **Bash shell** (Linux/Mac/WSL for validation scripts)
- All scripts should be run from the repository root

## 🔧 Cross-Platform Support

All execution scripts have both Linux/Mac (`.sh`) and Windows (`.bat`) versions for cross-platform compatibility.

## 📚 Related Documentation

- **[TESTING.md](../docs/TESTING.md)**: Complete testing guide
- **[TEST_EXECUTION_STATUS.md](../docs/TEST_EXECUTION_STATUS.md)**: Infrastructure status
- **[DEVELOPMENT.md](../docs/DEVELOPMENT.md)**: Development workflow