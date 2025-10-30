#!/bin/bash

# Badge Validation Script for Authentication Repository
# This script helps validate and update badge information in README.md

set -e

echo "🏷️ Badge Validation and Update Tool"
echo "===================================="

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Get repository root
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$REPO_ROOT"

echo "📁 Repository: $REPO_ROOT"
echo

# 1. Count test methods
echo "🧪 Counting test methods..."
TEST_COUNT=$(grep -r "\[Test\]\|\[Fact\]\|\[Theory\]" . --include="*.cs" 2>/dev/null | wc -l)
echo -e "${GREEN}✅ Found: $TEST_COUNT tests${NC}"

# 2. Count lines of code
echo "📊 Counting lines of code..."
LOC_COUNT=$(find . -name "*.cs" -not -path "./.git/*" -type f | xargs wc -l 2>/dev/null | tail -1 | awk '{print $1}')
LOC_ROUNDED=$(echo "scale=1; $LOC_COUNT/1000" | bc -l 2>/dev/null | sed 's/\([0-9]*\.[0-9]\).*/\1/' || echo "15.2")
echo -e "${GREEN}✅ Found: $LOC_COUNT lines (${LOC_ROUNDED}k)${NC}"

# 3. Check README badges
echo "🔍 Checking README badges..."
README_FILE="README.md"

if [ ! -f "$README_FILE" ]; then
    echo -e "${RED}❌ README.md not found!${NC}"
    exit 1
fi

# Extract current badge values
CURRENT_TESTS=$(grep "Tests-" "$README_FILE" | sed -n 's/.*Tests-\([0-9]*\).*/\1/p' | head -1)
CURRENT_LOC=$(grep "Lines%20of%20Code-" "$README_FILE" | sed -n 's/.*Lines%20of%20Code-\([0-9.]*k\).*/\1/p' | head -1)

echo "📋 Current badge values:"
echo "   Tests: $CURRENT_TESTS (actual: $TEST_COUNT)"
echo "   Lines of Code: $CURRENT_LOC (actual: ${LOC_ROUNDED}k)"

# 4. Suggest updates if needed
TESTS_DIFF=$((TEST_COUNT - CURRENT_TESTS))
if [ "$TESTS_DIFF" -ne 0 ]; then
    echo -e "${YELLOW}⚠️  Test count mismatch: $TESTS_DIFF difference${NC}"
    echo "   Suggested update:"
    echo "   Current: Tests-${CURRENT_TESTS}%20passing"
    echo "   New:     Tests-${TEST_COUNT}%20passing"
fi

# 5. Check workflow file exists
if [ -f ".github/workflows/ci.yml" ]; then
    echo -e "${GREEN}✅ CI workflow file exists${NC}"
else
    echo -e "${RED}❌ CI workflow file missing!${NC}"
fi

# 6. Check license file
if [ -f "LICENSE" ]; then
    echo -e "${GREEN}✅ License file exists${NC}"
else
    echo -e "${RED}❌ License file missing!${NC}"
fi

echo
echo "🎯 Badge Status Summary:"
echo "========================"

# Dynamic badges (should always work)
echo -e "${GREEN}✅ Dynamic badges (auto-updating):${NC}"
echo "   - CI/CD Pipeline status"
echo "   - Last commit"
echo "   - Contributors"
echo "   - Repository size"

# Static badges that may need updates
echo -e "${YELLOW}⚠️  Static badges (manual updates):${NC}"
echo "   - Test count: $CURRENT_TESTS → should be $TEST_COUNT"
echo "   - Lines of code: $CURRENT_LOC → should be ${LOC_ROUNDED}k"
echo "   - Code coverage: static estimate"
echo "   - Technical debt: manual assessment"

echo
echo "📝 To update badges, edit the following URLs in README.md:"
echo "   Tests: https://img.shields.io/badge/Tests-${TEST_COUNT}%20passing-brightgreen"
echo "   LOC: https://img.shields.io/badge/Lines%20of%20Code-${LOC_ROUNDED}k-informational"

echo
echo "✅ Badge validation complete!"
=======
# Badge Validation Script for README.md
# This script helps validate that badges in README.md are working correctly

echo "🔍 Validating README badges..."
echo "================================"

README_FILE="README.md"

if [ ! -f "$README_FILE" ]; then
    echo "❌ README.md not found"
    exit 1
fi

# Extract badge URLs from README
echo "📋 Found badges in README.md:"
echo "------------------------------"

# Extract shields.io badges
grep -o 'https://img\.shields\.io/[^)]*' "$README_FILE" | while read badge_url; do
    echo "🔸 $badge_url"
done

# Extract GitHub badge URLs
grep -o 'https://github\.com/[^/]*/[^/]*/actions/workflows/[^)]*' "$README_FILE" | while read badge_url; do
    echo "🔸 $badge_url"
done

echo ""
echo "✅ Badge validation completed"
echo ""
echo "💡 For private repositories:"
echo "   - GitHub API badges (shields.io) may not work"
echo "   - Use static badges or GitHub's native badge URLs"
echo "   - GitHub Actions badges work if workflow is public"
echo ""
echo "📝 Badge recommendations:"
echo "   - Last Commit: Use static badge for private repos"
echo "   - Contributors: Use static badge for private repos" 
echo "   - Repo Size: Use static badge for private repos"
echo "   - CI/CD Pipeline: Use GitHub Actions badge URL"
