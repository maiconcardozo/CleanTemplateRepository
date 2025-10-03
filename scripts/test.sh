#!/bin/bash

# Single command test runner for CI/CD and development
# This script provides a simple interface to run all tests

set -e

echo "🧪 Authentication Project - Test Runner"
echo "========================================"

# Set up .NET path if needed
if [[ ":$PATH:" != *":/home/runner/.dotnet:"* ]]; then
    export PATH="/home/runner/.dotnet:$PATH"
fi

# Function to check .NET version compatibility
check_dotnet_version() {
    local required_version="8.0"
    local current_version=$(dotnet --version 2>/dev/null || echo "not found")
    
    echo "🔧 .NET Version: $current_version"
    
    if [[ "$current_version" == "not found" ]]; then
        echo "❌ .NET SDK not found. Please install .NET 8.0 SDK."
        echo "📥 Download from: https://dotnet.microsoft.com/download/dotnet/8.0"
        exit 1
    fi
    
    if [[ ! "$current_version" =~ ^8\. ]]; then
        echo "⚠️  Warning: This project requires .NET 8.0, but found $current_version"
        echo "📥 Download .NET 8.0 from: https://dotnet.microsoft.com/download/dotnet/8.0"
        echo "🔄 Attempting to continue with current version..."
    else
        echo "✅ .NET version compatible"
    fi
}

# Navigate to project root
cd "$(dirname "$0")/.."

# Check .NET version
check_dotnet_version

# Check if solution exists
if [ ! -f "Solution/CleanTemplate.sln" ]; then
    echo "❌ Solution file not found at Solution/CleanTemplate.sln"
    echo "📂 Current directory: $(pwd)"
    echo "📁 Available files:"
    ls -la | head -10
    exit 1
fi

echo "📦 Restoring packages..."
if dotnet restore Solution/CleanTemplate.sln; then
    echo "✅ Package restore successful"
else
    echo "❌ Package restore failed"
    exit 1
fi

echo "🏗️ Building solution..."
if dotnet build Solution/CleanTemplate.sln --configuration Release --no-restore; then
    echo "✅ Build successful"
else
    echo "❌ Build failed"
    exit 1
fi

echo "🧪 Running all tests..."
echo "📊 Test execution details:"
echo "  • Configuration: Release"
echo "  • Verbosity: Normal"  
echo "  • Results: Console output + TRX files"
echo ""

# Create TestResults directory if it doesn't exist
mkdir -p TestResults

if dotnet test Solution/CleanTemplate.sln \
    --configuration Release \
    --no-build \
    --verbosity normal \
    --logger trx \
    --results-directory TestResults; then
    
    echo ""
    echo "✅ Test execution completed successfully!"
    
    # Show test result summary
    if [ -d "TestResults" ]; then
        echo "📊 Test Results Summary:"
        TRX_COUNT=$(find TestResults -name "*.trx" | wc -l)
        echo "  • TRX files generated: $TRX_COUNT"
        echo "  • Results location: TestResults/"
        
        # List TRX files
        if [ $TRX_COUNT -gt 0 ]; then
            echo "  • Files:"
            find TestResults -name "*.trx" | while read file; do
                echo "    - $(basename "$file")"
            done
        fi
    fi
else
    echo ""
    echo "❌ Some tests failed or test execution encountered errors"
    echo "📁 Check TestResults/ directory for detailed results"
    exit 1
fi

echo ""
echo "🎉 Test runner completed successfully!"
echo "📋 Next steps:"
echo "  • Review test results in TestResults/ directory"
echo "  • Check build output for any warnings"
echo "  • Run 'scripts/run-tests.sh coverage' for coverage analysis"