# 📋 Practical Examples

## Overview

This guide provides practical usage examples of the Authentication API, including real use cases, implementations in different languages, and complete integration scenarios.

> **🔔 Recent Changes (PR #124):** The handling of audit fields in update operations has been standardized:
> - `updatedBy` field is now available in all update payloads (optional)
> - `createdBy` field is automatically preserved and should NOT be sent in updates
> - See [Audit Fields Documentation](AUDIT_FIELDS.md) for complete details

## 🚀 Initial Setup

### Prerequisites
- Authentication API running at `https://localhost:7001`
- User account created
- HTTP request tool (curl, Postman, etc.)

### Sample Data
For the examples below, we assume:
- **User**: `admin`
- **Password**: `AdminPassword123!`
- **Email**: `admin@example.com`

## 🔐 Complete Authentication Flow

### 1. Create User Account

#### cURL
```bash
curl -X POST "https://localhost:7001/Authentication/AddAccount" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "userName": "admin",
    "password": "AdminPassword123!",
    "email": "admin@example.com"
  }'
```

#### PowerShell
```powershell
$body = @{
    userName = "admin"
    password = "AdminPassword123!"
    email = "admin@example.com"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7001/Authentication/AddAccount" `
  -Method POST `
  -Body $body `
  -ContentType "application/json" `
  -SkipCertificateCheck
```

#### JavaScript/Fetch
```javascript
const createAccount = async () => {
  try {
    const response = await fetch('https://localhost:7001/Authentication/AddAccount', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        userName: 'admin',
        password: 'AdminPassword123!',
        email: 'admin@example.com'
      })
    });

    const result = await response.json();
    console.log('Account created:', result);
    return result;
  } catch (error) {
    console.error('Error creating account:', error);
  }
};
```

#### C# HttpClient
```csharp
using System.Text;
using System.Text.Json;

public class AuthClient
{
    private readonly HttpClient _httpClient;
    
    public AuthClient()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7001");
    }

    public async Task<AccountResponseDTO> CreateAccountAsync(AccountPayLoadDTO account)
    {
        var json = JsonSerializer.Serialize(account);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("/Authentication/AddAccount", content);
        response.EnsureSuccessStatusCode();
        
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AccountResponseDTO>(responseJson);
    }
}

// Usage
var client = new AuthClient();
var newAccount = new AccountPayLoadDTO
{
    UserName = "admin",
    Password = "AdminPassword123!",
    Email = "admin@example.com"
};

var result = await client.CreateAccountAsync(newAccount);
```

### 2. Generate JWT Token

#### cURL
```bash
curl -X POST "https://localhost:7001/Authentication/GenerateToken" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "userName": "admin",
    "password": "AdminPassword123!"
  }'
```

**Expected response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJhZG1pbiIsImVtYWlsIjoiYWRtaW5AZXhhbXBsZS5jb20iLCJBY2NvdW50SWQiOiIxIiwianRpIjoiMDA0YzE0Y2YtZmUzYS00YTk0LThjNGMtZDAzNjhjMjJkNDNlIiwiaWF0IjoxNzA1NDIzMjAwLCJuYmYiOjE3MDU0MjMyMDAsImV4cCI6MTcwNTQyNjgwMCwiaXNzIjoiQXV0aGVudGljYXRpb25TZXJ2aWNlIiwiYXVkIjoiQXV0aGVudGljYXRpb25DbGllbnRzIn0.signature-hash",
  "expiresIn": 3600,
  "tokenType": "Bearer",
  "userName": "admin",
  "issuedAt": "2024-01-16T10:00:00Z",
  "expiresAt": "2024-01-16T11:00:00Z"
}
```

#### JavaScript with token storage
```javascript
const loginAndStoreToken = async (userName, password) => {
  try {
    const response = await fetch('https://localhost:7001/Authentication/GenerateToken', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ userName, password })
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const tokenData = await response.json();
    
    // Store token in localStorage
    localStorage.setItem('authToken', tokenData.accessToken);
    localStorage.setItem('tokenExpiration', tokenData.expiresAt);
    
    console.log('Login successful!');
    return tokenData;
  } catch (error) {
    console.error('Login error:', error);
    throw error;
  }
};

