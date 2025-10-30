# 🔒 Framework Downgrade Prevention Guide

This document explains how the Authentication project protects against accidental .NET framework downgrades and how to maintain this protection.

## 🚨 Why Prevent Downgrades?

The Authentication project requires .NET 8.0 for critical reasons:

1. **Package Compatibility**: Entity Framework Core 8.0.8 and other dependencies require .NET 8.0
2. **Security Features**: Newer versions include essential security fixes
3. **Performance**: .NET 8.0 offers significant performance improvements
4. **Features**: The code uses .NET 8.0-specific features

## 🛡️ Multi-Layer Protection System

### 1. **global.json** - SDK Protection
```json
{
  "sdk": {
    "version": "9.0.101",
    "rollForward": "latestMajor",
    "allowPrerelease": false
  }
}
```

**How it works:**
- Forces the use of .NET 8.0.101 SDK or higher
- `rollForward: "latestMajor"` allows automatic updates to newer versions
- `allowPrerelease: false` prevents use of beta/preview versions

### 2. **.csproj Files** - Target Framework Protection

All `.csproj` files contain:

```xml
<PropertyGroup>
    <!-- WARNING: Maintain .NET 8.0 - Project requires .NET 8.0 for all dependencies and features -->
    <TargetFramework>net8.0</TargetFramework>
</PropertyGroup>

<ItemGroup>
    <!-- Package versions updated for .NET 8.0 compatibility - Never downgrade to older versions -->
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.8" />
    <!-- ... other packages with 9.x versions ... -->
</ItemGroup>
```

**How it works:**
- Explicit comments alert developers about restrictions
- All dependencies use versions compatible with .NET 8.0
- Target framework fixed at `net8.0`

### 3. **Automatic Validation Script**

#### Manual Execution:
```bash
# Linux/Mac
./scripts/validate-framework-consistency.sh

# Windows
scripts\validate-framework-consistency.bat
```

#### What the script verifies:
- ✅ Consistency between `global.json` and `.csproj` files
- ✅ Documentation updated with correct version
- ✅ Presence of protection warnings in all projects
- ✅ No references to old versions

### 4. **CI/CD Protection** - GitHub Actions

The `.github/workflows/ci.yml` workflow includes:

```yaml
- name: Validate Framework Consistency
  run: |
    echo "🔍 Validating .NET framework consistency across project..."
    chmod +x ./scripts/validate-framework-consistency.sh
    ./scripts/validate-framework-consistency.sh
    echo "✅ Framework consistency validated"
```

**How it works:**
- Runs automatically on each push/pull request
- Build fails if inconsistencies are found
- Prevents merging code with framework issues

## 🔧 How to Maintain Protection

### When Adding New Projects:

1. **Always use .NET 8.0**:
```bash
dotnet new webapi -n NewService -f net8.0
```

2. **Add warnings in .csproj**:
```xml
<!-- WARNING: Maintain .NET 8.0 - Project requires .NET 8.0 for all dependencies and features -->
<TargetFramework>net8.0</TargetFramework>
```

3. **Use compatible packages**:
```xml
<!-- Package versions updated for .NET 8.0 compatibility - Never downgrade to older versions -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.8" />
```

4. **Run validation**:
```bash
./scripts/validate-framework-consistency.sh
```

### When Updating Dependencies:

1. **Always prefer 9.x versions**:
```bash
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.8
```

2. **Avoid 8.x versions**:
```bash
# ❌ NEVER do this:
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.x

# ✅ Always do this:
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.x
```

3. **Validate after updates**:
```bash
./scripts/validate-framework-consistency.sh
dotnet build
dotnet test
```

### When Reviewing Pull Requests:

Always verify:
- [ ] No `.csproj` file was changed to use `net8.0`
- [ ] No dependency was downgraded to 8.x version
- [ ] `global.json` was not incorrectly modified
- [ ] Documentation maintains references to .NET 8.0
- [ ] CI/CD passed framework validation

## 🚫 Problem Signs

### Common Errors Indicating Downgrade:

```bash
# Compilation error due to incompatibility:
error NU1202: Package Microsoft.EntityFrameworkCore 8.0.8 is not compatible with net8.0

# Runtime error due to missing features:
System.NotSupportedException: This feature requires .NET 8.0 or higher

# Validator error:
❌ Framework version mismatch: net8.0 vs global.json 9.0
```

### How to Fix Problems:

1. **Revert problematic changes**:
```bash
git checkout HEAD~1 -- path/to/problematic.csproj
```

2. **Update framework in projects**:
```xml
<TargetFramework>net8.0</TargetFramework>
```

3. **Update dependencies**:
```bash
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.8
```

4. **Run validation**:
```bash
./scripts/validate-framework-consistency.sh
```

## 🎯 Best Practices Summary

| ✅ **ALWAYS DO** | ❌ **NEVER DO** |
|---|---|
| Use .NET 8.0 SDK | Downgrade to .NET 8.0 |
| Run `validate-framework-consistency.sh` | Ignore framework warnings |
| Keep documentation updated | Mix framework versions |
| Use 9.x version packages | Use 8.x version packages |
| Add warnings in new projects | Remove existing warnings |

## 🔗 Additional Resources

- [.NET 8.0 Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- [.NET Framework Targeting](https://docs.microsoft.com/en-us/dotnet/standard/frameworks)
- [global.json Documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/global-json)
- [Authentication Project - README.md](../../README.md)

---

⚠️ **IMPORTANT**: This protection system should be maintained and updated whenever new projects are added to the repository.