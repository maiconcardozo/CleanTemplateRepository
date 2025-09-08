# 🚀 Quick Start Guide - Authentication Service

Este guia fornece instruções passo a passo para configurar e usar o serviço de autenticação em diferentes cenários.

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter os seguintes componentes instalados:

### Obrigatórios
- **.NET 9.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **MySQL 8.0+** - [Download](https://dev.mysql.com/downloads/mysql/)
- **Git** - [Download](https://git-scm.com/)

### Recomendados
- **Visual Studio 2022** com workload .NET - [Download](https://visualstudio.microsoft.com/)
- **Visual Studio Code** com extensão C# Dev Kit - [Download](https://code.visualstudio.com/)
- **MySQL Workbench** para gerenciamento do banco - [Download](https://dev.mysql.com/downloads/workbench/)

## 🏃‍♂️ Configuração Rápida (5 minutos)

### 1. Clone e Construa o Projeto

```bash
# Clone o repositório
git clone https://github.com/maiconcardozo/CleanTemplateRepository.git
cd CleanTemplateRepository

# Restaure as dependências
dotnet restore Solution/Authentication.sln

# Construa o projeto
dotnet build Solution/Authentication.sln --configuration Debug
```

### 2. Configure o Banco de Dados

#### Opção A: MySQL Local
```bash
# Inicie o MySQL e crie um banco
mysql -u root -p
CREATE DATABASE AuthenticationDB;
CREATE USER 'authuser'@'localhost' IDENTIFIED BY 'password123';
GRANT ALL PRIVILEGES ON AuthenticationDB.* TO 'authuser'@'localhost';
FLUSH PRIVILEGES;
exit;
```

#### Opção B: Docker MySQL (Mais Rápido)
```bash
# Execute MySQL em container Docker
docker run --name mysql-auth \
  -e MYSQL_ROOT_PASSWORD=rootpass \
  -e MYSQL_DATABASE=AuthenticationDB \
  -e MYSQL_USER=authuser \
  -e MYSQL_PASSWORD=password123 \
  -p 3306:3306 \
  -d mysql:8.0
```

### 3. Configure a Connection String

Edite `Src/Authentication.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AuthenticationDB;Uid=authuser;Pwd=password123;SslMode=none;"
  },
  "JwtSettings": {
    "Issuer": "Authentication",
    "Audience": "AuthenticationClients",
    "SecretKey": "super-secret-jwt-key-minimum-32-characters-long",
    "ExpirationMinutes": 60
  }
}
```

### 4. Execute as Migrações do Banco

```bash
cd Src/Authentication.API
dotnet ef database update --context ApiContextDevelopment
```

### 5. Execute a Aplicação

```bash
# Execute em modo de desenvolvimento
dotnet run --configuration Debug

# A API estará disponível em: https://localhost:7001
# Documentação Swagger: https://localhost:7001
```

## 🔐 Primeiro Uso - Testando a API

### 1. Acesse a Documentação Swagger

Abra seu navegador e vá para: **https://localhost:7001**

Você verá duas APIs documentadas:
- **Authentication API** - Login e geração de tokens
- **Access Control API** - Gerenciamento RBAC (Claims, Actions, etc.)

### 2. Crie sua Primeira Conta

```bash
# Usando curl
curl -X POST "https://localhost:7001/Authentication/AddAccount" \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "admin",
    "password": "AdminPass123!"
  }'
```

**Resposta esperada (200 OK):**
```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "data": {
    "userId": 1,
    "userName": "admin"
  }
}
```

### 3. Gere um Token JWT

```bash
curl -X POST "https://localhost:7001/Authentication/GenerateToken" \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "admin",
    "password": "AdminPass123!"
  }'
```

**Resposta esperada (200 OK):**
```json
{
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresIn": 3600,
    "userName": "admin",
    "tokenType": "Bearer"
  }
}
```

### 4. Use o Token para Acessar Endpoints Protegidos

```bash
# Salve o token em uma variável
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Liste todas as claims
curl -X GET "https://localhost:7001/Claim/GetClaims" \
  -H "Authorization: Bearer $TOKEN"
```

## 🔒 Configurando RBAC (Controle de Acesso)

### 1. Crie uma Claim (Permissão)

```bash
curl -X POST "https://localhost:7001/Claim/AddClaim" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "type": "Permission",
    "value": "UserManagement",
    "description": "Permissão para gerenciar usuários"
  }'
```

### 2. Crie uma Action (Ação do Sistema)

```bash
curl -X POST "https://localhost:7001/Action/AddAction" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Create",
    "description": "Criar novos registros"
  }'
```

### 3. Mapeie Claim para Action

```bash
curl -X POST "https://localhost:7001/ClaimAction/AddClaimAction" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "claimId": 1,
    "actionId": 1
  }'
```

### 4. Atribua Permissão ao Usuário

```bash
curl -X POST "https://localhost:7001/AccountClaimAction/AddAccountClaimAction" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "accountId": 1,
    "claimActionId": 1
  }'
```

## 🧪 Validando a Configuração

### Execute os Testes

```bash
# Execute todos os testes
dotnet test Src/Authentication.Tests/Authentication.Tests.csproj

# Execute apenas testes unitários
dotnet test --filter "FullyQualifiedName~Unit"

# Execute com scripts de conveniência
scripts/run-tests.sh unit    # Linux/Mac
scripts/run-tests.bat unit     # Windows
```

### Verifique a Geração de Token com Claims

Após configurar o RBAC, gere um novo token:

```bash
curl -X POST "https://localhost:7001/Authentication/GenerateToken" \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "admin",
    "password": "AdminPass123!"
  }'
```

O token agora deve incluir as claims no formato `"UserManagement:Create"`.

## 🔧 Integração com Frontend

### JavaScript/React Example

```javascript
class AuthService {
  constructor() {
    this.baseURL = 'https://localhost:7001';
    this.token = localStorage.getItem('authToken');
  }

  async login(userName, password) {
    const response = await fetch(`${this.baseURL}/Authentication/GenerateToken`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ userName, password })
    });
    
    if (response.ok) {
      const data = await response.json();
      this.token = data.data.accessToken;
      localStorage.setItem('authToken', this.token);
      return data;
    }
    throw new Error('Login failed');
  }

  async apiCall(endpoint, options = {}) {
    return fetch(`${this.baseURL}${endpoint}`, {
      ...options,
      headers: {
        'Authorization': `Bearer ${this.token}`,
        'Content-Type': 'application/json',
        ...options.headers
      }
    });
  }
}

// Uso
const auth = new AuthService();
await auth.login('admin', 'AdminPass123!');
const claims = await auth.apiCall('/Claim/GetClaims');
```

### C# Client Example

```csharp
public class AuthenticationClient
{
    private readonly HttpClient _httpClient;
    private string? _token;

    public AuthenticationClient()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7001") };
    }

    public async Task<bool> LoginAsync(string userName, string password)
    {
        var request = new { userName, password };
        var response = await _httpClient.PostAsJsonAsync("/Authentication/GenerateToken", request);
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            _token = result?.Data?.AccessToken;
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Claim>> GetClaimsAsync()
    {
        var response = await _httpClient.GetAsync("/Claim/GetClaims");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<Claim>>();
    }
}
```

## 📚 Próximos Passos

1. **Leia a documentação completa**: [docs/DEVELOPMENT.md](DEVELOPMENT.md)
2. **Configure para produção**: [docs/DEPLOYMENT.md](DEPLOYMENT.md)
3. **Entenda a arquitetura**: [docs/ARCHITECTURE.md](ARCHITECTURE.md)
4. **Veja mais exemplos**: [docs/EXAMPLES.md](EXAMPLES.md)
5. **Configure segurança**: [docs/SECURITY.md](SECURITY.md)

## 🆘 Problemas Comuns

### ❌ Erro de Conexão com Banco
```
Unable to connect to any of the specified MySQL hosts
```
**Solução**: Verifique se o MySQL está rodando e a connection string está correta.

### ❌ Erro de Migração
```
Unable to create an object of type 'ApiContextDevelopment'
```
**Solução**: 
```bash
cd Src/Authentication.API
dotnet ef database update --context ApiContextDevelopment --verbose
```

### ❌ Token Inválido
```
401 Unauthorized
```
**Solução**: Verifique se o token está sendo enviado no header `Authorization: Bearer {token}`.

### ❌ CORS Error (Frontend)
```
Access to fetch at 'https://localhost:7001' has been blocked by CORS policy
```
**Solução**: A API já está configurada com CORS permitindo todas as origens. Verifique se está usando HTTPS.

## 💡 Dicas de Desenvolvimento

- Use `dotnet watch run` para hot reload durante desenvolvimento
- Configure variáveis de ambiente para diferentes ambientes
- Use o Swagger UI para testar endpoints interativamente
- Monitore logs com `dotnet run --verbosity detailed`
- Use ferramentas como Postman ou Insomnia para testes de API

---

🎉 **Parabéns!** Você configurou com sucesso o Authentication Service. Para dúvidas, consulte a [documentação completa](../README.md) ou abra uma [issue](https://github.com/maiconcardozo/CleanTemplateRepository/issues).