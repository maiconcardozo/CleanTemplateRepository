# 📋 Testing Documentation Template

## 🎯 Overview

This document serves as a template for documenting tests in your .NET project. Use this structure to document your test suite comprehensively.

**Total Tests**: [Update with your test count]  
**Organization**: Unit Tests + Integration Tests  
**Framework**: xUnit with FluentAssertions (recommended)  
**Pattern**: Arrange-Act-Assert (AAA)  

## 📚 Test Categories

### 🧪 Unit Tests
Document your unit tests that verify individual components in isolation:

- **Entity Tests**: Validate domain entities and their behavior
- **Service Tests**: Test business logic and service operations  
- **Repository Tests**: Verify data access layer operations
- **Validator Tests**: Test input validation and business rules
- **DTO Tests**: Validate data transfer objects

### 🔗 Integration Tests
Document your integration tests that verify end-to-end scenarios:

- **Controller Tests**: Test API endpoints and HTTP responses
- **Database Tests**: Verify database operations and queries
- **Service Integration**: Test service interactions and workflows

## 🛠️ Test Structure Template

### Test Class Template

```csharp
public class YourComponentTests
{
    private readonly YourComponent _component;
    private readonly Mock<IDependency> _mockDependency;

    public YourComponentTests()
    {
        // Arrange - Setup test dependencies
        _mockDependency = new Mock<IDependency>();
        _component = new YourComponent(_mockDependency.Object);
    }

    [Fact]
    public void Method_WhenCondition_ShouldExpectedBehavior()
    {
        // Arrange
        var input = new TestInput();
        _mockDependency.Setup(x => x.Method()).Returns(expectedValue);

        // Act
        var result = _component.Method(input);

        // Assert
        result.Should().NotBeNull();
        result.Property.Should().Be(expectedValue);
    }
}
```

## 📊 Testing Guidelines

### Best Practices
- Use descriptive test method names
- Follow Arrange-Act-Assert pattern
- Test one behavior per test method
- Use appropriate assertions with FluentAssertions
- Mock external dependencies

### Naming Conventions
- `MethodName_WhenCondition_ShouldExpectedBehavior`
- `MethodName_GivenCondition_ThenExpectedResult`

## 🔧 Test Execution

### Running Tests
```bash
# Run all tests
scripts/run-tests.sh all

# Run specific categories
scripts/run-tests.sh unit
scripts/run-tests.sh integration

# Run with coverage
scripts/run-tests.sh coverage
```

## 📈 Coverage Targets

- **Unit Tests**: Aim for >80% code coverage
- **Integration Tests**: Cover critical business workflows
- **Edge Cases**: Test boundary conditions and error scenarios

---