// Function to get stored token
const getStoredToken = () => {
  const token = localStorage.getItem('authToken');
  const expiration = localStorage.getItem('tokenExpiration');
  
  if (!token || !expiration) {
    return null;
  }
  
  // Check if token expired
  if (new Date() >= new Date(expiration)) {
    localStorage.removeItem('authToken');
    localStorage.removeItem('tokenExpiration');
    return null;
  }
  
  return token;
};
```

## 🔒 RBAC System - Complete Examples

### 1. Configure Permission System

#### Step 1: Get authentication token
```bash
TOKEN=$(curl -s -X POST "https://localhost:7001/Authentication/GenerateToken" \
  -H "Content-Type: application/json" \
  -k \
  -d '{"userName": "admin", "password": "AdminPassword123!"}' | \
  jq -r '.accessToken')
```

#### Step 2: Create Claims (Permissions)

> **Note:** When creating entities, you don't need to provide audit fields like `createdBy` - they're set automatically by the system.

```bash
# Create administrator claim
curl -X POST "https://localhost:7001/Claim/AddClaim" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "type": "Role",
    "value": "Administrator",
    "description": "System administrator with full access"
  }'

# Create common user claim
curl -X POST "https://localhost:7001/Claim/AddClaim" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "type": "Role", 
    "value": "User",
    "description": "Common user with limited access"
  }'

# Create specific permission claims
curl -X POST "https://localhost:7001/Claim/AddClaim" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "type": "Permission",
    "value": "users:create",
    "description": "Permission to create users"
  }'
```

#### Step 3: Create Actions (System Actions)
```bash
# Create create user action
curl -X POST "https://localhost:7001/Action/AddAction" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "name": "CreateUser",
    "description": "Create new user in the system"
  }'

# Create delete user action
curl -X POST "https://localhost:7001/Action/AddAction" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "name": "DeleteUser",
    "description": "Delete user from the system"
  }'

# Create view reports action
curl -X POST "https://localhost:7001/Action/AddAction" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "name": "ViewReports",
    "description": "View system reports"
  }'
```

#### Step 4: Map Claims to Actions
```bash
# Map Administrator to CreateUser (assuming IDs 1 and 1)
curl -X POST "https://localhost:7001/ClaimAction/AddClaimAction" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "claimId": 1,
    "actionId": 1
  }'

# Map Administrator to DeleteUser (assuming IDs 1 and 2)
curl -X POST "https://localhost:7001/ClaimAction/AddClaimAction" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "claimId": 1,
    "actionId": 2
  }'
```

#### Step 5: Assign Permissions to User
```bash
# Assign Administrator claim to admin user (assuming IDs)
curl -X POST "https://localhost:7001/AccountClaimAction/AddAccountClaimAction" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "idAccount": 1,
    "idClaimAction": 1
  }'
```

### 2b. Update Existing Entities

> **Important (PR #124):** When updating entities, you can optionally include the `updatedBy` field to track who made the change. The `createdBy` field is automatically preserved and should NOT be included in update requests.

#### Update a Claim
```bash
# Update an existing claim
curl -X PUT "https://localhost:7001/Claim/UpdateClaim" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "id": 1,
    "type": "Permission",
    "value": "users:manage",
    "description": "Updated: Permission to manage all user operations",
    "updatedBy": "admin"
  }'
```

#### Update an Action
```bash
# Update an existing action
curl -X PUT "https://localhost:7001/Action/UpdateAction" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "id": 1,
    "name": "ManageUsers",
    "updatedBy": "systemadmin"
  }'
```

#### Update an Account
```bash
# Update an existing account
curl -X PUT "https://localhost:7001/Account/UpdateAccount" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "id": 2,
    "userName": "regularuser",
    "password": "NewPassword123!",
    "updatedBy": "admin"
  }'
