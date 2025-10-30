# 🔍 Code Quality Test Script

This script validates that the code quality rules are working correctly.

## Test Cases:
1. Creates a temporary file with else statement violations
2. Attempts to build and expects failure
3. Removes the test file and confirms successful build

echo "🧪 Testing Code Quality Rules..."

# Create test file with else statement violations
cat > /tmp/CodeQualityTestTemp.cs << 'EOF'
using System;

namespace TestNamespace
{
    public class TestClass
    {
        // This should trigger IDE0046 error
        public string TestElseReturn(bool condition)
        {
            if (condition)
                return "true";
            else
                return "false";
        }

        // This should trigger IDE0045 error  
        public void TestElseAssignment(bool condition)
        {
            string result;
            if (condition)
                result = "true";
            else
                result = "false";
            
            Console.WriteLine(result);
        }
    }
}
EOF

echo "📄 Created test file with else statements..."

# Copy test file to project
cp /tmp/CodeQualityTestTemp.cs Src/Authentication.Tests/

# Test build should fail
echo "🔥 Testing build failure with else statements..."
if dotnet build Src/Authentication.Tests/Authentication.Tests.csproj --configuration Release /warnaserror:IDE0046,IDE0045 > /tmp/build_output.txt 2>&1; then
    echo "❌ FAIL: Build should have failed but succeeded"
    exit 1
else
    echo "✅ PASS: Build correctly failed due to else statement violations"
    grep -i "IDE0046\|IDE0045" /tmp/build_output.txt || echo "Warning: Error codes not found in output"
fi

# Remove test file
rm Src/Authentication.Tests/CodeQualityTestTemp.cs

# Test build should succeed  
echo "✅ Testing build success without else statements..."
if dotnet build Src/Authentication.Tests/Authentication.Tests.csproj --configuration Release /warnaserror:IDE0046,IDE0045 > /tmp/build_output_clean.txt 2>&1; then
    echo "✅ PASS: Build succeeded after removing else statements"
else
    echo "❌ FAIL: Build should have succeeded but failed"
    echo "Build output:"
    cat /tmp/build_output_clean.txt
    exit 1
fi

echo "🎉 All code quality tests passed!"
echo ""
echo "📋 Summary:"
echo "- ✅ Else statement detection working (IDE0046, IDE0045)"
echo "- ✅ Build fails when else statements are present"
echo "- ✅ Build succeeds when code follows quality standards"
echo ""
echo "🚫 The following patterns are now BLOCKED:"
echo "   • if/else return statements (use ternary operator)"
echo "   • if/else assignments (use ternary operator)"
echo ""
echo "✅ Use instead:"
echo "   • return condition ? \"true\" : \"false\";"
echo "   • result = condition ? \"true\" : \"false\";"