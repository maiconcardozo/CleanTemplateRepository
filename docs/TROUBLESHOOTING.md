# 🔧 Troubleshooting Guide

Este guia aborda os problemas mais comuns encontrados ao configurar e usar o Authentication Service, com soluções detalhadas e dicas de prevenção.

## 🚨 Problemas de Instalação e Configuração

### ❌ Erro de Conexão com Banco de Dados

**Erro:**
```
Unable to connect to any of the specified MySQL hosts
MySqlConnector.MySqlException: Unable to connect to server
```

**Causas Possíveis:**
1. MySQL não está rodando
2. Connection string incorreta
3. Firewall bloqueando conexão
4. Credenciais inválidas

**Soluções:**

#### 1. Verificar Status do MySQL
```bash
# Linux/Mac
sudo systemctl status mysql
# ou
brew services list | grep mysql

# Windows
net start | findstr mysql
# ou via Services.msc
```

#### 2. Testar Conexão Manual
```bash
mysql -h localhost -u authuser -p
# Digite a senha quando solicitado
```

#### 3. Verificar Connection String
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=AuthenticationDB;Uid=authuser;Pwd=password123;SslMode=none;AllowPublicKeyRetrieval=true;"
  }
}
```

#### 4. Recriar Usuário do Banco
```sql
DROP USER IF EXISTS 'authuser'@'localhost';
CREATE USER 'authuser'@'localhost' IDENTIFIED BY 'password123';
GRANT ALL PRIVILEGES ON AuthenticationDB.* TO 'authuser'@'localhost';
FLUSH PRIVILEGES;
```

### ❌ Erro de Migração do Entity Framework

**Erro:**
```
Unable to create an object of type 'ApiContextDevelopment'
No database provider has been configured for this DbContext
```

**Soluções:**

#### 1. Verificar Context Registration
```csharp
// No Program.cs ou Startup.cs
builder.Services.AddDbContext<ApiContextDevelopment>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
```

#### 2. Executar Migração com Verbose
```bash
cd Src/Authentication.API
dotnet ef database update --context ApiContextDevelopment --verbose
```

#### 3. Regenerar Migração se Necessário
```bash
# Remove migração corrompida
dotnet ef migrations remove --context ApiContextDevelopment

# Cria nova migração
dotnet ef migrations add InitialCreate --context ApiContextDevelopment

# Aplica migração
dotnet ef database update --context ApiContextDevelopment
```

### ❌ Erro de Dependências/Packages

**Erro:**
```
Package 'Package.Name' is incompatible with 'net9.0'
Could not load file or assembly 'System.Text.Json'
```

**Soluções:**

#### 1. Limpar Cache e Restaurar
```bash
dotnet nuget locals all --clear
dotnet restore Solution/Authentication.sln --force
dotnet build Solution/Authentication.sln --no-restore
```

#### 2. Verificar Versões de Packages
```bash
dotnet list package --outdated
dotnet list package --vulnerable
```

#### 3. Atualizar Packages Específicos
```bash
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.13
dotnet add package MySqlConnector --version 2.4.0
```

## 🔐 Problemas de Autenticação e JWT

### ❌ Token Inválido ou Expirado

**Erro:**
```
401 Unauthorized
{"type":"https://tools.ietf.org/html/rfc7231#section-6.3.1","title":"Unauthorized"}
```

**Diagnóstico:**

#### 1. Verificar Formato do Token
```bash
# Token deve estar no formato: Bearer {token}
curl -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