```

#### Update a Claim-Action Mapping
```bash
# Update a claim-action mapping
curl -X PUT "https://localhost:7001/ClaimAction/UpdateClaimAction" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -k \
  -d '{
    "id": 1,
    "idClaim": 2,
    "idAction": 3,
    "updatedBy": "rbacadmin"
  }'
```

### 2. Complete RBAC Setup Script

#### Bash Script
```bash
#!/bin/bash

# Script to configure complete RBAC system
BASE_URL="https://localhost:7001"

# Function to login and get token
get_token() {
    local username=$1
    local password=$2
    
    TOKEN=$(curl -s -X POST "$BASE_URL/Authentication/GenerateToken" \
      -H "Content-Type: application/json" \
      -k \
      -d "{\"userName\": \"$username\", \"password\": \"$password\"}" | \
      jq -r '.accessToken')
    
    if [ "$TOKEN" == "null" ] || [ -z "$TOKEN" ]; then
        echo "Error: Could not obtain authentication token"
        exit 1
    fi
    
    echo "Token obtained successfully"
}

# Function to create claim
create_claim() {
    local type=$1
    local value=$2
    local description=$3
    
    curl -s -X POST "$BASE_URL/Claim/AddClaim" \
      -H "Authorization: Bearer $TOKEN" \
      -H "Content-Type: application/json" \
      -k \
      -d "{\"type\": \"$type\", \"value\": \"$value\", \"description\": \"$description\"}"
}

# Function to create action
create_action() {
    local name=$1
    local description=$2
    
    curl -s -X POST "$BASE_URL/Action/AddAction" \
      -H "Authorization: Bearer $TOKEN" \
      -H "Content-Type: application/json" \
      -k \
      -d "{\"name\": \"$name\", \"description\": \"$description\"}"
}

# Main
echo "🚀 Configuring RBAC system..."

# 1. Login
echo "📝 Logging in..."
get_token "admin" "AdminPassword123!"

# 2. Create claims
echo "🔐 Creating claims..."
create_claim "Role" "Administrator" "Administrator with full access"
create_claim "Role" "Manager" "Manager with management access"
create_claim "Role" "User" "Common user"
create_claim "Permission" "users:create" "Create users"
create_claim "Permission" "users:delete" "Delete users"
create_claim "Permission" "reports:view" "View reports"

# 3. Create actions
echo "⚡ Creating actions..."
create_action "CreateUser" "Create new user"
create_action "DeleteUser" "Delete user"
create_action "ViewReports" "View reports"
create_action "ManageSystem" "Manage system"

echo "✅ RBAC setup completed!"
```

### 3. Complete JavaScript Client

```javascript
class AuthenticationClient {
  constructor(baseUrl = 'https://localhost:7001') {
    this.baseUrl = baseUrl;
    this.token = this.getStoredToken();
  }

  // Token management
  getStoredToken() {
    const token = localStorage.getItem('authToken');
    const expiration = localStorage.getItem('tokenExpiration');
    
    if (!token || !expiration || new Date() >= new Date(expiration)) {
      this.clearToken();
      return null;
    }
    
    return token;
  }

  setToken(tokenData) {
    localStorage.setItem('authToken', tokenData.accessToken);
    localStorage.setItem('tokenExpiration', tokenData.expiresAt);
    this.token = tokenData.accessToken;
  }

