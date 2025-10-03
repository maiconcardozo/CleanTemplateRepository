#!/bin/bash

# Test Infrastructure Validation Script
# Validates that all test components are properly configured
# Can run without .NET 9.0 to verify setup

set -e

echo "🔍 Authentication Project - Test Infrastructure Validation"
echo "=========================================================="

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Counters
CHECKS_PASSED=0
CHECKS_FAILED=0
WARNINGS=0

# Function to print status
print_status() {
    local status=$1
    local message=$2
    
    case $status in
        "PASS")
            echo -e "${GREEN}✅ PASS${NC}: $message"
            ((CHECKS_PASSED++))
            ;;
        "FAIL")
            echo -e "${RED}❌ FAIL${NC}: $message"
            ((CHECKS_FAILED++))
            ;;
        "WARN")
            echo -e "${YELLOW}⚠️  WARN${NC}: $message"
            ((WARNINGS++))
            ;;
        "INFO")
            echo -e "${BLUE}ℹ️  INFO${NC}: $message"
            ;;
    esac
}

# Navigate to project root
cd "$(dirname "$0")/.."

echo "📂 Project Root: $(pwd)"
echo ""

# 1. Check Project Structure
echo "🏗️ Checking Project Structure..."
echo "================================"

if [ -f "Solution/Authentication.sln" ]; then
    print_status "PASS" "Solution file exists (Solution/Authentication.sln)"
else
    print_status "FAIL" "Solution file missing (Solution/Authentication.sln)"
fi

if [ -f "Src/Authentication.Tests/Authentication.Tests.csproj" ]; then
    print_status "PASS" "Test project exists (Src/Authentication.Tests/Authentication.Tests.csproj)"
else
    print_status "FAIL" "Test project missing (Src/Authentication.Tests/Authentication.Tests.csproj)"
fi

if [ -d "Src/Authentication.Tests/Integration" ]; then
    print_status "PASS" "Integration tests directory exists"
    INTEGRATION_TESTS=$(find Src/Authentication.Tests/Integration -name "*.cs" | wc -l)
    print_status "INFO" "Found $INTEGRATION_TESTS integration test files"
else
    print_status "FAIL" "Integration tests directory missing"
fi

if [ -d "Src/Authentication.Tests/Unit" ]; then
    print_status "PASS" "Unit tests directory exists"
    UNIT_TESTS=$(find Src/Authentication.Tests/Unit -name "*.cs" | wc -l)
    print_status "INFO" "Found $UNIT_TESTS unit test files"
else
    print_status "FAIL" "Unit tests directory missing"
fi

echo ""

# 2. Check Test Scripts
echo "📜 Checking Test Scripts..."
echo "=========================="

if [ -f "scripts/test.sh" ] && [ -x "scripts/test.sh" ]; then
    print_status "PASS" "Linux test script exists and is executable"
else
    print_status "FAIL" "Linux test script missing or not executable"
fi

if [ -f "scripts/test.bat" ]; then
    print_status "PASS" "Windows test script exists"
else
    print_status "FAIL" "Windows test script missing"
fi

if [ -f "scripts/run-tests.sh" ] && [ -x "scripts/run-tests.sh" ]; then
    print_status "PASS" "Advanced test runner exists and is executable"
else
    print_status "FAIL" "Advanced test runner missing or not executable"
fi

if [ -f "scripts/build.sh" ] && [ -x "scripts/build.sh" ]; then
    print_status "PASS" "Build script exists and is executable"
else
    print_status "FAIL" "Build script missing or not executable"
fi

echo ""

# 3. Check CI/CD Configuration
echo "🚀 Checking CI/CD Configuration..."
echo "=================================="

if [ -f ".github/workflows/ci.yml" ]; then
    print_status "PASS" "GitHub Actions workflow exists"
    
    # Check for .NET 8.0 configuration
    if grep -q "DOTNET_VERSION.*8.0" .github/workflows/ci.yml; then
        print_status "PASS" "CI/CD configured for .NET 8.0"
    else
        print_status "WARN" "CI/CD may not be configured for .NET 8.0"
    fi
    
    # Check for test execution
    if grep -q "dotnet test" .github/workflows/ci.yml; then
        print_status "PASS" "CI/CD includes test execution"
    else
        print_status "FAIL" "CI/CD missing test execution"
    fi
    
    # Check for artifact upload
    if grep -q "actions/upload-artifact" .github/workflows/ci.yml; then
        print_status "PASS" "CI/CD includes artifact upload"
    else
        print_status "WARN" "CI/CD missing artifact upload"
    fi
else
    print_status "FAIL" "GitHub Actions workflow missing"
fi

echo ""

# 4. Check Documentation
echo "📚 Checking Documentation..."
echo "============================"

if [ -f "docs/TESTING.md" ]; then
    print_status "PASS" "Testing documentation exists"
    
    # Check for key sections
    if grep -q "Quick Test Execution" docs/TESTING.md; then
        print_status "PASS" "Documentation includes quick execution guide"
    else
        print_status "WARN" "Documentation missing quick execution guide"
    fi
    
    if grep -q "Troubleshooting" docs/TESTING.md; then
        print_status "PASS" "Documentation includes troubleshooting"
    else
        print_status "WARN" "Documentation missing troubleshooting section"
    fi
else
    print_status "FAIL" "Testing documentation missing"
fi

if [ -f "docs/TEST_EXECUTION_STATUS.md" ]; then
    print_status "PASS" "Test execution status document exists"
else
    print_status "WARN" "Test execution status document missing"
fi

if grep -q "Running Tests" README.md; then
    print_status "PASS" "README includes testing information"
else
    print_status "WARN" "README missing testing information"
fi

echo ""

