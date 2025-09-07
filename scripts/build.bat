@echo off
setlocal enabledelayedexpansion

REM Authentication Build Script para Windows
REM Este script facilita a compilação do projeto Authentication

echo 🏗️ Authentication Build Script
echo ==============================

REM Navegar para o diretório raiz do projeto
cd /d "%~dp0\.."

REM Função para mostrar ajuda
if "%1"=="help" goto :show_help
if "%1"=="-h" goto :show_help
if "%1"=="--help" goto :show_help

REM Verificar se o arquivo de solução existe
if not exist "Solution\Authentication.sln" (
    echo ❌ Arquivo de solução não encontrado!
    echo Verifique se você está na raiz do projeto Authentication.
    exit /b 1
)

REM Verificar .NET 9.0
echo 🔍 Verificando versão do .NET...
for /f "tokens=*" %%i in ('dotnet --version 2^>nul') do set DOTNET_VERSION=%%i
if "%DOTNET_VERSION%"=="" (
    echo ❌ .NET SDK não encontrado!
    echo Instale o .NET 9.0 SDK de: https://dotnet.microsoft.com/download/dotnet/9.0
    exit /b 1
)

REM Verificar se é versão 9.x
echo %DOTNET_VERSION% | findstr /r "^9\." >nul
if %errorlevel% neq 0 (
    echo ❌ .NET 9.0 SDK não encontrado!
    echo Versão atual: %DOTNET_VERSION%
    echo Instale o .NET 9.0 SDK de: https://dotnet.microsoft.com/download/dotnet/9.0
    exit /b 1
)
echo ✅ .NET versão: %DOTNET_VERSION%

REM Processar argumentos
set "command=%1"
if "%command%"=="" set "command=debug"

if "%command%"=="debug" goto :build_debug
if "%command%"=="release" goto :build_release
if "%command%"=="clean" goto :build_clean
if "%command%"=="restore" goto :restore_only
if "%command%"=="verify" goto :verify_all

echo ❌ Opção inválida: %1
echo.
goto :show_help

:build_debug
echo 🛠️ Restaurando dependências...
dotnet restore Solution\Authentication.sln
if %errorlevel% neq 0 exit /b 1

echo 🏃 Compilando em modo Debug...
echo.
dotnet build Solution\Authentication.sln --configuration Debug
if %errorlevel% equ 0 (
    echo.
    echo ✅ Compilação concluída com sucesso!
) else (
    echo.
    echo ❌ Falha na compilação!
    exit /b 1
)
goto :end

:build_release
echo 🛠️ Restaurando dependências...
dotnet restore Solution\Authentication.sln
if %errorlevel% neq 0 exit /b 1

echo 🏃 Compilando em modo Release...
echo.
dotnet build Solution\Authentication.sln --configuration Release
if %errorlevel% equ 0 (
    echo.
    echo ✅ Compilação concluída com sucesso!
) else (
    echo.
    echo ❌ Falha na compilação!
    exit /b 1
)
goto :end

:build_clean
echo 🧹 Limpando projeto...
dotnet clean Solution\Authentication.sln
echo 🛠️ Restaurando dependências...
dotnet restore Solution\Authentication.sln
if %errorlevel% neq 0 exit /b 1

echo 🏃 Compilando em modo Debug...
echo.
dotnet build Solution\Authentication.sln --configuration Debug
if %errorlevel% equ 0 (
    echo.
    echo ✅ Compilação concluída com sucesso!
) else (
    echo.
    echo ❌ Falha na compilação!
    exit /b 1
)
goto :end

:restore_only
echo 📦 Restaurando dependências...
dotnet restore Solution\Authentication.sln
if %errorlevel% equ 0 (
    echo ✅ Dependências restauradas com sucesso!
) else (
    echo ❌ Falha ao restaurar dependências!
    exit /b 1
)
goto :end

:verify_all
echo 🔍 Verificação completa do projeto...
echo.

echo 📦 Restaurando dependências...
dotnet restore Solution\Authentication.sln
if %errorlevel% neq 0 exit /b 1

echo 🏃 Compilando em modo Release...
echo.
dotnet build Solution\Authentication.sln --configuration Release
if %errorlevel% neq 0 (
    echo ❌ Falha na compilação!
    exit /b 1
)
echo ✅ Compilação concluída com sucesso!

echo.
echo 🧪 Executando testes...
call run-tests.bat all
if %errorlevel% neq 0 exit /b 1

echo.
echo 🎉 Verificação completa bem-sucedida!
echo ✅ Projeto compila corretamente
echo ✅ Todos os testes passaram
goto :end

:show_help
echo Uso: %0 [opção]
echo.
echo Opções:
echo   debug         Compila em modo Debug (padrão)
echo   release       Compila em modo Release
echo   clean         Limpa e reconstrói
echo   restore       Apenas restaura dependências
echo   verify        Verifica compilação e testes
echo   help          Mostra esta ajuda
echo.
echo Exemplos:
echo   %0              # Compila em modo Debug
echo   %0 release      # Compila em modo Release
echo   %0 verify       # Verifica tudo funciona
goto :end

:end
echo.
echo 🎉 Script executado com sucesso!