  clearToken() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('tokenExpiration');
    this.token = null;
  }

  // Generic method for requests
  async request(endpoint, options = {}) {
    const url = `${this.baseUrl}${endpoint}`;
    const config = {
      headers: {
        'Content-Type': 'application/json',
      },
      ...options,
    };

    // Add token if available
    if (this.token) {
      config.headers.Authorization = `Bearer ${this.token}`;
    }

    try {
      const response = await fetch(url, config);
      
      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`HTTP ${response.status}: ${errorText}`);
      }

      const contentType = response.headers.get('content-type');
      if (contentType && contentType.includes('application/json')) {
        return await response.json();
      }
      
      return await response.text();
    } catch (error) {
      console.error(`Error in request ${endpoint}:`, error);
      throw error;
    }
  }

  // Authentication methods
  async login(userName, password) {
    const tokenData = await this.request('/Authentication/GenerateToken', {
      method: 'POST',
      body: JSON.stringify({ userName, password })
    });

    this.setToken(tokenData);
    return tokenData;
  }

  async createAccount(userName, password, email) {
    return await this.request('/Authentication/AddAccount', {
      method: 'POST',
      body: JSON.stringify({ userName, password, email })
    });
  }

  logout() {
    this.clearToken();
  }

  // RBAC methods
  async getClaims() {
    return await this.request('/Claim/GetClaims');
  }

  async createClaim(type, value, description) {
    return await this.request('/Claim/AddClaim', {
      method: 'POST',
      body: JSON.stringify({ type, value, description })
    });
  }

  async updateClaim(id, type, value, description, updatedBy) {
    return await this.request('/Claim/UpdateClaim', {
      method: 'PUT',
      body: JSON.stringify({ id, type, value, description, updatedBy })
    });
  }

  async getActions() {
    return await this.request('/Action/GetActions');
  }

  async createAction(name, description) {
    return await this.request('/Action/AddAction', {
      method: 'POST',
      body: JSON.stringify({ name, description })
    });
  }

  async updateAction(id, name, updatedBy) {
    return await this.request('/Action/UpdateAction', {
      method: 'PUT',
      body: JSON.stringify({ id, name, updatedBy })
    });
  }

  async mapClaimToAction(idClaim, idAction) {
    return await this.request('/ClaimAction/AddClaimAction', {
      method: 'POST',
      body: JSON.stringify({ idClaim, idAction })
    });
  }

  async assignPermissionToUser(idAccount, idClaimAction) {
    return await this.request('/AccountClaimAction/AddAccountClaimAction', {
      method: 'POST',
      body: JSON.stringify({ idAccount, idClaimAction })
    });
  }

  // Check if user is authenticated
  isAuthenticated() {
    return this.token !== null;
  }

  // Decode JWT token (client-side only for non-sensitive information)
  decodeToken() {
    if (!this.token) return null;

    try {
      const base64Url = this.token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(
        atob(base64).split('').map(c => 
          '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
        ).join('')
      );
      
      return JSON.parse(jsonPayload);
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }
}

// Usage example
const authClient = new AuthenticationClient();

// Complete setup example
async function setupCompleteRBAC() {
  try {
    // 1. Login
    console.log('Logging in...');
    await authClient.login('admin', 'AdminPassword123!');
    
    // 2. Create claims
    console.log('Creating claims...');
    const adminClaim = await authClient.createClaim('Role', 'Administrator', 'Full admin');
    const userClaim = await authClient.createClaim('Role', 'User', 'Common user');
    
    // 3. Create actions
    console.log('Creating actions...');
    const createUserAction = await authClient.createAction('CreateUser', 'Create user');
    const deleteUserAction = await authClient.createAction('DeleteUser', 'Delete user');
    
    // 4. Map claims to actions
    console.log('Mapping permissions...');
    await authClient.mapClaimToAction(1, 1); // Admin can create users
    await authClient.mapClaimToAction(1, 2); // Admin can delete users
    
    console.log('✅ RBAC setup completed!');
    
    // 5. Check token information
    const tokenInfo = authClient.decodeToken();
    console.log('User information:', {
      userId: tokenInfo.nameid,
      userName: tokenInfo.unique_name,
      email: tokenInfo.email
    });
    
  } catch (error) {
    console.error('❌ Setup error:', error);
  }
}
```

## 🔧 Frontend Framework Integration

### React Hook
```jsx
import { useState, useEffect, useContext, createContext } from 'react';

