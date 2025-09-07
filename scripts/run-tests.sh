#!/bin/bash

# CleanTemplate Tests Runner
# Este script facilita a execução de testes em projetos .NET

set -e

echo "🧪 CleanTemplate Tests Runner"
echo "=============================="

# Função para mostrar ajuda
show_help() {
    echo "Uso: $0 [opção]"
    echo ""
    echo "Opções:"
    echo "  all           Executa todos os testes (padrão)"
    echo "  integration   Executa apenas testes de integração"
    echo "  unit          Executa apenas testes unitários"
    echo "  coverage      Executa testes com cobertura de código"
    echo "  watch         Executa testes em modo watch"
    echo "  verbose       Executa testes com saída detalhada"
    echo "  clean         Limpa e reconstrói antes de executar"
    echo "  help          Mostra esta ajuda"
    echo ""
    echo "Exemplos:"
    echo "  $0                # Executa todos os testes"
    echo "  $0 integration    # Executa apenas testes de integração"
    echo "  $0 coverage       # Executa com cobertura de código"
}

# Navegar para o diretório raiz do projeto
cd "$(dirname "$0")/.."

# Verificar se existem projetos de teste
TEST_PROJECTS=$(find Src -name "*.Tests.csproj" 2>/dev/null)
if [ -z "$TEST_PROJECTS" ]; then
    echo "❌ Nenhum projeto de testes encontrado!"
    echo "Crie um projeto de teste primeiro usando 'dotnet new xunit' em Src/"
    exit 1
fi

echo "📁 Projetos de teste encontrados:"
echo "$TEST_PROJECTS" | sed 's/^/  - /'

# Verificar se existe alguma solução
SOLUTION_FILE=$(find Solution -name "*.sln" 2>/dev/null | head -1)
if [ -n "$SOLUTION_FILE" ]; then
    echo "📁 Usando solução: $SOLUTION_FILE"
    # Restaurar dependências se necessário
    echo "📦 Restaurando dependências..."
    dotnet restore "$SOLUTION_FILE"
else
    echo "ℹ️ Nenhuma solução encontrada, testando projetos individualmente"
fi

# Função para executar testes
run_tests() {
    local test_command="$1"
    echo "🏃 Executando: $test_command"
    echo ""
    
    if eval "$test_command"; then
        echo ""
        echo "✅ Testes executados com sucesso!"
    else
        echo ""
        echo "❌ Alguns testes falharam!"
        exit 1
    fi
}

# Função para executar testes em todos os projetos
run_all_tests() {
    local filter="$1"
    local description="$2"
    
    echo "$description"
    
    if [ -n "$SOLUTION_FILE" ]; then
        if [ -n "$filter" ]; then
            run_tests "dotnet test \"$SOLUTION_FILE\" --filter \"$filter\""
        else
            run_tests "dotnet test \"$SOLUTION_FILE\""
        fi
    else
        # Executar em cada projeto individualmente
        echo "$TEST_PROJECTS" | while read -r project; do
            if [ -n "$filter" ]; then
                run_tests "dotnet test \"$project\" --filter \"$filter\""
            else
                run_tests "dotnet test \"$project\""
            fi
        done
    fi
}

# Processar argumentos
case "${1:-all}" in
    "all")
        run_all_tests "" "🎯 Executando todos os testes..."
        ;;
    
    "integration")
        run_all_tests "FullyQualifiedName~Integration" "🔗 Executando testes de integração..."
        ;;
    
    "unit")
        run_all_tests "FullyQualifiedName~Unit" "🧩 Executando testes unitários..."
        ;;
    
    "coverage")
        echo "📊 Executando testes com cobertura de código..."
        if [ -n "$SOLUTION_FILE" ]; then
            run_tests "dotnet test \"$SOLUTION_FILE\" --collect:\"XPlat Code Coverage\""
        else
            # Usar o primeiro projeto de teste encontrado
            FIRST_TEST_PROJECT=$(echo "$TEST_PROJECTS" | head -1)
            run_tests "dotnet test \"$FIRST_TEST_PROJECT\" --collect:\"XPlat Code Coverage\""
        fi
        echo ""
        echo "📈 Relatório de cobertura gerado em: TestResults/"
        ;;
    
    "watch")
        echo "👀 Executando testes em modo watch..."
        echo "Pressione Ctrl+C para parar"
        # Usar o primeiro projeto de teste encontrado para watch
        FIRST_TEST_PROJECT=$(echo "$TEST_PROJECTS" | head -1)
        dotnet watch test "$FIRST_TEST_PROJECT"
        ;;
    
    "verbose")
        run_all_tests "" "📝 Executando testes com saída detalhada..."
        ;;
    
    "clean")
        echo "🧹 Limpando e reconstruindo..."
        if [ -n "$SOLUTION_FILE" ]; then
            dotnet clean "$SOLUTION_FILE"
            dotnet build "$SOLUTION_FILE"
        else
            echo "$TEST_PROJECTS" | while read -r project; do
                dotnet clean "$project"
                dotnet build "$project"
            done
        fi
        run_all_tests "" "🎯 Executando todos os testes..."
        ;;
    
    "help"|"-h"|"--help")
        show_help
        ;;
    
    *)
        echo "❌ Opção inválida: $1"
        echo ""
        show_help
        exit 1
        ;;
esac

echo ""
echo "🎉 Script executado com sucesso!"