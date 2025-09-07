#!/bin/bash
# Swagger API Separation Verification Script

echo "🔍 Verifying Swagger API Documentation Separation"
echo "=================================================="

# Check if the application builds successfully
echo "📦 Building application..."
cd "$(dirname "$0")/../Src/Authentication.API"
BUILD_RESULT=$(dotnet build --verbosity minimal 2>&1)
BUILD_EXIT_CODE=$?

if [ $BUILD_EXIT_CODE -ne 0 ]; then
    echo "❌ Build failed:"
    echo "$BUILD_RESULT"
    exit 1
fi

echo "✅ Build successful"

# Verify Program.cs contains the correct API definitions
echo "🔍 Checking Program.cs API definitions..."

# Check Authentication API definition
if grep -q "ApplicationConstants.Api.SwaggerDefinitions.Authentication" Program.cs; then
    echo "✅ Authentication API definition found"
else
    echo "❌ Authentication API definition missing"
    exit 1
fi

# Check AccessControl API definition
if grep -q "ApplicationConstants.Api.SwaggerDefinitions.AccessControl" Program.cs; then
    echo "✅ AccessControl API definition found"
else
    echo "❌ AccessControl API definition missing"
    exit 1
fi

# Check DocInclusionPredicate logic
echo "🔍 Verifying controller mappings..."

# Check Authentication controller mapping
if grep -q 'controllerName?.Equals("Authentication", StringComparison.OrdinalIgnoreCase)' Program.cs; then
    echo "✅ Authentication controller mapping correct"
else
    echo "❌ Authentication controller mapping incorrect"
    exit 1
fi

# Check AccessControl controllers mapping
EXPECTED_CONTROLLERS=("Account" "AccountClaimAction" "Action" "ClaimAction" "Claim")

for controller in "${EXPECTED_CONTROLLERS[@]}"; do
    if grep -q "controllerName?.Equals(\"$controller\", StringComparison.OrdinalIgnoreCase)" Program.cs; then
        echo "✅ $controller controller mapping found"
    else
        echo "❌ $controller controller mapping missing"
        exit 1
    fi
done

# Verify Swagger route files exist for all controllers
echo "🔍 Checking Swagger route files..."

ROUTE_FILES=("AuthenticationRoutes.cs" "AccountRoutes.cs" "AccountClaimActionRoutes.cs" "ActionRoutes.cs" "ClaimActionRoutes.cs" "ClaimRoutes.cs")

for route_file in "${ROUTE_FILES[@]}"; do
    if [ -f "Swagger/$route_file" ]; then
        echo "✅ $route_file exists"
    else
        echo "❌ $route_file missing"
        exit 1
    fi
done

# Check ApplicationConstants contains SwaggerDefinitions
echo "🔍 Checking ApplicationConstants SwaggerDefinitions..."

cd ../Authentication.Login/Constants
if grep -q "SwaggerDefinitions" ApplicationConstants.cs; then
    echo "✅ SwaggerDefinitions class found"
else
    echo "❌ SwaggerDefinitions class missing"
    exit 1
fi

# Check specific constants
EXPECTED_CONSTANTS=("Authentication" "AccessControl" "AuthenticationEndpoint" "AccessControlEndpoint" "AuthenticationDisplayName" "AccessControlDisplayName")

for constant in "${EXPECTED_CONSTANTS[@]}"; do
    if grep -q "public const string $constant" ApplicationConstants.cs; then
        echo "✅ $constant constant found"
    else
        echo "❌ $constant constant missing"
        exit 1
    fi
done

cd ../../..

echo ""
echo "🎉 All verification checks passed!"
echo "✅ API documentation is properly separated into:"
echo "   - Authentication API (AuthenticationController)"
echo "   - AccessControl API (Account, AccountClaimAction, Action, ClaimAction, Claim controllers)"
echo ""
echo "📚 Documentation files updated:"
echo "   - docs/swagger-configuration.md"
echo "   - docs/API.md"
echo ""
echo "🚀 Ready for deployment!"