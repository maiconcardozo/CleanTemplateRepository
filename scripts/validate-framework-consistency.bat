@echo off
REM Framework Consistency Validation Script
REM Prevents accidental framework downgrades by validating consistency
REM between global.json, project files, and documentation

setlocal enabledelayedexpansion

set "SCRIPT_DIR=%~dp0"
set "REPO_ROOT=%SCRIPT_DIR%.."
set "VALIDATION_ERRORS=0"

echo 🔍 Validating .NET Framework Consistency...
echo Repository: %REPO_ROOT%
echo.

REM Function to extract .NET version from global.json
set "GLOBAL_VERSION="
if exist "%REPO_ROOT%\build\global.json" (
    for /f "tokens=2 delims=:, " %%a in ('findstr "version" "%REPO_ROOT%\build\global.json"') do (
        set "temp=%%a"
        set "temp=!temp:"=!"
        for /f "tokens=1,2 delims=." %%x in ("!temp!") do (
            set "GLOBAL_VERSION=%%x.%%y"
        )
    )
) else (
    set "GLOBAL_VERSION=NOT_FOUND"
)

echo 1️⃣ Checking global.json...
if "%GLOBAL_VERSION%"=="NOT_FOUND" (
    echo ❌ global.json not found
    set /a VALIDATION_ERRORS+=1
) else (
    echo ✅ global.json version: %GLOBAL_VERSION%
)

echo.
echo 2️⃣ Checking project files...

REM Check project files for TargetFramework
set "PROJECT_CHECK_PASSED=true"
for /r "%REPO_ROOT%" %%f in (*.csproj) do (
    findstr /c:"TargetFramework" "%%f" >nul 2>&1
    if !errorlevel! equ 0 (
        for /f "tokens=2 delims=<>" %%a in ('findstr /c:"TargetFramework" "%%f"') do (
            set "framework=%%a"
            echo 📦 Found framework in %%~nf: !framework!
            
            REM Extract version (e.g., net9.0 -> 9.0)
            set "framework_version=!framework:net=!"
            
            if not "%GLOBAL_VERSION%"=="NOT_FOUND" (
                if not "!framework_version!"=="%GLOBAL_VERSION%" (
                    echo ❌ Framework version mismatch in %%~nf: !framework! vs global.json %GLOBAL_VERSION%
                    set /a VALIDATION_ERRORS+=1
                    set "PROJECT_CHECK_PASSED=false"
                ) else (
                    echo ✅ Framework version consistent: !framework_version!
                )
            )
        )
    )
)

echo.
echo 3️⃣ Checking documentation consistency...

if not "%GLOBAL_VERSION%"=="NOT_FOUND" (
    REM Check QUICK_START.md
    if exist "%REPO_ROOT%\docs\QUICK_START.md" (
        findstr /c:".NET %GLOBAL_VERSION%" "%REPO_ROOT%\docs\QUICK_START.md" >nul 2>&1
        if !errorlevel! neq 0 (
            echo ❌ QUICK_START.md does not reference .NET %GLOBAL_VERSION%
            set /a VALIDATION_ERRORS+=1
        ) else (
            echo ✅ QUICK_START.md references correct .NET version
        )
    )
    
    REM Check README.md
    if exist "%REPO_ROOT%\README.md" (
        findstr /c:".NET %GLOBAL_VERSION%" "%REPO_ROOT%\README.md" >nul 2>&1
        if !errorlevel! neq 0 (
            echo ⚠️ README.md may not reference .NET %GLOBAL_VERSION%
        ) else (
            echo ✅ README.md references correct .NET version
        )
    )
) else (
    echo ⚠️ Skipping documentation check due to missing global.json
)

echo.
echo 4️⃣ Checking for downgrade protection...

set "PROJECTS_WITH_WARNINGS=0"
set "TOTAL_PROJECTS=0"

for /r "%REPO_ROOT%" %%f in (*.csproj) do (
    set /a TOTAL_PROJECTS+=1
    findstr /c:"Never.*downgrade" "%%f" >nul 2>&1
    if !errorlevel! equ 0 (
        set /a PROJECTS_WITH_WARNINGS+=1
    ) else (
        findstr /c:"Never.*revert" "%%f" >nul 2>&1
        if !errorlevel! equ 0 (
            set /a PROJECTS_WITH_WARNINGS+=1
        )
    )
)

if !PROJECTS_WITH_WARNINGS! equ !TOTAL_PROJECTS! (
    echo ✅ All !TOTAL_PROJECTS! project files have downgrade protection warnings
) else (
    echo ⚠️ Only !PROJECTS_WITH_WARNINGS! of !TOTAL_PROJECTS! project files have downgrade protection warnings
    echo Consider adding comments like ^<!-- WARNING: Never revert to .NET 8.0 - Project requires .NET 9.0 --^>
)

echo.
echo 📊 Validation Summary:
if %VALIDATION_ERRORS%==0 (
    echo ✅ All framework versions are consistent!
    echo ✅ global.json: %GLOBAL_VERSION%
    echo ✅ Project files: Consistent
    echo ✅ Documentation: Consistent
    exit /b 0
) else (
    echo ❌ Found %VALIDATION_ERRORS% validation errors
    echo.
    echo 🛠️ To fix framework inconsistencies:
    echo 1. Update global.json to the desired .NET version
    echo 2. Update all *.csproj files to use matching TargetFramework
    echo 3. Update documentation in docs/getting-started/QUICK_START.md and README.md
    echo 4. Add downgrade protection warnings to project files
    exit /b 1
)