#### 2. Validar Token JWT Online
- Acesse [jwt.io](https://jwt.io)
- Cole seu token para verificar estrutura
- Confirme se não está expirado

#### 3. Verificar Configuração JWT
```json
{
  "JwtSettings": {
    "Issuer": "Authentication",
    "Audience": "AuthenticationClients", 
    "SecretKey": "super-secret-jwt-key-minimum-32-characters-long",
    "ExpirationMinutes": 60
  }
}
```

**Importante:** `SecretKey` deve ter pelo menos 32 caracteres.

### ❌ Erro de Hash de Senha

**Erro:**
```
Argon2 hashing failed
Invalid password verification
```

**Soluções:**

#### 1. Verificar Implementação Argon2
```csharp
// Correto
var hashedPassword = StringHelper.ComputeArgon2Hash(plainPassword);
var isValid = StringHelper.VerifyArgon2Hash(plainPassword, hashedPassword);
```

#### 2. Verificar Encoding
Certifique-se de que passwords estão em UTF-8:
```csharp
var passwordBytes = Encoding.UTF8.GetBytes(password);
```

### ❌ Claims não Aparecem no Token

**Problema:** Token é gerado mas não contém claims/permissions esperadas.

**Diagnóstico:**

#### 1. Verificar Mapeamento RBAC
```sql
-- Verificar se usuário tem claims associadas
SELECT a.UserName, c.Value, ac.Name 
FROM Account a 
JOIN AccountClaimAction aca ON a.Id = aca.IdAccount
JOIN ClaimAction ca ON aca.IdClaimAction = ca.Id  
JOIN Claim c ON ca.IdClaim = c.Id
JOIN Action ac ON ca.IdAction = ac.Id
WHERE a.UserName = 'admin';
```

#### 2. Debug Token Generation
```csharp
// Adicione logs no AccountService.GenerateToken
_logger.LogDebug("User {UserName} has {ClaimCount} claims", 
    account.UserName, accountClaimActions.Count);

foreach(var claim in accountClaimActions)
{
    _logger.LogDebug("Adding claim: {Claim}:{Action}", 
        claim.ClaimAction.Claim.Value, claim.ClaimAction.Action.Name);
}
```

## 🌐 Problemas de API e HTTP

### ❌ CORS Errors

**Erro:**
```
Access to fetch at 'https://localhost:7001' from origin 'http://localhost:3000' 
has been blocked by CORS policy
```

**Soluções:**

#### 1. Verificar Configuração CORS
```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://yourfrontend.com")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Se usar cookies
    });
});

// Aplicar middleware
app.UseCors("AllowSpecificOrigin");
```

#### 2. Ordem dos Middlewares
```csharp
// Ordem correta
app.UseRouting();
app.UseCors(); // ANTES de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
```

### ❌ SSL/HTTPS Issues

**Erro:**
```
The SSL connection could not be established
Certificate validation failed
```

**Soluções:**

#### 1. Desenvolvimento Local
```bash
# Confiar no certificado de desenvolvimento
dotnet dev-certs https --trust
```

#### 2. Configurar HTTP para Desenvolvimento
```json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5000"
      },
      "Https": {
        "Url": "https://localhost:5001"
      }
    }
  }
}
```

### ❌ Swagger não Carrega

**Erro:**
```
Failed to load API definition
Swagger UI not accessible
```

**Soluções:**

#### 1. Verificar Configuração Swagger
```csharp
// Program.cs
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// Only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

#### 2. Verificar XML Documentation
```xml
<!-- Authentication.API.csproj -->
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <DocumentationFile>bin\Debug\net9.0\Authentication.API.xml</DocumentationFile>
</PropertyGroup>
```

## 🧪 Problemas de Testes

### ❌ Testes Falhando

**Erro:**
```
Failed: Test method TestName threw exception
Connection string not found
```

**Soluções:**

#### 1. Configurar Test Settings
```json
// appsettings.Testing.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AuthenticationDB_Test;Uid=authuser;Pwd=password123;"
  }
}
```

#### 2. Mock Dependencies
```csharp
[Test]
public void Should_Authenticate_Valid_User()
{
    // Arrange
    var mockRepository = new Mock<IAccountRepository>();
    var mockUnitOfWork = new Mock<ILoginUnitOfWork>();
    
    mockRepository.Setup(r => r.GetByUserName("admin"))
               .Returns(new Account { UserName = "admin", Password = hashedPassword });
    
    mockUnitOfWork.Setup(u => u.AccountRepository).Returns(mockRepository.Object);
    
    var service = new AccountService(mockUnitOfWork.Object);
    
    // Act & Assert
    var result = service.GetAccountByUserNameAndPassword(account);
    Assert.IsNotNull(result);
}
```

### ❌ Integration Tests Failing

**Erro:**
```
Database connection failed during integration test
Service not registered
```

**Soluções:**

#### 1. Test Database Setup
```csharp
public class IntegrationTestBase : IDisposable
{
    protected readonly TestServer Server;
    protected readonly HttpClient Client;
    