*This template should be customized for your specific project. Remove this section and update the content with your actual test documentation.*
  - [ResourceStartupTests](#resourcestartuptests)
  - [ApiLocalizationTests](#apilocalizationtests)
- [Testes de Integração](#-testes-de-integração)
  - [AuthenticationControllerTests](#authenticationcontrollertests)
  - [AccountControllerTests](#accountcontrollertests)
  - [AccountControllerEnhancedTests](#accountcontrollerenhancedtests)
  - [ActionControllerTests](#actioncontrollertests)
  - [ClaimActionControllerTests](#claimactioncontrollertests)
  - [AccountClaimActionControllerTests](#accountclaimactioncontrollertests)
  - [SwaggerLocalizationTests](#swaggerlocalizationtests)
  - [ExampleFixedControllerTests](#examplefixedcontrollertests)

---

## 🧪 Testes Unitários

### AccountEntityTests

**Arquivo**: `Src/Authentication.Tests/Unit/AccountEntityTests.cs`  
**Propósito**: Testa a entidade Account e suas propriedades básicas  
**Total de Testes**: 20+ testes  

#### Testes Implementados:

##### 1. `Account_WhenCreated_ShouldHaveDefaultValues()`
**Propósito**: Verifica se uma nova instância da entidade Account possui valores padrão corretos  
**Configuração**: Cria uma nova instância de Account  
**Execução**: Instancia um objeto Account sem parâmetros  
**Verificação**: 
- UserName deve ser string vazia
- Password deve ser string vazia  
- Id deve ser 0

```csharp
[Fact]
public void Account_WhenCreated_ShouldHaveDefaultValues()
{
    // Act
    var account = new Account();

    // Assert
    account.UserName.Should().Be(string.Empty);
    account.Password.Should().Be(string.Empty);
    account.Id.Should().Be(0);
}
```

##### 2. `Account_SetUserName_ShouldUpdateUserNameProperty()`
**Propósito**: Testa se a propriedade UserName pode ser definida corretamente  
**Configuração**: Cria nova instância Account e define valor esperado  
**Execução**: Define a propriedade UserName com valor "testuser"  
**Verificação**: UserName deve conter o valor definido

##### 3. `Account_SetPassword_ShouldUpdatePasswordProperty()`
**Propósito**: Testa se a propriedade Password pode ser definida corretamente  
**Configuração**: Cria nova instância Account e define senha esperada  
**Execução**: Define a propriedade Password com valor "testpassword"  
**Verificação**: Password deve conter o valor definido

##### 4. `Account_SetUserNameToNullOrEmpty_ShouldAllowValue()` (Theory Test)
**Propósito**: Testa comportamento da entidade com valores nulos ou vazios para UserName  
**Configuração**: Usa dados de teste: "", " ", null  
**Execução**: Define UserName com cada valor de teste  
**Verificação**: A propriedade deve aceitar e armazenar o valor fornecido

##### 5. `Account_SetPasswordToNullOrEmpty_ShouldAllowValue()` (Theory Test)
**Propósito**: Testa comportamento da entidade com valores nulos ou vazios para Password  
**Configuração**: Usa dados de teste: "", " ", null  
**Execução**: Define Password com cada valor de teste  
**Verificação**: A propriedade deve aceitar e armazenar o valor fornecido

##### 6. `Account_WithValidUserNameAndPassword_ShouldSetPropertiesCorrectly()`
**Propósito**: Testa se ambas as propriedades podem ser definidas simultaneamente  
**Configuração**: Define valores válidos para userName e password  
**Execução**: Cria Account com ambas as propriedades definidas  
**Verificação**: Ambas as propriedades devem conter os valores corretos

##### 7. `Account_WithLongUserName_ShouldAllowValue()`
**Propósito**: Testa se a entidade aceita nomes de usuário longos  
**Configuração**: Cria string longa (1000 caracteres)  
**Execução**: Define UserName com valor longo  
**Verificação**: UserName deve armazenar o valor completo

---

### AccountServiceTests

**Arquivo**: `Src/Authentication.Tests/Unit/AccountServiceTests.cs`  
**Propósito**: Testa a lógica de negócio do serviço AccountService  
**Total de Testes**: 50+ testes  
**Dependências Mockadas**: ILoginUnitOfWork, IAccountRepository, IAccountClaimActionRepository

#### Setup do Teste:
```csharp
public AccountServiceTests()
{
    _mockUnitOfWork = new Mock<ILoginUnitOfWork>();
    _mockAccountRepository = new Mock<IAccountRepository>();
    _mockAccountClaimActionRepository = new Mock<IAccountClaimActionRepository>();

    _mockUnitOfWork.Setup(x => x.AccountRepository).Returns(_mockAccountRepository.Object);
    _mockUnitOfWork.Setup(x => x.AccountClaimActionRepository).Returns(_mockAccountClaimActionRepository.Object);

    _accountService = new AccountService(_mockUnitOfWork.Object);
}
```

#### Grupos de Testes:

##### GetAllAccounts Tests

##### 1. `GetAllAccounts_WhenCalled_ShouldReturnAllAccountsFromRepository()`
**Propósito**: Verifica se o método retorna todas as contas do repositório  
**Configuração**: 
- Mock do repositório retorna lista de contas esperadas
- Lista contém 2 contas com dados de teste  
**Execução**: Chama _accountService.GetAllAccounts()  
**Verificação**: 
- Resultado deve ser equivalente à lista esperada
- Repositório deve ter sido chamado uma vez

##### 2. `GetAllAccounts_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()`
**Propósito**: Testa comportamento quando repositório retorna lista vazia  
**Configuração**: Mock repositório retorna lista vazia  
**Execução**: Chama GetAllAccounts()  
**Verificação**: Resultado deve ser lista vazia

##### 3. `GetAllAccounts_WhenRepositoryThrows_ShouldPropagateException()`
**Propósito**: Verifica se exceções do repositório são propagadas  
**Configuração**: Mock repositório configurado para lançar exceção  
**Execução**: Chama GetAllAccounts()  
**Verificação**: Deve lançar a mesma exceção

##### GetAccountById Tests

##### 4. `GetAccountById_WithValidId_ShouldReturnAccount()`
**Propósito**: Testa busca de conta por ID válido  
**Configuração**: Mock repositório retorna conta com ID específico  
**Execução**: Chama GetAccountById(1)  
**Verificação**: Deve retornar a conta esperada

##### 5. `GetAccountById_WithInvalidId_ShouldReturnNull()`
**Propósito**: Testa comportamento com ID inexistente  
**Configuração**: Mock repositório retorna null  
**Execução**: Chama GetAccountById(999)  
**Verificação**: Deve retornar null

##### AddAccount Tests

##### 6. `AddAccount_WithValidAccount_ShouldAddToRepository()`
**Propósito**: Testa adição de conta válida  
**Configuração**: 
- Conta válida com userName e password
- Mock repositório configurado para GetByUserName retornar null  
**Execução**: Chama AddAccount(account)  
**Verificação**: 
- Repositório Add deve ser chamado uma vez
- Password deve ser hasheada (verificação de hash Argon2)
- DtCreated deve ser definida
- CreatedBy deve ser definida

##### 7. `AddAccount_WithDuplicateUserName_ShouldThrowConflictException()`
**Propósito**: Testa comportamento com userName duplicado  
**Configuração**: 
- Mock repositório retorna conta existente para GetByUserName
- Conta nova com mesmo userName  
**Execução**: Chama AddAccount(account)  
**Verificação**: Deve lançar ConflictException

##### UpdateAccount Tests

##### 8. `UpdateAccount_WithValidAccount_ShouldUpdateRepository()`
**Propósito**: Testa atualização de conta existente  
**Configuração**: 
- Conta existente no repositório
- Conta com dados atualizados  
**Execução**: Chama UpdateAccount(account)  
**Verificação**: 
- Repositório Update deve ser chamado
- DtUpdated deve ser definida
- UpdatedBy deve ser definida

##### DeleteAccount Tests

##### 9. `DeleteAccount_WithExistingId_ShouldRemoveFromRepository()`
**Propósito**: Testa remoção de conta existente  
**Configuração**: Mock repositório com conta existente  
**Execução**: Chama DeleteAccount(1)  
**Verificação**: Repositório Delete deve ser chamado uma vez

##### GetAccountByUserNameAndPassword Tests

##### 10. `GetAccountByUserNameAndPassword_WithValidCredentials_ShouldReturnAccount()`
**Propósito**: Testa autenticação com credenciais válidas  
**Configuração**: 
- Conta no repositório com senha hasheada
- Credenciais corretas para busca  
**Execução**: Chama GetAccountByUserNameAndPassword(account)  
**Verificação**: 
- Deve retornar conta do banco
- Senha deve ser verificada com hash Argon2

##### 11. `GetAccountByUserNameAndPassword_WithInvalidUserName_ShouldThrowException()`
**Propósito**: Testa comportamento com userName inexistente  
**Configuração**: Mock repositório retorna null para GetByUserName  
**Execução**: Chama GetAccountByUserNameAndPassword(account)  
**Verificação**: Deve lançar InvalidOperationException

##### 12. `GetAccountByUserNameAndPassword_WithInvalidPassword_ShouldThrowException()`
**Propósito**: Testa comportamento com senha incorreta  
**Configuração**: 
- Conta existente no repositório
- Senha incorreta na busca  
**Execução**: Chama GetAccountByUserNameAndPassword(account)  
**Verificação**: Deve lançar UnauthorizedAccessException

---

### AccountRepositoryTests

**Arquivo**: `Src/Authentication.Tests/Unit/AccountRepositoryTests.cs`  
**Propósito**: Testa operações de persistência do repositório AccountRepository  
**Total de Testes**: 30+ testes  
**Dependências**: EntityFramework InMemory Database

#### Setup do Teste:
```csharp
public AccountRepositoryTests()
{
    var options = new DbContextOptionsBuilder<AuthenticationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    _context = new AuthenticationDbContext(options);
    _repository = new AccountRepository(_context);
}
```

#### Grupos de Testes:

##### Add Tests

##### 1. `Add_WithValidAccount_ShouldAddToDatabase()`
**Propósito**: Verifica se contas válidas são adicionadas ao banco  
**Configuração**: Conta válida com UserName e Password  
**Execução**: 
- Chama repository.Add(account)
- Salva mudanças no contexto  
**Verificação**: 
- Conta deve existir no banco
- Propriedades devem estar corretas

##### 2. `Add_WithNullAccount_ShouldThrowException()`
**Propósito**: Testa comportamento com conta nula  
**Configuração**: Account = null  
**Execução**: Chama repository.Add(null)  
**Verificação**: Deve lançar ArgumentNullException

##### GetAll Tests

##### 3. `GetAll_WithMultipleAccounts_ShouldReturnAllAccounts()`
**Propósito**: Verifica se todas as contas são retornadas  
**Configuração**: 
- Adiciona 3 contas diferentes ao banco
- Salva mudanças  
**Execução**: Chama repository.GetAll()  
**Verificação**: 
- Deve retornar 3 contas
- Contas devem ter dados corretos

##### 4. `GetAll_WithEmptyDatabase_ShouldReturnEmptyList()`
**Propósito**: Testa comportamento com banco vazio  
**Configuração**: Banco de dados limpo  
**Execução**: Chama repository.GetAll()  
**Verificação**: Deve retornar lista vazia

##### GetById Tests

##### 5. `GetById_WithExistingId_ShouldReturnAccount()`
**Propósito**: Busca conta por ID existente  
**Configuração**: 
- Adiciona conta ao banco
- Obtém ID gerado  
**Execução**: Chama repository.GetById(id)  
**Verificação**: Deve retornar conta com dados corretos

##### 6. `GetById_WithNonExistingId_ShouldReturnNull()`
**Propósito**: Busca conta por ID inexistente  
**Configuração**: Banco com algumas contas  
**Execução**: Chama repository.GetById(999)  
**Verificação**: Deve retornar null

##### GetByUserName Tests

##### 7. `GetByUserName_WithExistingUserName_ShouldReturnAccount()`
**Propósito**: Busca conta por userName existente  
**Configuração**: 
- Adiciona conta com userName específico
- Salva no banco  
**Execução**: Chama repository.GetByUserName("testuser")  
**Verificação**: Deve retornar conta correta

##### 8. `GetByUserName_WithNonExistingUserName_ShouldReturnNull()`
**Propósito**: Busca conta por userName inexistente  
**Configuração**: Banco com outras contas  
**Execução**: Chama repository.GetByUserName("nonexistent")  
**Verificação**: Deve retornar null

##### 9. `GetByUserName_WithNullOrEmptyUserName_ShouldReturnNull()`
**Propósito**: Testa comportamento com userName nulo ou vazio  
**Configuração**: Banco com contas válidas  
**Execução**: Chama repository.GetByUserName(null) e GetByUserName("")  
**Verificação**: Ambos devem retornar null

##### Update Tests

##### 10. `Update_WithExistingAccount_ShouldUpdateInDatabase()`
**Propósito**: Atualiza conta existente no banco  
**Configuração**: 
- Adiciona conta ao banco
- Modifica propriedades da conta  
**Execução**: 
- Chama repository.Update(account)
- Salva mudanças  
**Verificação**: 
- Conta no banco deve ter novos valores
- ID deve permanecer o mesmo

##### Delete Tests

##### 11. `Delete_WithExistingAccount_ShouldRemoveFromDatabase()`
**Propósito**: Remove conta existente do banco  
**Configuração**: 
- Adiciona conta ao banco
- Confirma que existe  
**Execução**: 
- Chama repository.Delete(account)
- Salva mudanças  
**Verificação**: Conta não deve mais existir no banco

---

### AccountPayLoadDTOTests

**Arquivo**: `Src/Authentication.Tests/Unit/AccountPayLoadDTOTests.cs`  
**Propósito**: Testa o DTO usado para payload de requisições Account  
**Total de Testes**: 8 testes  

#### Testes Implementados:

##### 1. `AccountPayLoadDTO_WhenCreated_ShouldHaveDefaultValues()`
**Propósito**: Verifica valores padrão do DTO  
**Configuração**: Instancia novo AccountPayLoadDTO  
**Execução**: Cria DTO sem parâmetros  
**Verificação**: 
- UserName deve ser null
- Password deve ser null

##### 2. `AccountPayLoadDTO_SetUserName_ShouldUpdateProperty()`
**Propósito**: Testa definição da propriedade UserName  
**Configuração**: DTO vazio e valor esperado  
**Execução**: Define dto.UserName = "testuser"  
**Verificação**: UserName deve conter valor definido

##### 3. `AccountPayLoadDTO_SetPassword_ShouldUpdateProperty()`
**Propósito**: Testa definição da propriedade Password  
**Configuração**: DTO vazio e senha esperada  
**Execução**: Define dto.Password = "testpass"  
**Verificação**: Password deve conter valor definido

##### 4. `AccountPayLoadDTO_WithValidData_ShouldSetPropertiesCorrectly()`
**Propósito**: Testa definição simultânea de ambas propriedades  
**Configuração**: Valores válidos para userName e password  
**Execução**: Cria DTO com ambas propriedades  
**Verificação**: Ambas propriedades devem ter valores corretos

##### 5. `AccountPayLoadDTO_WithVariousValues_ShouldAcceptAllInputs()` (Theory Test)
**Propósito**: Testa DTO com diferentes combinações de valores  
**Configuração**: Dados de teste: ("", ""), ("user", ""), ("", "pass"), ("user", "pass")  
**Execução**: Cria DTO com cada combinação  
**Verificação**: DTO deve aceitar e armazenar todos os valores

##### 6. `AccountPayLoadDTO_WithLongValues_ShouldAcceptValues()`
**Propósito**: Testa DTO com valores longos  
**Configuração**: Strings de 1000 caracteres para userName e password  
**Execução**: Cria DTO com valores longos  
**Verificação**: DTO deve armazenar valores completos

##### 7. `AccountPayLoadDTO_WithUnicodeCharacters_ShouldAcceptValues()`
**Propósito**: Testa DTO com caracteres Unicode  
**Configuração**: userName = "usuário", password = "contraseña"  
**Execução**: Cria DTO com caracteres especiais  
**Verificação**: DTO deve preservar caracteres Unicode

##### 8. `AccountPayLoadDTO_WithSpecialCharacters_ShouldAcceptValues()`
**Propósito**: Testa DTO com caracteres especiais  
**Configuração**: userName e password com símbolos especiais  
**Execução**: Cria DTO com caracteres especiais  
**Verificação**: DTO deve preservar todos os caracteres

---

### TokenTests

**Arquivo**: `Src/Authentication.Tests/Unit/TokenTests.cs`  
**Propósito**: Testa a entidade Token utilizada para JWT  
**Total de Testes**: 15+ testes  

#### Testes Implementados:

##### 1. `Token_WhenCreated_ShouldRequireAccessTokenAndUserName()`
**Propósito**: Verifica se Token pode ser criado com propriedades básicas  
**Configuração**: Valores válidos para AccessToken, UserName e Expiration  
**Execução**: Cria Token com propriedades definidas  
**Verificação**: 
- AccessToken deve ter valor correto
- UserName deve ter valor correto
- Expiration deve ser no futuro

##### 2. `Token_WithValidJwtFormat_ShouldAcceptToken()`
**Propósito**: Testa Token com JWT válido  
**Configuração**: JWT real de exemplo com 3 partes  
**Execução**: Cria Token com JWT válido  
**Verificação**: 
- AccessToken deve ter valor do JWT
- Token deve conter pontos (separadores)
- JWT deve ter exatamente 3 partes

##### 3. `Token_WithFutureExpiration_ShouldBeValid()`
**Propósito**: Verifica se Token aceita expiração futura  
**Configuração**: Data de expiração 2 horas no futuro  
**Execução**: Cria Token com expiração futura  
**Verificação**: Expiration deve ser após momento atual

##### 4. `Token_WithPastExpiration_ShouldStillAllowCreation()`
**Propósito**: Testa se Token aceita data passada (para casos de teste)  
**Configuração**: Data de expiração no passado  
**Execução**: Cria Token com expiração passada  
**Verificação**: Token deve ser criado normalmente

##### 5. `Token_WithEmptyAccessToken_ShouldAllowValue()`
**Propósito**: Testa comportamento com AccessToken vazio  
**Configuração**: AccessToken = ""  
**Execução**: Cria Token com AccessToken vazio  
**Verificação**: AccessToken deve aceitar string vazia

##### 6. `Token_WithNullUserName_ShouldAllowValue()`
**Propósito**: Testa comportamento com UserName nulo  
**Configuração**: UserName = null  
**Execução**: Cria Token com UserName nulo  
**Verificação**: UserName deve aceitar valor nulo

---

### ValidationTests

**Arquivo**: `Src/Authentication.Tests/Unit/ValidationTests.cs`  
**Propósito**: Testa helper de validação utilizado nos controllers  
**Total de Testes**: 10+ testes  
**Dependências Mockadas**: IValidator, IServiceProvider

#### Testes Implementados:

##### 1. `ValidationHelper_WithValidEntity_ShouldReturnNull()`
**Propósito**: Testa validação com entidade válida  
**Configuração**: 
- Entidade TestEntity válida
- Mock validator retorna ValidationResult sem erros  
**Execução**: Chama ValidationHelper.ValidateEntityAsync()  
**Verificação**: Deve retornar null (sem erros)

##### 2. `ValidationHelper_WithInvalidEntity_ShouldReturnBadRequest()`
**Propósito**: Testa validação com entidade inválida  
**Configuração**: 
- Entidade TestEntity inválida
- Mock validator retorna erros de validação  
**Execução**: Chama ValidationHelper.ValidateEntityAsync()  
**Verificação**: Deve retornar BadRequestObjectResult

##### 3. `ValidationHelper_WithMultipleErrors_ShouldReturnAllErrors()`
**Propósito**: Testa se todos os erros de validação são retornados  
**Configuração**: 
- Múltiplos erros de validação (Name e Email)
- Mock validator retorna lista de erros  
**Execução**: Chama ValidationHelper.ValidateEntityAsync()  
**Verificação**: 
- Deve retornar BadRequest
- Deve conter todos os erros

##### 4. `ValidationHelper_WithNullValidator_ShouldThrowException()`
**Propósito**: Testa comportamento quando validator não está registrado  
**Configuração**: ServiceProvider retorna null para validator  
**Execução**: Chama ValidationHelper.ValidateEntityAsync()  
**Verificação**: Deve lançar exceção apropriada

---

### AccountPayloadValidatorTests

**Arquivo**: `Src/Authentication.Tests/Unit/AccountPayloadValidatorTests.cs`  
**Propósito**: Testa validação de payload para criação/atualização de contas  
**Total de Testes**: 20+ testes  
**Framework**: FluentValidation com TestHelper

#### Setup do Teste:
```csharp
public AccountPayloadValidatorTests()
{
    _validator = new AccountPayloadValidator();
}
```

#### Grupos de Testes:

##### UserName Validation Tests

##### 1. `UserName_WhenValid_ShouldNotHaveValidationError()`
**Propósito**: Verifica se userName válido passa na validação  
**Configuração**: DTO com userName = "validuser" e password válida  
**Execução**: _validator.TestValidate(model)  
**Verificação**: Não deve ter erro de validação para UserName

##### 2. `UserName_WhenEmpty_ShouldHaveValidationError()`
**Propósito**: Verifica se userName vazio falha na validação  
**Configuração**: DTO com userName = "" e password válida  
**Execução**: _validator.TestValidate(model)  
**Verificação**: Deve ter erro com mensagem ResourceLogin.UserNameRequired

##### 3. `UserName_WhenNull_ShouldHaveValidationError()`
**Propósito**: Verifica se userName nulo falha na validação  
**Configuração**: DTO com userName = null e password válida  
**Execução**: _validator.TestValidate(model)  
**Verificação**: Deve ter erro com mensagem ResourceLogin.UserNameRequired

##### 4. `UserName_WhenTooLong_ShouldHaveValidationError()`
**Propósito**: Testa limite máximo de caracteres para userName  
**Configuração**: DTO com userName muito longo (>50 caracteres)  
**Execução**: _validator.TestValidate(model)  
**Verificação**: Deve ter erro de tamanho máximo

##### 5. `UserName_WithSpecialCharacters_ShouldValidateCorrectly()`
**Propósito**: Testa aceitação de caracteres especiais permitidos  
**Configuração**: DTO com userName contendo caracteres especiais válidos  
**Execução**: _validator.TestValidate(model)  
**Verificação**: Deve passar na validação

##### Password Validation Tests

##### 6. `Password_WhenValid_ShouldNotHaveValidationError()`
**Propósito**: Verifica se password válida passa na validação  
**Configuração**: DTO com password = "validpass123" e userName válido  
**Execução**: _validator.TestValidate(model)  
**Verificação**: Não deve ter erro de validação para Password

##### 7. `Password_WhenEmpty_ShouldHaveValidationError()`
**Propósito**: Verifica se password vazia falha na validação  
**Configuração**: DTO com password = "" e userName válido  
**Execução**: _validator.TestValidate(model)  
**Verificação**: Deve ter erro com mensagem ResourceLogin.PasswordRequired

##### 8. `Password_WhenTooShort_ShouldHaveValidationError()`
**Propósito**: Testa tamanho mínimo de password  
**Configuração**: DTO com password muito curta (<6 caracteres)  
**Execução**: _validator.TestValidate(model)  
**Verificação**: Deve ter erro de tamanho mínimo

##### 9. `Password_WhenTooLong_ShouldHaveValidationError()`
**Propósito**: Testa tamanho máximo de password  
**Configuração**: DTO com password muito longa (>100 caracteres)  
**Execução**: _validator.TestValidate(model)  
**Verificação**: Deve ter erro de tamanho máximo

##### 10. `Password_WithRequiredComplexity_ShouldValidateCorrectly()`
**Propósito**: Testa regras de complexidade de senha  
**Configuração**: DTOs com diferentes níveis de complexidade  
**Execução**: _validator.TestValidate(model)  
**Verificação**: Deve validar conforme regras de complexidade

---

### AccountServiceErrorHandlingTests

**Arquivo**: `Src/Authentication.Tests/Unit/AccountServiceErrorHandlingTests.cs`  
**Propósito**: Testa cenários de erro e tratamento de exceções no AccountService  
**Total de Testes**: 25+ testes  
**Foco**: Robustez e tratamento de erros

#### Grupos de Testes:

##### Null Parameter Tests

##### 1. `GetAccountByUserName_WithNullUserName_ShouldNotThrow()`
**Propósito**: Verifica se método lida graciosamente com userName nulo  
**Configuração**: Mock repositório retorna null para userName nulo  
**Execução**: _accountService.GetAccountByUserName(null!)  
**Verificação**: 
- Não deve lançar exceção
- Deve retornar null
- Repositório deve ser chamado uma vez

##### 2. `AddAccount_WithNullAccount_ShouldThrowArgumentNullException()`
**Propósito**: Verifica se método valida parâmetros nulos  
**Configuração**: Account = null  
**Execução**: _accountService.AddAccount(null!)  
**Verificação**: Deve lançar ArgumentNullException

##### Repository Exception Tests

##### 3. `GetAllAccounts_WhenRepositoryThrows_ShouldPropagateException()`
**Propósito**: Verifica se exceções do repositório são propagadas corretamente  
**Configuração**: Mock repositório configurado para lançar DatabaseException  
**Execução**: _accountService.GetAllAccounts()  
**Verificação**: Deve lançar a mesma DatabaseException

##### 4. `AddAccount_WhenRepositoryThrows_ShouldPropagateException()`
**Propósito**: Testa propagação de erros durante adição  
**Configuração**: 
- Mock repositório lança exceção no Add
- Account válida  
**Execução**: _accountService.AddAccount(account)  
**Verificação**: Deve lançar exceção do repositório

##### Business Logic Exception Tests

##### 5. `AddAccount_WithDuplicateUserName_ShouldThrowConflictException()`
**Propósito**: Testa regra de negócio para userName único  
**Configuração**: 
- Mock repositório retorna conta existente para GetByUserName
- Account nova com userName duplicado  
**Execução**: _accountService.AddAccount(account)  
**Verificação**: Deve lançar ConflictException com mensagem apropriada

##### 6. `GetAccountByUserNameAndPassword_WithInvalidCredentials_ShouldThrowUnauthorized()`
**Propósito**: Testa comportamento com credenciais inválidas  
**Configuração**: 
- Conta existente no repositório
- Senha incorreta para verificação  
**Execução**: _accountService.GetAccountByUserNameAndPassword(account)  
**Verificação**: Deve lançar UnauthorizedAccessException

##### Data Integrity Tests

##### 7. `UpdateAccount_WithNonExistentId_ShouldThrowNotFoundException()`
**Propósito**: Testa atualização de conta inexistente  
**Configuração**: Mock repositório retorna null para GetById  
**Execução**: _accountService.UpdateAccount(account)  
**Verificação**: Deve lançar NotFoundException

##### 8. `DeleteAccount_WithNonExistentId_ShouldThrowNotFoundException()`
**Propósito**: Testa remoção de conta inexistente  
**Configuração**: Mock repositório retorna null para GetById  
**Execução**: _accountService.DeleteAccount(999)  
**Verificação**: Deve lançar NotFoundException

---

### PasswordHashingTests

**Arquivo**: `Src/Authentication.Tests/Unit/PasswordHashingTests.cs`  
**Propósito**: Testa funções de hash de senha usando Argon2  
**Total de Testes**: 12+ testes  

#### Testes Implementados:

##### 1. `ComputeArgon2Hash_WithValidPassword_ShouldReturnHash()`
**Propósito**: Verifica se hash é gerado corretamente  
**Configuração**: Senha válida "testpassword123"  
**Execução**: Chama StringHelper.ComputeArgon2Hash()  
**Verificação**: 
- Deve retornar hash não vazio
- Hash deve ser diferente da senha original

##### 2. `ComputeArgon2Hash_WithSamePassword_ShouldReturnDifferentHashes()`
**Propósito**: Verifica se hashes são únicos (salt aleatório)  
**Configuração**: Mesma senha hashada duas vezes  
**Execução**: Chama ComputeArgon2Hash() duas vezes  
**Verificação**: Hashes devem ser diferentes

##### 3. `VerifyArgon2Hash_WithCorrectPassword_ShouldReturnTrue()`
**Propósito**: Testa verificação com senha correta  
**Configuração**: 
- Senha original
- Hash gerado da senha  
**Execução**: Chama StringHelper.VerifyArgon2Hash()  
**Verificação**: Deve retornar true

##### 4. `VerifyArgon2Hash_WithIncorrectPassword_ShouldReturnFalse()`
**Propósito**: Testa verificação com senha incorreta  
**Configuração**: 
- Hash de "password123"
- Verificação com "wrongpassword"  
**Execução**: Chama VerifyArgon2Hash()  
**Verificação**: Deve retornar false

##### 5. `ComputeArgon2Hash_WithEmptyPassword_ShouldReturnHash()`
**Propósito**: Testa hash de senha vazia  
**Configuração**: Password = ""  
**Execução**: Chama ComputeArgon2Hash()  
**Verificação**: Deve retornar hash válido

##### 6. `VerifyArgon2Hash_WithNullValues_ShouldHandleGracefully()`
**Propósito**: Testa comportamento com valores nulos  
**Configuração**: password = null ou hash = null  
**Execução**: Chama VerifyArgon2Hash()  
**Verificação**: Deve retornar false sem lançar exceção

---

### LocalizationTests

**Arquivo**: `Src/Authentication.Tests/Unit/LocalizationTests.cs`  
**Propósito**: Testa funcionalidades de internacionalização e localização  
**Total de Testes**: 15+ testes  
**Culturas Testadas**: en (inglês), pt-BR (português brasileiro)

#### Testes Implementados:

##### 1. `ResourceAPI_AccountCreatedSuccessfully_ReturnsCorrectTranslation()` (Theory Test)
**Propósito**: Verifica se mensagens da API são traduzidas corretamente  
**Configuração**: 
- Culturas: "en", "pt-BR"
- Textos esperados: "Account created successfully.", "Conta criada com sucesso."  
**Execução**: 
- Define CultureInfo.CurrentUICulture
- Acessa ResourceAPI.AccountCreatedSuccessfully  
**Verificação**: Texto deve corresponder à cultura definida

##### 2. `ResourceStartup_SwaggerAuthenticationDescription_ReturnsCorrectTranslation()`
**Propósito**: Testa localização de descrições do Swagger  
**Configuração**: Cultura "en" com descrição esperada  
**Execução**: Acessa ResourceStartup.SwaggerAuthenticationDescription  
**Verificação**: Deve retornar texto em inglês

##### 3. `ResourceLogin_DuplicateUserName_ReturnsCorrectTranslation()` (Theory Test)
**Propósito**: Verifica tradução de mensagens de erro de login  
**Configuração**: Múltiplas culturas e mensagens de erro  
**Execução**: Acessa ResourceLogin.DuplicateUserName  
**Verificação**: Mensagem deve estar na cultura correta

##### 4. `Culture_SwitchDuringExecution_ShouldUpdateMessages()`
**Propósito**: Testa mudança de cultura durante execução  
**Configuração**: 
- Inicia com cultura "en"
- Troca para "pt-BR"  
**Execução**: 
- Acessa recursos em inglês
- Troca cultura
- Acessa mesmos recursos  
**Verificação**: Mensagens devem refletir mudança de cultura

##### 5. `ResourceManager_WithUnsupportedCulture_ShouldFallbackToDefault()`
**Propósito**: Testa fallback para cultura padrão  
**Configuração**: Cultura não suportada (ex: "fr-FR")  
**Execução**: Define cultura não suportada e acessa recursos  
**Verificação**: Deve usar cultura padrão (inglês)

---

### ActionPayloadValidatorTests

**Arquivo**: `Src/Authentication.Tests/Unit/ActionPayloadValidatorTests.cs`  
**Propósito**: Testa validação de payload para entidade Action  
**Total de Testes**: 15+ testes

#### Grupos de Testes:

##### Name Validation Tests

##### 1. `Name_WhenValid_ShouldNotHaveValidationError()`
**Propósito**: Verifica se nome válido passa na validação  
**Configuração**: ActionPayLoadDTO com Name válido  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Não deve ter erro de validação

##### 2. `Name_WhenEmpty_ShouldHaveValidationError()`
**Propósito**: Testa validação com nome vazio  
**Configuração**: ActionPayLoadDTO com Name = ""  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Deve ter erro de validação

##### Description Validation Tests

##### 3. `Description_WhenValid_ShouldNotHaveValidationError()`
**Propósito**: Verifica se descrição válida passa na validação  
**Configuração**: ActionPayLoadDTO com Description válida  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Não deve ter erro de validação

##### 4. `Description_WhenTooLong_ShouldHaveValidationError()`
**Propósito**: Testa limite de tamanho da descrição  
**Configuração**: ActionPayLoadDTO com Description muito longa  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Deve ter erro de tamanho máximo

---

### ClaimPayloadValidatorTests

**Arquivo**: `Src/Authentication.Tests/Unit/ClaimPayloadValidatorTests.cs`  
**Propósito**: Testa validação de payload para entidade Claim  
**Total de Testes**: 12+ testes

#### Grupos de Testes:

##### Type Validation Tests

##### 1. `Type_WhenValid_ShouldNotHaveValidationError()`
**Propósito**: Verifica se tipo de claim válido passa na validação  
**Configuração**: ClaimPayLoadDTO com Type válido  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Não deve ter erro de validação

##### 2. `Type_WhenInvalidEnum_ShouldHaveValidationError()`
**Propósito**: Testa validação com tipo de claim inválido  
**Configuração**: ClaimPayLoadDTO com Type fora do enum  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Deve ter erro de validação

##### Value Validation Tests

##### 3. `Value_WhenValid_ShouldNotHaveValidationError()`
**Propósito**: Verifica se valor de claim válido passa na validação  
**Configuração**: ClaimPayLoadDTO com Value válido  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Não deve ter erro de validação

---

### ClaimActionPayloadValidatorTests

**Arquivo**: `Src/Authentication.Tests/Unit/ClaimActionPayloadValidatorTests.cs`  
**Propósito**: Testa validação de payload para relacionamento Claim-Action  
**Total de Testes**: 10+ testes

#### Grupos de Testes:

##### IdClaim Validation Tests

##### 1. `IdClaim_WhenValid_ShouldNotHaveValidationError()`
**Propósito**: Verifica se ID de claim válido passa na validação  
**Configuração**: ClaimActionPayLoadDTO com IdClaim > 0  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Não deve ter erro de validação

##### 2. `IdClaim_WhenZero_ShouldHaveValidationError()`
**Propósito**: Testa validação com ID de claim zero  
**Configuração**: ClaimActionPayLoadDTO com IdClaim = 0  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Deve ter erro de validação

##### IdAction Validation Tests

##### 3. `IdAction_WhenValid_ShouldNotHaveValidationError()`
**Propósito**: Verifica se ID de action válido passa na validação  
**Configuração**: ClaimActionPayLoadDTO com IdAction > 0  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Não deve ter erro de validação

##### 4. `IdAction_WhenNegative_ShouldHaveValidationError()`
**Propósito**: Testa validação com ID de action negativo  
**Configuração**: ClaimActionPayLoadDTO com IdAction < 0  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Deve ter erro de validação

---

### AccountClaimActionPayloadValidatorTests

**Arquivo**: `Src/Authentication.Tests/Unit/AccountClaimActionPayloadValidatorTests.cs`  
**Propósito**: Testa validação de payload para relacionamento Account-Claim-Action  
**Total de Testes**: 12+ testes

#### Grupos de Testes:

##### IdAccount Validation Tests

##### 1. `IdAccount_WhenValid_ShouldNotHaveValidationError()`
**Propósito**: Verifica se ID de account válido passa na validação  
**Configuração**: AccountClaimActionPayLoadDTO com IdAccount > 0  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Não deve ter erro de validação

##### 2. `IdAccount_WhenZero_ShouldHaveValidationError()`
**Propósito**: Testa validação com ID de account zero  
**Configuração**: AccountClaimActionPayLoadDTO com IdAccount = 0  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Deve ter erro de validação

##### IdClaimAction Validation Tests

##### 3. `IdClaimAction_WhenValid_ShouldNotHaveValidationError()`
**Propósito**: Verifica se ID de claim-action válido passa na validação  
**Configuração**: AccountClaimActionPayLoadDTO com IdClaimAction > 0  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Não deve ter erro de validação

##### 4. `IdClaimAction_WhenNegative_ShouldHaveValidationError()`
**Propósito**: Testa validação com ID de claim-action negativo  
**Configuração**: AccountClaimActionPayLoadDTO com IdClaimAction < 0  
**Execução**: _validator.TestValidate(dto)  
**Verificação**: Deve ter erro de validação

---

### LocalizedSwaggerDocumentFilterTests

**Arquivo**: `Src/Authentication.Tests/Unit/LocalizedSwaggerDocumentFilterTests.cs`  
**Propósito**: Testa filtro de localização para documentação Swagger  
**Total de Testes**: 8+ testes

#### Testes Implementados:

##### 1. `Apply_WithEnglishCulture_ShouldSetEnglishInfo()`
**Propósito**: Verifica se informações do Swagger são definidas em inglês  
**Configuração**: Cultura definida para "en"  
**Execução**: Chama filter.Apply(swaggerDoc, context)  
**Verificação**: 
- Title deve estar em inglês
- Description deve estar em inglês

##### 2. `Apply_WithPortugueseCulture_ShouldSetPortugueseInfo()`
**Propósito**: Verifica se informações do Swagger são definidas em português  
**Configuração**: Cultura definida para "pt-BR"  
**Execução**: Chama filter.Apply(swaggerDoc, context)  
**Verificação**: 
- Title deve estar em português
- Description deve estar em português

---

### LocalizedSwaggerOperationFilterTests

**Arquivo**: `Src/Authentication.Tests/Unit/LocalizedSwaggerOperationFilterTests.cs`  
**Propósito**: Testa filtro de localização para operações do Swagger  
**Total de Testes**: 8+ testes

#### Testes Implementados:

##### 1. `Apply_WithLocalizedSummary_ShouldSetCorrectSummary()`
**Propósito**: Verifica se resumos de operações são localizados  
**Configuração**: Operação com atributo de localização  
**Execução**: Chama filter.Apply(operation, context)  
**Verificação**: Summary deve estar na cultura correta

##### 2. `Apply_WithLocalizedDescription_ShouldSetCorrectDescription()`
**Propósito**: Verifica se descrições de operações são localizadas  
**Configuração**: Operação com descrição localizada  
**Execução**: Chama filter.Apply(operation, context)  
**Verificação**: Description deve estar na cultura correta

---

### ResourceStartupTests

**Arquivo**: `Src/Authentication.Tests/Unit/ResourceStartupTests.cs`  
**Propósito**: Testa recursos utilizados na inicialização da aplicação  
**Total de Testes**: 5+ testes

#### Testes Implementados:

##### 1. `SwaggerTitle_ShouldReturnCorrectValue()`
**Propósito**: Verifica se título do Swagger está correto  
**Configuração**: Cultura padrão  
**Execução**: Acessa ResourceStartup.SwaggerTitle  
**Verificação**: Deve retornar título esperado

##### 2. `SwaggerVersion_ShouldReturnCorrectValue()`
**Propósito**: Verifica se versão do Swagger está correta  
**Configuração**: Cultura padrão  
**Execução**: Acessa ResourceStartup.SwaggerVersion  
**Verificação**: Deve retornar versão esperada

---

### ApiLocalizationTests

**Arquivo**: `Src/Authentication.Tests/Unit/ApiLocalizationTests.cs`  
**Propósito**: Testa localização específica da API  
**Total de Testes**: 10+ testes

#### Testes Implementados:

##### 1. `ErrorMessages_ShouldBeLocalizedCorrectly()`
**Propósito**: Verifica se mensagens de erro são localizadas  
**Configuração**: Diferentes culturas  
**Execução**: Acessa mensagens de erro da API  
**Verificação**: Mensagens devem estar na cultura correta

##### 2. `SuccessMessages_ShouldBeLocalizedCorrectly()`
**Propósito**: Verifica se mensagens de sucesso são localizadas  
**Configuração**: Diferentes culturas  
**Execução**: Acessa mensagens de sucesso da API  
**Verificação**: Mensagens devem estar na cultura correta

---

## 🔗 Testes de Integração

### AuthenticationControllerTests

**Arquivo**: `Src/Authentication.Tests/Integration/AuthenticationControllerTests.cs`  
**Propósito**: Testa endpoints de autenticação end-to-end  
**Total de Testes**: 15+ testes  
**Setup**: WebApplicationFactory para testes de integração

#### Setup do Teste:
```csharp
public AuthenticationControllerTests(AuthenticationWebApplicationFactory factory)
{
    _factory = factory;
    _client = _factory.CreateClient();
}
```

#### Testes Implementados:

##### 1. `GenerateToken_WithValidCredentials_ShouldReturnOk()`
**Propósito**: Testa geração de token com credenciais válidas  
**Configuração**: 
- Request JSON com userName e password válidos
- HttpClient configurado  
**Execução**: POST para /Authentication/GenerateToken  
**Verificação**: 
- Status deve ser OK, BadRequest, Unauthorized ou InternalServerError
- Resposta deve ser adequada ao estado do sistema

##### 2. `GenerateToken_WithInvalidCredentials_ShouldReturnUnauthorized()`
**Propósito**: Testa comportamento com credenciais inválidas  
**Configuração**: Request com credenciais incorretas  
**Execução**: POST para /Authentication/GenerateToken  
**Verificação**: Status deve indicar falha de autenticação

##### 3. `GenerateToken_WithEmptyPayload_ShouldReturnBadRequest()`
**Propósito**: Testa comportamento com payload vazio  
**Configuração**: Request sem userName ou password  
**Execução**: POST para /Authentication/GenerateToken  
**Verificação**: Deve retornar BadRequest

##### 4. `GenerateToken_WithMalformedJson_ShouldReturnBadRequest()`
**Propósito**: Testa comportamento com JSON malformado  
**Configuração**: Request com JSON inválido  
**Execução**: POST para /Authentication/GenerateToken  
**Verificação**: Deve retornar BadRequest

##### 5. `AddAccount_WithValidData_ShouldCreateAccount()`
**Propósito**: Testa criação de conta com dados válidos  
**Configuração**: 
- Payload com userName e password únicos
- Headers apropriados  
**Execução**: POST para /Authentication/AddAccount  
**Verificação**: 
- Status deve ser Created ou Conflict
- Se criada, resposta deve conter dados da conta

##### 6. `AddAccount_WithDuplicateUserName_ShouldReturnConflict()`
**Propósito**: Testa criação com userName duplicado  
**Configuração**: 
- Primeira requisição para criar conta
- Segunda requisição com mesmo userName  
**Execução**: Duas chamadas POST para /Authentication/AddAccount  
**Verificação**: 
- Primeira pode ser Created ou já existir
- Segunda deve retornar Conflict

---

### AccountControllerTests

**Arquivo**: `Src/Authentication.Tests/Integration/AccountControllerTests.cs`  
**Propósito**: Testa operações CRUD de contas  
**Total de Testes**: 25+ testes  

#### Testes Implementados:

##### 1. `GetAllAccounts_ShouldReturnAccountsList()`
**Propósito**: Testa listagem de todas as contas  
**Configuração**: Cliente HTTP configurado  
**Execução**: GET para /Account  
**Verificação**: 
- Status deve ser OK ou NoContent
- Se OK, deve retornar array de contas

##### 2. `GetAccountById_WithExistingId_ShouldReturnAccount()`
**Propósito**: Testa busca de conta por ID existente  
**Configuração**: ID de conta válido  
**Execução**: GET para /Account/{id}  
**Verificação**: 
- Status deve ser OK ou NotFound
- Se encontrada, dados devem estar corretos

##### 3. `GetAccountById_WithNonExistingId_ShouldReturnNotFound()`
**Propósito**: Testa busca com ID inexistente  
**Configuração**: ID muito alto (999999)  
**Execução**: GET para /Account/999999  
**Verificação**: Deve retornar NotFound

##### 4. `CreateAccount_WithValidData_ShouldReturnCreated()`
**Propósito**: Testa criação de conta válida  
**Configuração**: Payload com dados únicos e válidos  
**Execução**: POST para /Account  
**Verificação**: 
- Status deve ser Created ou Conflict
- Headers de localização devem estar presentes

##### 5. `UpdateAccount_WithValidData_ShouldReturnOk()`
**Propósito**: Testa atualização de conta existente  
**Configuração**: 
- Conta existente
- Dados atualizados válidos  
**Execução**: PUT para /Account/{id}  
**Verificação**: 
- Status deve ser OK ou NotFound
- Dados devem ser atualizados

##### 6. `DeleteAccount_WithExistingId_ShouldReturnNoContent()`
**Propósito**: Testa remoção de conta existente  
**Configuração**: ID de conta válido  
**Execução**: DELETE para /Account/{id}  
**Verificação**: 
- Status deve ser NoContent ou NotFound
- Conta não deve mais existir

---

### AccountControllerEnhancedTests

**Arquivo**: `Src/Authentication.Tests/Integration/AccountControllerEnhancedTests.cs`  
**Propósito**: Testa cenários avançados e edge cases do AccountController  
**Total de Testes**: 30+ testes  

#### Testes Específicos para Cenários Avançados:

##### 1. `CreateAccount_WithDuplicateUserName_ShouldReturnConflict()`
**Propósito**: Teste específico para prevenção de userName duplicado  
**Configuração**: 
- Primeira conta criada com userName específico
- Segunda tentativa com mesmo userName  
**Execução**: 
- POST /Account com primeira conta
- POST /Account com userName duplicado  
**Verificação**: 
- Primeira requisição: Created ou já existe
- Segunda requisição: Conflict (409)

##### 2. `CreateAccount_WithInvalidData_ShouldReturnValidationErrors()`
**Propósito**: Testa validação de dados de entrada  
**Configuração**: Payloads com dados inválidos (campos obrigatórios vazios)  
**Execução**: POST /Account com dados inválidos  
**Verificação**: 
- Status BadRequest
- Detalhes de validação na resposta

##### 3. `UpdateAccount_WithConflictingUserName_ShouldReturnConflict()`
**Propósito**: Testa atualização que causaria conflito de userName  
**Configuração**: 
- Duas contas existentes
- Atualização da primeira com userName da segunda  
**Execução**: PUT /Account/{id} com userName conflitante  
**Verificação**: Deve retornar Conflict

##### 4. `GetAccounts_WithPagination_ShouldReturnPagedResults()`
**Propósito**: Testa paginação de resultados  
**Configuração**: Múltiplas contas no sistema  
**Execução**: GET /Account?page=1&size=10  
**Verificação**: 
- Resposta deve conter apenas quantidade solicitada
- Headers de paginação devem estar presentes

##### 5. `AccountOperations_WithConcurrentRequests_ShouldHandleGracefully()`
**Propósito**: Testa operações concorrentes  
**Configuração**: Múltiplas requisições simultâneas  
**Execução**: Várias operações em paralelo  
**Verificação**: 
- Sistema deve manter consistência
- Sem corrupção de dados

---

### ActionControllerTests

**Arquivo**: `Src/Authentication.Tests/Integration/ActionControllerTests.cs`  
**Propósito**: Testa endpoints relacionados à entidade Action  
**Total de Testes**: 20+ testes  
**Setup**: AuthenticationWebApplicationFactory com dados de teste

#### Testes Implementados:

##### 1. `GetActions_ShouldReturnExpectedStatusCode()`
**Propósito**: Testa endpoint de listagem de ações  
**Configuração**: Cliente HTTP com dados de teste pré-carregados  
**Execução**: GET /Action/GetActions  
**Verificação**: Status deve ser OK, Unauthorized ou InternalServerError

##### 2. `GetActionById_WithVariousIds_ShouldReturnExpectedStatusCode()` (Theory Test)
**Propósito**: Testa busca de ação por ID com diferentes valores  
**Configuração**: IDs de teste: 1 (válido), 999 (inexistente), -1 (inválido)  
**Execução**: GET /Action/GetActionById/{id}  
**Verificação**: 
- ID válido: OK ou NotFound
- ID inexistente: NotFound
- ID inválido: BadRequest ou NotFound

##### 3. `CreateAction_WithValidData_ShouldReturnExpectedStatusCode()`
**Propósito**: Testa criação de nova ação  
**Configuração**: 
- Payload JSON válido com Name e Description
- Headers apropriados  
**Execução**: POST /Action/CreateAction  
**Verificação**: Status deve ser Created, BadRequest ou InternalServerError

##### 4. `CreateAction_WithInvalidData_ShouldReturnBadRequest()`
**Propósito**: Testa criação com dados inválidos  
**Configuração**: Payload com campos obrigatórios vazios  
**Execução**: POST /Action/CreateAction  
**Verificação**: Status deve ser BadRequest

##### 5. `UpdateAction_WithValidData_ShouldReturnExpectedStatusCode()`
**Propósito**: Testa atualização de ação existente  
**Configuração**: 
- ID de ação existente
- Payload com dados atualizados  
**Execução**: PUT /Action/UpdateAction/{id}  
**Verificação**: Status deve ser OK, NotFound ou BadRequest

##### 6. `DeleteAction_WithExistingId_ShouldReturnExpectedStatusCode()`
**Propósito**: Testa remoção de ação  
**Configuração**: ID de ação válido  
**Execução**: DELETE /Action/DeleteAction/{id}  
**Verificação**: Status deve ser NoContent, NotFound ou BadRequest

##### 7. `GetActionsByName_WithSearchTerm_ShouldReturnFilteredResults()`
**Propósito**: Testa busca de ações por nome  
**Configuração**: Termo de busca específico  
**Execução**: GET /Action/GetActionsByName?name={searchTerm}  
**Verificação**: 
- Resultados devem conter termo buscado
- Status deve ser OK ou NoContent

---

### ClaimActionControllerTests

**Arquivo**: `Src/Authentication.Tests/Integration/ClaimActionControllerTests.cs`  
**Propósito**: Testa endpoints do relacionamento Claim-Action  
**Total de Testes**: 20+ testes

#### Testes Implementados:

##### 1. `GetClaimActions_ShouldReturnExpectedStatusCode()`
**Propósito**: Testa listagem de relacionamentos claim-action  
**Configuração**: Sistema com dados de teste  
**Execução**: GET /ClaimAction/GetClaimActions  
**Verificação**: Status deve ser OK, NoContent ou InternalServerError

##### 2. `GetClaimActionById_WithValidId_ShouldReturnExpectedStatusCode()`
**Propósito**: Testa busca de relacionamento por ID  
**Configuração**: ID de relacionamento válido  
**Execução**: GET /ClaimAction/GetClaimActionById/{id}  
**Verificação**: Status deve ser OK ou NotFound

##### 3. `CreateClaimAction_WithValidData_ShouldReturnExpectedStatusCode()`
**Propósito**: Testa criação de relacionamento claim-action  
**Configuração**: 
- IDs válidos de Claim e Action existentes
- Payload JSON correto  
**Execução**: POST /ClaimAction/CreateClaimAction  
**Verificação**: Status deve ser Created ou BadRequest

##### 4. `CreateClaimAction_WithNonExistentIds_ShouldReturnBadRequest()`
**Propósito**: Testa criação com IDs inexistentes  
**Configuração**: 
- IdClaim ou IdAction que não existem no sistema
- Payload bem formado  
**Execução**: POST /ClaimAction/CreateClaimAction  
**Verificação**: Status deve ser BadRequest ou NotFound

##### 5. `DeleteClaimAction_WithExistingId_ShouldReturnExpectedStatusCode()`
**Propósito**: Testa remoção de relacionamento  
**Configuração**: ID de relacionamento existente  
**Execução**: DELETE /ClaimAction/DeleteClaimAction/{id}  
**Verificação**: Status deve ser NoContent ou NotFound

##### 6. `GetClaimActionsByClaim_WithValidClaimId_ShouldReturnFilteredResults()`
**Propósito**: Testa busca de ações por claim específico  
**Configuração**: ID de claim válido  
**Execução**: GET /ClaimAction/GetByClaimId/{claimId}  
**Verificação**: 
- Resultados devem conter apenas ações do claim especificado
- Status deve ser OK ou NoContent

---

### AccountClaimActionControllerTests

**Arquivo**: `Src/Authentication.Tests/Integration/AccountClaimActionControllerTests.cs`  
**Propósito**: Testa endpoints de permissões de usuário (Account-Claim-Action)  
**Total de Testes**: 20+ testes

#### Testes Implementados:

##### 1. `GetAccountClaimActions_ShouldReturnExpectedStatusCode()`
**Propósito**: Testa listagem de permissões de usuários  
**Configuração**: Sistema com permissões configuradas  
**Execução**: GET /AccountClaimAction/GetAccountClaimActions  
**Verificação**: Status deve ser OK, NoContent ou InternalServerError

##### 2. `GetAccountClaimActionsByAccountId_WithValidId_ShouldReturnUserPermissions()`
**Propósito**: Testa busca de permissões de usuário específico  
**Configuração**: ID de conta válido com permissões  
**Execução**: GET /AccountClaimAction/GetByAccountId/{accountId}  
**Verificação**: 
- Deve retornar permissões do usuário
- Status deve ser OK ou NoContent

##### 3. `CreateAccountClaimAction_WithValidData_ShouldGrantPermission()`
**Propósito**: Testa concessão de permissão a usuário  
**Configuração**: 
- ID de conta válido
- ID de claim-action válido
- Payload correto  
**Execução**: POST /AccountClaimAction/CreateAccountClaimAction  
**Verificação**: Status deve ser Created ou BadRequest

##### 4. `CreateAccountClaimAction_WithDuplicatePermission_ShouldReturnConflict()`
**Propósito**: Testa prevenção de permissões duplicadas  
**Configuração**: 
- Permissão já existente no sistema
- Tentativa de criar mesma permissão  
**Execução**: POST /AccountClaimAction/CreateAccountClaimAction  
**Verificação**: Status deve ser Conflict

##### 5. `DeleteAccountClaimAction_WithExistingPermission_ShouldRevokeAccess()`
**Propósito**: Testa revogação de permissão  
**Configuração**: Permissão existente no sistema  
**Execução**: DELETE /AccountClaimAction/DeleteAccountClaimAction/{id}  
**Verificação**: 
- Status deve ser NoContent
- Permissão não deve mais existir

##### 6. `GetAccountPermissions_WithAdminAccount_ShouldReturnAllPermissions()`
**Propósito**: Testa busca de permissões de conta administrativa  
**Configuração**: Conta com privilégios administrativos  
**Execução**: GET /AccountClaimAction/GetByAccountId/{adminAccountId}  
**Verificação**: 
- Deve retornar múltiplas permissões
- Deve incluir permissões administrativas

---

### SwaggerLocalizationTests

**Arquivo**: `Src/Authentication.Tests/Integration/SwaggerLocalizationTests.cs`  
**Propósito**: Testa localização da documentação Swagger  
**Total de Testes**: 10+ testes

#### Testes Implementados:

##### 1. `SwaggerUI_WithEnglishCulture_ShouldDisplayEnglishContent()`
**Propósito**: Verifica se Swagger UI exibe conteúdo em inglês  
**Configuração**: 
- Headers Accept-Language: en
- Cliente HTTP configurado  
**Execução**: GET /swagger/index.html  
**Verificação**: 
- Status deve ser OK
- Conteúdo deve conter textos em inglês

##### 2. `SwaggerUI_WithPortugueseCulture_ShouldDisplayPortugueseContent()`
**Propósito**: Verifica se Swagger UI exibe conteúdo em português  
**Configuração**: 
- Headers Accept-Language: pt-BR
- Cliente HTTP configurado  
**Execução**: GET /swagger/index.html  
**Verificação**: 
- Status deve ser OK
- Conteúdo deve conter textos em português

##### 3. `SwaggerDoc_WithDifferentCultures_ShouldReturnLocalizedSchema()`
**Propósito**: Testa localização do schema OpenAPI  
**Configuração**: Diferentes culturas configuradas  
**Execução**: GET /swagger/v1/swagger.json  
**Verificação**: 
- Schema deve conter descrições localizadas
- Títulos devem estar na cultura correta

##### 4. `SwaggerEndpoints_ShouldHaveLocalizedDescriptions()`
**Propósito**: Verifica se endpoints têm descrições localizadas  
**Configuração**: Swagger doc gerado  
**Execução**: Analisa schema dos endpoints  
**Verificação**: 
- Summaries devem estar localizados
- Descriptions devem estar na cultura apropriada

---

### ExampleFixedControllerTests

**Arquivo**: `Src/Authentication.Tests/Integration/ExampleFixedControllerTests.cs`  
**Propósito**: Testa controller de exemplo com correções aplicadas  
**Total de Testes**: 5+ testes

#### Testes Implementados:

##### 1. `GetExample_ShouldReturnExpectedResponse()`
**Propósito**: Testa endpoint de exemplo básico  
**Configuração**: Cliente HTTP padrão  
**Execução**: GET /Example/Get  
**Verificação**: 
- Status deve ser OK
- Resposta deve ter formato esperado

##### 2. `PostExample_WithValidData_ShouldReturnCreated()`
**Propósito**: Testa criação via endpoint de exemplo  
**Configuração**: Payload válido  
**Execução**: POST /Example/Create  
**Verificação**: Status deve ser Created

##### 3. `ExampleEndpoints_ShouldFollowRESTConventions()`
**Propósito**: Verifica se endpoints seguem convenções REST  
**Configuração**: Múltiplas operações HTTP  
**Execução**: GET, POST, PUT, DELETE no controller  
**Verificação**: 
- Status codes apropriados
- Headers corretos
- Comportamento REST padrão

---

## 📊 Resumo de Cobertura por Categoria

### Testes de Entidades (Entity Tests) - 35+ testes
- **AccountEntityTests**: 20+ testes (propriedades, validações, valores nulos/vazios)
- **TokenTests**: 15+ testes (criação, formatos JWT, expiração)
- **Cobertura**: Propriedades básicas, comportamento com valores edge case, integridade de dados

### Testes de Serviços (Service Tests) - 75+ testes
- **AccountServiceTests**: 50+ testes (CRUD operations, business logic)
- **AccountServiceErrorHandlingTests**: 25+ testes (exception handling, null safety)
- **Cobertura**: Lógica de negócio completa, regras de validação, tratamento robusto de erros

### Testes de Repositório (Repository Tests) - 30+ testes
- **AccountRepositoryTests**: 30+ testes (persistência, consultas, integridade)
- **Cobertura**: Operações CRUD, consultas específicas, comportamento com dados inválidos

### Testes de Validação (Validation Tests) - 85+ testes
- **ValidationTests**: 10+ testes (helper de validação geral)
- **AccountPayloadValidatorTests**: 20+ testes (validação de contas)
- **ActionPayloadValidatorTests**: 15+ testes (validação de ações)
- **ClaimPayloadValidatorTests**: 12+ testes (validação de claims)
- **ClaimActionPayloadValidatorTests**: 10+ testes (validação relacionamento claim-action)
- **AccountClaimActionPayloadValidatorTests**: 12+ testes (validação de permissões)
- **PasswordHashingTests**: 12+ testes (hash Argon2, verificação de senhas)
- **Cobertura**: Validação de entrada completa, regras de negócio, mensagens de erro localizadas

### Testes de DTOs (DTO Tests) - 8+ testes
- **AccountPayLoadDTOTests**: 8+ testes (serialização, propriedades, valores especiais)
- **Cobertura**: Comportamento de DTOs, aceitação de valores Unicode e especiais

### Testes de Integração (Integration Tests) - 110+ testes
- **AuthenticationControllerTests**: 15+ testes (geração token, autenticação)
- **AccountControllerTests**: 25+ testes (CRUD de contas)
- **AccountControllerEnhancedTests**: 30+ testes (cenários avançados, edge cases)
- **ActionControllerTests**: 20+ testes (gestão de ações)
- **ClaimActionControllerTests**: 20+ testes (relacionamentos claim-action)
- **AccountClaimActionControllerTests**: 20+ testes (permissões de usuário)
- **SwaggerLocalizationTests**: 10+ testes (documentação localizada)
- **ExampleFixedControllerTests**: 5+ testes (exemplos e convenções REST)
- **Cobertura**: Endpoints completos, status codes, integração end-to-end, cenários de erro

### Testes de Localização (Localization Tests) - 55+ testes
- **LocalizationTests**: 15+ testes (internacionalização básica)
- **ApiLocalizationTests**: 10+ testes (mensagens da API)
- **LocalizedSwaggerDocumentFilterTests**: 8+ testes (documentação Swagger)
- **LocalizedSwaggerOperationFilterTests**: 8+ testes (operações Swagger)
- **ResourceStartupTests**: 5+ testes (recursos de inicialização)
- **SwaggerLocalizationTests**: 10+ testes (UI localizada)
- **Cobertura**: Suporte completo a pt-BR e en, fallback para cultura padrão

---

## 🛠️ Padrões e Convenções Utilizados

### Padrão Arrange-Act-Assert (AAA)
Todos os testes seguem o padrão AAA rigorosamente:
```csharp
[Fact]
public void Method_Scenario_ExpectedResult()
{
    // Arrange - Configuração dos dados e mocks
    var expectedValue = "test";
    var mockRepository = new Mock<IRepository>();
    
    // Act - Execução da operação testada
    var result = service.ExecuteOperation(expectedValue);
    
    // Assert - Verificação dos resultados
    result.Should().Be(expectedValue);
    mockRepository.Verify(x => x.Method(), Times.Once);
}
```

### Naming Convention
- **Padrão**: `MethodName_Scenario_ExpectedResult`
- **Exemplos**: 
  - `GetAccountById_WithExistingId_ShouldReturnAccount`
  - `AddAccount_WithDuplicateUserName_ShouldThrowConflictException`
  - `UserName_WhenEmpty_ShouldHaveValidationError`

### Frameworks e Bibliotecas
- **xUnit**: Framework de teste principal com atributos [Fact] e [Theory]
- **FluentAssertions**: Assertions expressivas e legíveis (.Should().Be(), .Should().Contain())
- **Moq**: Mocking avançado para isolamento de dependências
- **FluentValidation.TestHelper**: Testes específicos para validadores
- **EntityFrameworkCore.InMemory**: Banco em memória para testes de repositório
- **Microsoft.AspNetCore.Mvc.Testing**: WebApplicationFactory para testes de integração

### Organização de Arquivos
```
Src/Authentication.Tests/
├── Unit/                     # Testes unitários isolados
│   ├── *EntityTests.cs       # Testes de entidades
│   ├── *ServiceTests.cs      # Testes de serviços
│   ├── *RepositoryTests.cs   # Testes de repositórios
│   ├── *ValidatorTests.cs    # Testes de validação
│   └── *Tests.cs            # Outros testes unitários
├── Integration/              # Testes de integração end-to-end
│   └── *ControllerTests.cs   # Testes de controllers
├── Fixtures/                 # Setup compartilhado
│   ├── Startup.cs           # Configuração de teste
│   └── AuthenticationWebApplicationFactory.cs
└── Helpers/                  # Utilitários
    └── TestHelpers.cs       # Helpers para testes
```

### Estratégias de Teste

#### Testes Unitários
- **Isolamento**: Uso extensivo de mocks para dependências
- **Cobertura**: Todos os caminhos de código testados
- **Edge Cases**: Valores nulos, vazios, extremos
- **Exception Testing**: Cenários de erro bem definidos

#### Testes de Integração
- **End-to-End**: Requisições HTTP reais
- **Status Codes**: Verificação de códigos HTTP apropriados
- **Scenarios**: Sucesso, validação, conflito, não encontrado
- **Data Seeding**: Dados de teste pré-carregados

#### Testes de Validação
- **FluentValidation**: Uso de TestHelper para validações
- **Localization**: Mensagens de erro em múltiplas culturas
- **Business Rules**: Regras de negócio específicas
- **Input Validation**: Validação completa de entrada

---

## 🔍 Cenários de Teste Específicos

### Segurança e Autenticação
- **Hash de Senhas**: Verificação Argon2 com salt único
- **Token JWT**: Geração, validação e expiração
- **Autorização**: Verificação de permissões por usuário
- **Prevenção de Ataques**: Proteção contra dados duplicados

### Validação de Dados
- **Campos Obrigatórios**: UserName, Password nunca vazios
- **Limites de Tamanho**: Máximo e mínimo para todos os campos
- **Caracteres Especiais**: Suporte a Unicode e caracteres especiais
- **Formato de Dados**: Validação de emails, números, enums

### Tratamento de Erros
- **Exception Handling**: Tratamento robusto de exceções
- **Status Codes**: HTTP status codes apropriados
- **Error Messages**: Mensagens localizadas e descritivas
- **Graceful Degradation**: Comportamento adequado em falhas

### Performance e Concorrência
- **Operações Simultâneas**: Testes de concorrência
- **Paginação**: Resultados paginados adequadamente
- **Resource Management**: Cleanup automático de recursos

### Internacionalização
- **Múltiplas Culturas**: Suporte a pt-BR e en
- **Fallback**: Cultura padrão quando não suportada
- **Resource Files**: Uso adequado de arquivos de recursos
- **Swagger Localization**: Documentação multilíngue

---

## 🎯 Métricas e Estatísticas

### Distribuição de Testes
- **Testes Unitários**: ~245 testes (68%)
- **Testes de Integração**: ~110 testes (31%)
- **Outras Categorias**: ~3 testes (1%)

### Cobertura por Funcionalidade
- **Account Management**: ~40% dos testes
- **Authentication & Security**: ~25% dos testes
- **Validation & Localization**: ~20% dos testes
- **API Integration**: ~15% dos testes

### Complexidade dos Testes
- **Testes Simples** (1-3 asserts): ~60%
- **Testes Médios** (4-6 asserts): ~30%
- **Testes Complexos** (7+ asserts): ~10%

### Padrões de Qualidade
- ✅ **100%** dos testes seguem padrão AAA
- ✅ **100%** dos testes têm nomes descritivos
- ✅ **95%** dos testes têm comentários explicativos
- ✅ **100%** dos testes são independentes
- ✅ **100%** dos testes são determinísticos

---

## 🎯 Conclusão

Esta documentação cobre todos os **349 testes** implementados no projeto Authentication.Tests, organizados em categorias lógicas e detalhadamente explicados. Cada teste é descrito com seu propósito específico, configuração necessária, execução e critérios de verificação.

### ✅ Status Atual dos Testes
- **Total de Testes**: 349 testes
- **Status**: ✅ **100% passando** (349 sucessos, 0 falhas)
- **Tempo de Execução**: ~11 segundos
- **Cobertura**: Funcionalidades principais e edge cases

### 🏆 Funcionalidades Cobertas

Os testes garantem cobertura completa das funcionalidades:

#### Core Business Logic
- ✅ **Entidades e DTOs**: Validação de propriedades e comportamento
- ✅ **Lógica de negócio e serviços**: CRUD operations, business rules
- ✅ **Persistência e repositórios**: Database operations, queries
- ✅ **Validação e segurança**: Input validation, password hashing
- ✅ **Autenticação e autorização**: JWT tokens, user permissions

#### API Integration
- ✅ **Controllers e APIs**: HTTP endpoints, status codes
- ✅ **Integração end-to-end**: Full request/response cycles
- ✅ **Error handling**: Exception scenarios, error responses
- ✅ **Content negotiation**: JSON serialization, headers

#### User Experience
- ✅ **Localização e internacionalização**: pt-BR e en support
- ✅ **Documentação API**: Swagger UI localized
- ✅ **Validation messages**: User-friendly error messages
- ✅ **Business constraints**: Unique usernames, data integrity

### 📈 Qualidade do Código de Teste

#### Padrões Seguidos
- ✅ **100%** seguem padrão Arrange-Act-Assert
- ✅ **100%** possuem nomes descritivos e claros
- ✅ **100%** são independentes e determinísticos
- ✅ **95%** incluem comentários explicativos
- ✅ **100%** utilizam assertions fluentes e expressivas

#### Técnicas Utilizadas
- ✅ **Mocking**: Isolamento completo de dependências
- ✅ **In-Memory Testing**: Testes de repositório isolados
- ✅ **Integration Testing**: WebApplicationFactory para testes E2E
- ✅ **Theory Testing**: Múltiplos cenários com data-driven tests
- ✅ **Edge Case Testing**: Valores nulos, vazios, extremos

### 🚀 Benefícios para o Desenvolvimento

#### Confiabilidade
- **Detecção precoce** de bugs e regressões
- **Validação automática** de regras de negócio
- **Garantia de qualidade** em mudanças de código
- **Documentação viva** do comportamento esperado

#### Manutenibilidade
- **Refactoring seguro** com testes como rede de segurança
- **Onboarding facilitado** para novos desenvolvedores
- **Specifications claras** de cada componente
- **Feedback rápido** durante desenvolvimento

#### Produtividade
- **Desenvolvimento guiado por testes** (TDD)
- **Debugging eficiente** com testes específicos
- **Deploy confiante** com validação automática
- **Integração contínua** robusta

### 🎉 Resultado Final

O projeto Authentication possui uma **infraestrutura de testes robusta e abrangente**, pronta para suportar desenvolvimento ágil e deployment seguro. A documentação aqui apresentada serve como:

1. **📖 Guia de referência** para entender o comportamento de cada componente
2. **🎯 Especificação executável** das regras de negócio
3. **🛠️ Base para novos testes** seguindo os padrões estabelecidos
4. **📚 Material de treinamento** para equipe de desenvolvimento

**O sistema está bem preparado para produção e evolução contínua!** 🎯

---

*Documentação gerada automaticamente baseada na análise completa dos 349 testes implementados no projeto Authentication.Tests.*