# 5. Check .NET Configuration
echo "⚙️ Checking .NET Configuration..."
echo "================================="

if [ -f "global.json" ]; then
    print_status "PASS" "global.json exists"
    
    if grep -q '"version": "8.0' global.json; then
        print_status "PASS" "global.json configured for .NET 8.0"
    else
        print_status "WARN" "global.json may not be configured for .NET 8.0"
    fi
else
    print_status "WARN" "global.json missing"
fi

# Check .NET version if available
DOTNET_VERSION=$(dotnet --version 2>/dev/null || echo "not found")
if [ "$DOTNET_VERSION" = "not found" ]; then
    print_status "WARN" ".NET SDK not found (this is expected in some environments)"
elif [[ "$DOTNET_VERSION" =~ ^8\. ]]; then
    print_status "PASS" ".NET 8.0 SDK available ($DOTNET_VERSION)"
else
    print_status "WARN" ".NET version mismatch (found: $DOTNET_VERSION, expected: 8.0.x)"
fi

echo ""

# 6. Check Test Dependencies
echo "📦 Checking Test Dependencies..."
echo "==============================="

if [ -f "Src/Authentication.Tests/Authentication.Tests.csproj" ]; then
    # Check for xUnit
    if grep -q "xunit" Src/Authentication.Tests/Authentication.Tests.csproj; then
        print_status "PASS" "xUnit framework configured"
    else
        print_status "FAIL" "xUnit framework missing"
    fi
    
    # Check for FluentAssertions
    if grep -q "FluentAssertions" Src/Authentication.Tests/Authentication.Tests.csproj; then
        print_status "PASS" "FluentAssertions configured"
    else
        print_status "WARN" "FluentAssertions missing"
    fi
    
    # Check for ASP.NET Core Testing
    if grep -q "Microsoft.AspNetCore.Mvc.Testing" Src/Authentication.Tests/Authentication.Tests.csproj; then
        print_status "PASS" "ASP.NET Core testing framework configured"
    else
        print_status "WARN" "ASP.NET Core testing framework missing"
    fi
    
    # Check for EF Core InMemory
    if grep -q "Microsoft.EntityFrameworkCore.InMemory" Src/Authentication.Tests/Authentication.Tests.csproj; then
        print_status "PASS" "EF Core InMemory database configured"
    else
        print_status "WARN" "EF Core InMemory database missing"
    fi
fi

echo ""

# 7. Validate Test Files Content
echo "🧪 Validating Test Files Content..."
echo "==================================="

INTEGRATION_TEST_FILES=(
    "Src/Authentication.Tests/Integration/AuthenticationControllerTests.cs"
    "Src/Authentication.Tests/Integration/AccountControllerTests.cs"
    "Src/Authentication.Tests/Integration/ClaimControllerTests.cs"
    "Src/Authentication.Tests/Integration/ActionControllerTests.cs"
)

for test_file in "${INTEGRATION_TEST_FILES[@]}"; do
    if [ -f "$test_file" ]; then
        print_status "PASS" "$(basename "$test_file") exists"
        
        # Check for [Fact] attributes
        FACT_COUNT=$(grep -c "\[Fact\]" "$test_file" 2>/dev/null || echo "0")
        print_status "INFO" "$(basename "$test_file") contains $FACT_COUNT test methods"
    else
        print_status "WARN" "$(basename "$test_file") missing"
    fi
done

UNIT_TEST_FILES=(
    "Src/Authentication.Tests/Unit/AccountEntityTests.cs"
    "Src/Authentication.Tests/Unit/AccountServiceTests.cs"
    "Src/Authentication.Tests/Unit/AccountRepositoryTests.cs"
)

for test_file in "${UNIT_TEST_FILES[@]}"; do
    if [ -f "$test_file" ]; then
        print_status "PASS" "$(basename "$test_file") exists"
        
        # Check for [Fact] attributes
        FACT_COUNT=$(grep -c "\[Fact\]" "$test_file" 2>/dev/null || echo "0")
        print_status "INFO" "$(basename "$test_file") contains $FACT_COUNT test methods"
    else
        print_status "WARN" "$(basename "$test_file") missing"
    fi
done

echo ""

# 8. Summary
echo "📊 Validation Summary"
echo "===================="
echo ""

TOTAL_CHECKS=$((CHECKS_PASSED + CHECKS_FAILED))

echo "📈 Results:"
echo "  ✅ Passed: $CHECKS_PASSED"
echo "  ❌ Failed: $CHECKS_FAILED"
echo "  ⚠️  Warnings: $WARNINGS"
echo "  📊 Total: $TOTAL_CHECKS"
echo ""

# Calculate percentage
if [ $TOTAL_CHECKS -gt 0 ]; then
    PASS_PERCENTAGE=$((CHECKS_PASSED * 100 / TOTAL_CHECKS))
    echo "🎯 Success Rate: ${PASS_PERCENTAGE}%"
else
    echo "🎯 Success Rate: N/A"
fi

echo ""

# Final status
if [ $CHECKS_FAILED -eq 0 ]; then
    echo -e "${GREEN}🎉 Test infrastructure validation PASSED!${NC}"
    echo ""
    echo "✅ The test infrastructure is properly configured and ready for execution."
    echo "📋 To run tests, ensure .NET 9.0 SDK is installed and use:"
    echo "   • ./scripts/test.sh (Linux/Mac)"
    echo "   • scripts/test.bat (Windows)"
    echo "   • dotnet test Solution/Authentication.sln"
    exit 0
else
    echo -e "${RED}❌ Test infrastructure validation FAILED!${NC}"
    echo ""
    echo "🔧 Please fix the failed checks before running tests."
    echo "📚 See docs/TESTING.md for detailed setup instructions."
    exit 1
fi