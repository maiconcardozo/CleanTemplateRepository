@echo off
REM Single command test runner for CI/CD and development
REM This script provides a simple interface to run all tests

setlocal EnableDelayedExpansion

echo 🧪 Authentication Project - Test Runner
echo ========================================

REM Function to check .NET version compatibility
call :check_dotnet_version
if errorlevel 1 exit /b 1

REM Navigate to project root
cd /d "%~dp0\.."

REM Check if solution exists
if not exist "Solution\Authentication.sln" (
    echo ❌ Solution file not found at Solution\Authentication.sln
    echo 📂 Current directory: %CD%
    echo 📁 Available files:
    dir /b | head -10
    exit /b 1
)

echo 📦 Restoring packages...
dotnet restore Solution\Authentication.sln
if errorlevel 1 (
    echo ❌ Package restore failed
    exit /b 1
) else (
    echo ✅ Package restore successful
)

echo 🏗️ Building solution...
dotnet build Solution\Authentication.sln --configuration Release --no-restore
if errorlevel 1 (
    echo ❌ Build failed
    exit /b 1
) else (
    echo ✅ Build successful
)

echo 🧪 Running all tests...
echo 📊 Test execution details:
echo   • Configuration: Release
echo   • Verbosity: Normal
echo   • Results: Console output + TRX files
echo.

REM Create TestResults directory if it doesn't exist
if not exist "TestResults" mkdir TestResults

dotnet test Solution\Authentication.sln --configuration Release --no-build --verbosity normal --logger trx --results-directory TestResults
if errorlevel 1 (
    echo.
    echo ❌ Some tests failed or test execution encountered errors
    echo 📁 Check TestResults\ directory for detailed results
    exit /b 1
) else (
    echo.
    echo ✅ Test execution completed successfully!
    
    REM Show test result summary
    if exist "TestResults" (
        echo 📊 Test Results Summary:
        for /f %%a in ('dir /b TestResults\*.trx 2^>nul ^| find /c /v ""') do set TRX_COUNT=%%a
        echo   • TRX files generated: !TRX_COUNT!
        echo   • Results location: TestResults\
        
        REM List TRX files
        if !TRX_COUNT! gtr 0 (
            echo   • Files:
            for %%f in (TestResults\*.trx) do echo     - %%~nxf
        )
    )
)

echo.
echo 🎉 Test runner completed successfully!
echo 📋 Next steps:
echo   • Review test results in TestResults\ directory
echo   • Check build output for any warnings
echo   • Run 'scripts\run-tests.bat coverage' for coverage analysis

goto :eof

:check_dotnet_version
set "required_version=9.0"
for /f "tokens=*" %%i in ('dotnet --version 2^>nul') do set current_version=%%i

echo 🔧 .NET Version: !current_version!

if "!current_version!"=="ECHO is off." (
    echo ❌ .NET SDK not found. Please install .NET 9.0 SDK.
    echo 📥 Download from: https://dotnet.microsoft.com/download/dotnet/9.0
    exit /b 1
)

echo !current_version! | findstr /r "^9\." >nul
if errorlevel 1 (
    echo ⚠️  Warning: This project requires .NET 9.0, but found !current_version!
    echo 📥 Download .NET 9.0 from: https://dotnet.microsoft.com/download/dotnet/9.0
    echo 🔄 Attempting to continue with current version...
) else (
    echo ✅ .NET version compatible
)

exit /b 0