// Authentication context
const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(null);
  const [loading, setLoading] = useState(true);

  const authClient = new AuthenticationClient();

  useEffect(() => {
    // Check stored token on initialization
    const storedToken = authClient.getStoredToken();
    if (storedToken) {
      setToken(storedToken);
      const userInfo = authClient.decodeToken();
      setUser(userInfo);
    }
    setLoading(false);
  }, []);

  const login = async (userName, password) => {
    try {
      const tokenData = await authClient.login(userName, password);
      setToken(tokenData.accessToken);
      
      const userInfo = authClient.decodeToken();
      setUser(userInfo);
      
      return { success: true };
    } catch (error) {
      return { success: false, error: error.message };
    }
  };

  const logout = () => {
    authClient.logout();
    setToken(null);
    setUser(null);
  };

  const value = {
    user,
    token,
    login,
    logout,
    isAuthenticated: !!token,
    loading
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};

// Hook to use context
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
};

// Login component
export const LoginForm = () => {
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const { login, loading } = useAuth();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    
    const result = await login(userName, password);
    if (!result.success) {
      setError(result.error);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>User:</label>
        <input
          type="text"
          value={userName}
          onChange={(e) => setUserName(e.target.value)}
          required
        />
      </div>
      <div>
        <label>Password:</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
      </div>
      {error && <div style={{color: 'red'}}>{error}</div>}
      <button type="submit" disabled={loading}>
        {loading ? 'Logging in...' : 'Login'}
      </button>
    </form>
  );
};

// Protected component
export const ProtectedComponent = () => {
  const { user, isAuthenticated, logout } = useAuth();

  if (!isAuthenticated) {
    return <div>You need to be logged in to view this content.</div>;
  }

  return (
    <div>
      <h2>Protected Area</h2>
      <p>Welcome, {user?.unique_name}!</p>
      <p>Email: {user?.email}</p>
      <button onClick={logout}>Logout</button>
    </div>
  );
};
```

### Vue.js Composable
```javascript
// composables/useAuth.js
import { ref, computed } from 'vue'

const authToken = ref(null)
const user = ref(null)
const authClient = new AuthenticationClient()

export function useAuth() {
  const isAuthenticated = computed(() => !!authToken.value)
  
  const login = async (userName, password) => {
    try {
      const tokenData = await authClient.login(userName, password)
      authToken.value = tokenData.accessToken
      user.value = authClient.decodeToken()
      return { success: true }
    } catch (error) {
      return { success: false, error: error.message }
    }
  }
  
  const logout = () => {
    authClient.logout()
    authToken.value = null
    user.value = null
  }
  
  const initAuth = () => {
    const storedToken = authClient.getStoredToken()
    if (storedToken) {
      authToken.value = storedToken
      user.value = authClient.decodeToken()
    }
  }
  
  return {
    user: computed(() => user.value),
    token: computed(() => authToken.value),
    isAuthenticated,
    login,
    logout,
    initAuth
  }
}
```

## 🧪 Exemplos de Teste

### Teste de Integração Completo
```javascript
// Exemplo usando Jest
describe('Authentication API Integration', () => {
  let authClient;
  let testToken;

  beforeAll(async () => {
    authClient = new AuthenticationClient();
    
    // Create test account
    await authClient.createAccount('testuser', 'TestPassword123!', 'test@example.com');
    
    // Login and get token
    const tokenData = await authClient.login('testuser', 'TestPassword123!');
    testToken = tokenData.accessToken;
  });

  test('Should create and manage claims', async () => {
    // Create claim
    const claimResponse = await authClient.createClaim('Role', 'TestRole', 'Test role');
    expect(claimResponse).toHaveProperty('id');

    // List claims
    const claims = await authClient.getClaims();
    expect(Array.isArray(claims)).toBe(true);
    expect(claims.some(c => c.value === 'TestRole')).toBe(true);
  });

  test('Should create and manage actions', async () => {
    // Create action
    const actionResponse = await authClient.createAction('TestAction', 'Test action');
    expect(actionResponse).toHaveProperty('id');

    // List actions
    const actions = await authClient.getActions();
    expect(Array.isArray(actions)).toBe(true);
    expect(actions.some(a => a.name === 'TestAction')).toBe(true);
  });

  test('Should map claims to actions', async () => {
    // Assuming we have IDs from previous operations
    const mappingResponse = await authClient.mapClaimToAction(1, 1);
    expect(mappingResponse).toHaveProperty('id');
  });

  afterAll(async () => {
    // Cleanup if necessary
    authClient.logout();
  });
});
```

## 📱 Exemplo Mobile (React Native)

```javascript
// AuthService.js
import AsyncStorage from '@react-native-async-storage/async-storage';