    public IntegrationTestBase()
    {
        var builder = new WebApplicationBuilder();
        
        // Use in-memory database for tests
        builder.Services.AddDbContext<ApiContextDevelopment>(options =>
            options.UseInMemoryDatabase("TestDatabase"));
        
        var app = builder.Build();
        Server = new TestServer(app);
        Client = Server.CreateClient();
    }
}
```

## 📊 Problemas de Performance

### ❌ API Lenta

**Sintomas:**
- Endpoints demoram mais que 2-3 segundos
- Timeouts frequentes
- Alto uso de CPU/memória

**Diagnóstico:**

#### 1. Enable Detailed Logging
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

#### 2. Analisar Queries SQL
```bash
# Logs mostrarão queries executadas
dotnet run --verbosity detailed
```

#### 3. Monitor Performance
```csharp
// Add middleware for timing
app.Use(async (context, next) =>
{
    var stopwatch = Stopwatch.StartNew();
    await next();
    stopwatch.Stop();
    
    var responseTime = stopwatch.ElapsedMilliseconds;
    context.Response.Headers.Add("X-Response-Time", $"{responseTime}ms");
});
```

**Otimizações:**

#### 1. Database Indexing
```sql
-- Add indexes for frequently queried columns
CREATE INDEX idx_account_username ON Account(UserName);
CREATE INDEX idx_account_created ON Account(DtCreated);
```

#### 2. Connection Pooling
```csharp
builder.Services.AddDbContext<ApiContextDevelopment>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(3)));
```

## 🛠️ Ferramentas de Debug

### 📈 Monitoring
```bash
# Application insights
dotnet add package Microsoft.ApplicationInsights.AspNetCore

# Health checks
dotnet add package Microsoft.Extensions.Diagnostics.HealthChecks
```

### 🔍 Logging
```csharp
// Structured logging with Serilog
builder.Host.UseSerilog((context, config) =>
    config.WriteTo.Console()
          .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day));
```

### 📊 Profiling
```bash
# dotTrace (JetBrains)
# PerfView (Microsoft)
# Application Insights Profiler
```

## 📞 Quando Buscar Ajuda

### 🆘 Antes de Abrir Issue
1. ✅ Consultou este troubleshooting guide
2. ✅ Verificou logs detalhados
3. ✅ Testou em ambiente limpo
4. ✅ Reproduziu o problema consistentemente

### 📝 Informações para Include
```
**Environment:**
- OS: Windows 11 / Ubuntu 22.04 / macOS 13
- .NET Version: 8.0.118
- MySQL Version: 8.0.35
- IDE: Visual Studio 2022 17.8.0

**Issue:**
[Descrição clara do problema]

**Steps to Reproduce:**
1. [Passo 1]
2. [Passo 2]
3. [Problema ocorre]

**Expected vs Actual:**
Expected: [Comportamento esperado]
Actual: [O que realmente aconteceu]

**Logs:**
[Cole logs relevantes aqui]

**Configuration:**
[appsettings.json relevante]
```

### 🔗 Recursos de Ajuda
- **GitHub Issues**: [Criar nova issue](https://github.com/maiconcardozo/CleanTemplateRepository/issues)
- **Stack Overflow**: Tag `authentication-service`
- **Documentation**: [docs/](../docs/)
- **Community**: Discussões no GitHub

---

💡 **Dica**: Mantenha logs estruturados e monitore métricas para detectar problemas antes que afetem usuários finais.