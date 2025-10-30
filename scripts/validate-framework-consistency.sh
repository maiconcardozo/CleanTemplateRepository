#!/bin/bash

# Framework Consistency Validation Script
# Prevents accidental framework downgrades by validating consistency
# between global.json, project files, and documentation

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" &> /dev/null && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." &> /dev/null && pwd)"

echo "🔍 Validating .NET Framework Consistency..."
echo "Repository: $REPO_ROOT"
echo ""

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Initialize validation results
VALIDATION_ERRORS=0

# Function to extract .NET version from global.json
get_global_json_version() {
    if [ -f "$REPO_ROOT/build/global.json" ]; then
        grep -o '"version": "[^"]*"' "$REPO_ROOT/build/global.json" | cut -d'"' -f4 | cut -d'.' -f1,2
    else
        echo "NOT_FOUND"
    fi
}

# Function to extract target framework from project files
get_project_frameworks() {
    find "$REPO_ROOT" -name "*.csproj" -exec grep -H "TargetFramework" {} \; | sed 's/.*<TargetFramework>\(.*\)<\/TargetFramework>.*/\1/' | sort -u
}

# Function to check documentation consistency
check_documentation_versions() {
    local doc_files=("$REPO_ROOT/docs/getting-started/QUICK_START.md" "$REPO_ROOT/README.md")
    local expected_version="$1"
    
    for doc_file in "${doc_files[@]}"; do
        if [ -f "$doc_file" ]; then
            # Check for .NET version references
            local found_versions=$(grep -o "\.NET [0-9]\+\.[0-9]\+" "$doc_file" 2>/dev/null | sort -u)
            
            if [ -n "$found_versions" ]; then
                echo "📄 Found .NET versions in $(basename "$doc_file"): $found_versions"
                
                # Check if any version doesn't match expected
                while IFS= read -r version; do
                    local version_number=$(echo "$version" | grep -o "[0-9]\+\.[0-9]\+")
                    if [ "$version_number" != "$expected_version" ]; then
                        echo -e "${RED}❌ Version mismatch in $(basename "$doc_file"): found $version, expected .NET $expected_version${NC}"
                        VALIDATION_ERRORS=$((VALIDATION_ERRORS + 1))
                    fi
                done <<< "$found_versions"
            fi
        fi
    done
}

# Main validation logic
echo "1️⃣ Checking global.json..."
GLOBAL_VERSION=$(get_global_json_version)
if [ "$GLOBAL_VERSION" = "NOT_FOUND" ]; then
    echo -e "${RED}❌ global.json not found${NC}"
    VALIDATION_ERRORS=$((VALIDATION_ERRORS + 1))
else
    echo -e "${GREEN}✅ global.json version: $GLOBAL_VERSION${NC}"
fi

echo ""
echo "2️⃣ Checking project files..."
PROJECT_FRAMEWORKS=$(get_project_frameworks)
if [ -z "$PROJECT_FRAMEWORKS" ]; then
    echo -e "${RED}❌ No project files found or no TargetFramework specified${NC}"
    VALIDATION_ERRORS=$((VALIDATION_ERRORS + 1))
else
    echo "📦 Found target frameworks:"
    while IFS= read -r framework; do
        echo "   - $framework"
        
        # Extract version number (e.g., net9.0 -> 9.0)
        framework_version=$(echo "$framework" | grep -o "[0-9]\+\.[0-9]\+")
        
        if [ "$GLOBAL_VERSION" != "NOT_FOUND" ] && [ "$framework_version" != "$GLOBAL_VERSION" ]; then
            echo -e "${RED}❌ Framework version mismatch: $framework vs global.json $GLOBAL_VERSION${NC}"
            VALIDATION_ERRORS=$((VALIDATION_ERRORS + 1))
        else
            echo -e "${GREEN}✅ Framework version consistent: $framework_version${NC}"
        fi
    done <<< "$PROJECT_FRAMEWORKS"
fi

echo ""
echo "3️⃣ Checking documentation consistency..."
if [ "$GLOBAL_VERSION" != "NOT_FOUND" ]; then
    check_documentation_versions "$GLOBAL_VERSION"
else
    echo -e "${YELLOW}⚠️ Skipping documentation check due to missing global.json${NC}"
fi

echo ""
echo "4️⃣ Checking for downgrade protection..."
DOWNGRADE_WARNINGS=$(find "$REPO_ROOT" -name "*.csproj" -exec grep -l "Never.*downgrade\|Never.*revert" {} \; | wc -l)
TOTAL_PROJECTS=$(find "$REPO_ROOT" -name "*.csproj" | wc -l)

if [ "$DOWNGRADE_WARNINGS" -eq "$TOTAL_PROJECTS" ]; then
    echo -e "${GREEN}✅ All $TOTAL_PROJECTS project files have downgrade protection warnings${NC}"
else
    echo -e "${YELLOW}⚠️ Only $DOWNGRADE_WARNINGS of $TOTAL_PROJECTS project files have downgrade protection warnings${NC}"
    echo "Consider adding comments like '<!-- WARNING: Never revert to .NET 8.0 - Project requires .NET 9.0 -->'"
fi

echo ""
echo "📊 Validation Summary:"
if [ $VALIDATION_ERRORS -eq 0 ]; then
    echo -e "${GREEN}✅ All framework versions are consistent!${NC}"
    echo "✅ global.json: $GLOBAL_VERSION"
    echo "✅ Project files: $(echo "$PROJECT_FRAMEWORKS" | tr '\n' ' ')"
    echo "✅ Documentation: Consistent"
    exit 0
else
    echo -e "${RED}❌ Found $VALIDATION_ERRORS validation errors${NC}"
    echo ""
    echo "🛠️ To fix framework inconsistencies:"
    echo "1. Update global.json to the desired .NET version"
    echo "2. Update all *.csproj files to use matching TargetFramework"
    echo "3. Update documentation in docs/getting-started/QUICK_START.md and README.md"
    echo "4. Add downgrade protection warnings to project files"
    exit 1
fi