class MobileAuthService {
  constructor(baseUrl = 'https://api.seudominio.com') {
    this.baseUrl = baseUrl;
  }

  async request(endpoint, options = {}) {
    const url = `${this.baseUrl}${endpoint}`;
    const token = await AsyncStorage.getItem('authToken');
    
    const config = {
      headers: {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` })
      },
      ...options,
    };

    const response = await fetch(url, config);
    
    if (!response.ok) {
      throw new Error(`HTTP ${response.status}`);
    }

    return await response.json();
  }

  async login(userName, password) {
    const tokenData = await this.request('/Authentication/GenerateToken', {
      method: 'POST',
      body: JSON.stringify({ userName, password })
    });

    await AsyncStorage.setItem('authToken', tokenData.accessToken);
    await AsyncStorage.setItem('tokenExpiration', tokenData.expiresAt);
    
    return tokenData;
  }

  async logout() {
    await AsyncStorage.multiRemove(['authToken', 'tokenExpiration']);
  }

  async isAuthenticated() {
    const token = await AsyncStorage.getItem('authToken');
    const expiration = await AsyncStorage.getItem('tokenExpiration');
    
    if (!token || !expiration) return false;
    
    return new Date() < new Date(expiration);
  }
}

// Componente de Login
import React, { useState } from 'react';
import { View, TextInput, Button, Alert } from 'react-native';

const LoginScreen = ({ navigation }) => {
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const authService = new MobileAuthService();

  const handleLogin = async () => {
    try {
      await authService.login(userName, password);
      navigation.navigate('Home');
    } catch (error) {
      Alert.alert('Erro', 'Falha no login: ' + error.message);
    }
  };

  return (
    <View style={{ padding: 20 }}>
      <TextInput
        placeholder="Usuário"
        value={userName}
        onChangeText={setUserName}
        style={{ borderWidth: 1, marginBottom: 10, padding: 10 }}
      />
      <TextInput
        placeholder="Senha"
        value={password}
        onChangeText={setPassword}
        secureTextEntry
        style={{ borderWidth: 1, marginBottom: 10, padding: 10 }}
      />
      <Button title="Entrar" onPress={handleLogin} />
    </View>
  );
};
```

## 🔗 Recursos Adicionais

### Postman Collection
```json
{
  "info": {
    "name": "Authentication API",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "variable": [
    {
      "key": "baseUrl",
      "value": "https://localhost:7001"
    },
    {
      "key": "authToken",
      "value": ""
    }
  ],
  "item": [
    {
      "name": "Authentication",
      "item": [
        {
          "name": "Generate Token",
          "request": {
            "method": "POST",
            "header": [
              {
                "key": "Content-Type",
                "value": "application/json"
              }
            ],
            "body": {
              "mode": "raw",
              "raw": "{\n  \"userName\": \"admin\",\n  \"password\": \"AdminPassword123!\"\n}"
            },
            "url": {
              "raw": "{{baseUrl}}/Authentication/GenerateToken",
              "host": ["{{baseUrl}}"],
              "path": ["Authentication", "GenerateToken"]
            }
          },
          "event": [
            {
              "listen": "test",
              "script": {
                "exec": [
                  "if (responseCode.code === 200) {",
                  "    const jsonData = pm.response.json();",
                  "    pm.collectionVariables.set('authToken', jsonData.accessToken);",
                  "}"
                ]
              }
            }
          ]
        }
      ]
    }
  ]
}
```

---

**Tip**: Always implement proper error handling and never expose tokens in logs or source code. Use environment variables for sensitive configurations in production.