# 🌐 API Endpoints Reference

## Overview

This document provides a comprehensive reference of all API endpoints available in the Authentication Service.

> **🔔 Recent Changes (PR #124):** Update operations now support the `updatedBy` field for audit tracking. The `createdBy` field is automatically preserved and should not be sent in update requests. See the [Audit Fields section](#audit-fields-in-update-operations-pr-124) below for details.

## Base URL

- **Development**: `https://localhost:7001`
- **Production**: Configure based on your deployment

## Authentication

All endpoints marked with ✅ require JWT authentication. Include the token in the Authorization header:

```bash
Authorization: Bearer <your-jwt-token>
```

## Main Authentication Endpoints

| Method | Endpoint | Description | Authentication |
|--------|----------|-------------|----------------|
| **POST** | `/Authentication/GenerateToken` | 🔑 Generate JWT token | ❌ |
| **POST** | `/Authentication/AddAccount` | 👤 Create user account | ❌ |
| **GET** | `/health` | ❤️ Health check | ❌ |

## Account Management Endpoints

| Method | Endpoint | Description | Authentication |
|--------|----------|-------------|----------------|
| **GET** | `/Account/GetAccounts` | 📋 List all accounts | ✅ |
| **GET** | `/Account/GetAccountById/{id}` | 🔍 Get account by ID | ✅ |
| **POST** | `/Account/AddAccount` | ➕ Create new account | ❌ |
| **PUT** | `/Account/UpdateAccount/{id}` | ✏️ Update account | ✅ |
| **DELETE** | `/Account/DeleteAccount/{id}` | ❌ Delete account | ✅ |

## Claims Management Endpoints (RBAC)

| Method | Endpoint | Description | Authentication |
|--------|----------|-------------|----------------|
| **GET** | `/Claim/GetClaims` | 📋 List all claims | ✅ |
| **GET** | `/Claim/GetClaimById/{id}` | 🔍 Get claim by ID | ✅ |
| **POST** | `/Claim/AddClaim` | ➕ Create new claim | ✅ |
| **PUT** | `/Claim/UpdateClaim/{id}` | ✏️ Update claim | ✅ |
| **DELETE** | `/Claim/DeleteClaim/{id}` | ❌ Delete claim | ✅ |

## Actions Management Endpoints

| Method | Endpoint | Description | Authentication |
|--------|----------|-------------|----------------|
| **GET** | `/Action/GetActions` | 📋 List all actions | ✅ |
| **GET** | `/Action/GetActionById/{id}` | 🔍 Get action by ID | ✅ |
| **POST** | `/Action/AddAction` | ➕ Create new action | ✅ |
| **PUT** | `/Action/UpdateAction/{id}` | ✏️ Update action | ✅ |
| **DELETE** | `/Action/DeleteAction/{id}` | ❌ Delete action | ✅ |

## Claim-Action Mapping Endpoints

| Method | Endpoint | Description | Authentication |
|--------|----------|-------------|----------------|
| **GET** | `/ClaimAction/GetClaimActions` | 🔗 List claim-action mappings | ✅ |
| **POST** | `/ClaimAction/AddClaimAction` | 🔗 Map claim to action | ✅ |
| **PUT** | `/ClaimAction/UpdateClaimAction/{id}` | ✏️ Update mapping | ✅ |
| **DELETE** | `/ClaimAction/DeleteClaimAction/{id}` | ❌ Delete mapping | ✅ |

## Account Permissions Endpoints

| Method | Endpoint | Description | Authentication |
|--------|----------|-------------|----------------|
| **GET** | `/AccountClaimAction/GetAccountClaimActions` | 👥 List user permissions | ✅ |
| **POST** | `/AccountClaimAction/AddAccountClaimAction` | 👤 Assign permission to user | ✅ |
| **DELETE** | `/AccountClaimAction/DeleteAccountClaimAction/{idAccount}/{idClaimAction}` | ❌ Remove user permission | ✅ |

---

## Detailed Endpoint Documentation

### 🔑 Generate Authentication Token

Generates a JWT token for valid user credentials.

**Endpoint:** `POST /Authentication/GenerateToken`  
**Authentication:** Not required

**Request Body:**
```json
{
  "userName": "admin",
  "password": "password123"
}
```

**Success Response (200 OK):**
```json
{
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresIn": 3600,
    "userName": "admin",
    "claims": ["user:read", "user:write"],
    "tokenType": "Bearer"
  }
}
```

**Error Responses:**

**401 Unauthorized** - Invalid credentials:
```json
{
  "title": "Unauthorized",
  "status": 401,
  "detail": "Invalid username or password",
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.1",
  "instance": "/Authentication/GenerateToken"
}
```

**400 Bad Request** - Validation error:
```json
{
  "title": "Bad Request",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "instance": "/Authentication/GenerateToken"
}
```

**Example cURL:**
```bash
curl -X POST "https://localhost:7001/Authentication/GenerateToken" \
  -H "Content-Type: application/json" \
  -d '{"userName": "admin", "password": "password123"}'
```

---

### 👤 Create User Account

Registers a new user account with duplicate username prevention.

**Endpoint:** `POST /Account/AddAccount`  
**Authentication:** Not required (for initial registration)

**Request Body:**
```json
{
  "userName": "newUser",
  "password": "SecurePassword123!",
  "email": "newuser@example.com"
}
```

**Success Response (200 OK):**
```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/Account/AddAccount",
  "data": {
    "userId": 123,
    "userName": "newUser",
    "email": "newuser@example.com"
  }
}
```

**Error Responses:**

**409 Conflict** - Username already exists:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.8",
  "title": "Conflict",
  "status": 409,
  "detail": "Username already exists",
  "instance": "/Account/AddAccount"
}
```

**400 Bad Request** - Validation error:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Invalid request.",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/Account/AddAccount"
}
```

**Example cURL:**
```bash
curl -X POST "https://localhost:7001/Account/AddAccount" \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "newUser",
    "password": "SecurePassword123!",
    "email": "newuser@example.com"
  }'
```

---

### ❤️ Health Check

Verifies the API is running and the database is accessible.

**Endpoint:** `GET /health`  
**Authentication:** Not required

**Success Response (200 OK):**
```json
{
  "status": "Healthy"
}
```

**Example cURL:**
```bash
curl -X GET "https://localhost:7001/health"
```

---

### 📋 List All Claims

Returns a list of all permission claims in the system.

**Endpoint:** `GET /Claim/GetClaims`  
**Authentication:** Required

**Success Response (200 OK):**
```json
{
  "data": [
    {
      "id": 1,
      "type": "Permission",
      "value": "user:manage",
      "description": "Manage users"
    },
    {
      "id": 2,
      "type": "Permission",
      "value": "admin:full",
      "description": "Full administrator access"
    }
  ]
}
```

**Example cURL:**
```bash
curl -X GET "https://localhost:7001/Claim/GetClaims" \
  -H "Authorization: Bearer <your-token>"
```

---

### ➕ Create New Claim

Creates a new permission claim.

**Endpoint:** `POST /Claim/AddClaim`  
**Authentication:** Required

**Request Body:**
```json
{
  "type": "Permission",
  "value": "user:manage",
  "description": "Manage users"
}
```

**Success Response (200 OK):**
```json
{
  "data": {
    "id": 1,
    "type": "Permission",
    "value": "user:manage",
    "description": "Manage users"
  }
}
```

**Example cURL:**
```bash
curl -X POST "https://localhost:7001/Claim/AddClaim" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"type": "Permission", "value": "user:manage", "description": "Manage users"}'
```

---

### 🔗 Map Claim to Action

Creates a mapping between a claim and an action.

**Endpoint:** `POST /ClaimAction/AddClaimAction`  
**Authentication:** Required

**Request Body:**
```json
{
  "claimId": 1,
  "actionId": 1
}
```

**Success Response (200 OK):**
```json
{
  "data": {
    "id": 1,
    "claimId": 1,
    "actionId": 1
  }
}
```

**Example cURL:**
```bash
curl -X POST "https://localhost:7001/ClaimAction/AddClaimAction" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"claimId": 1, "actionId": 1}'
```

---

### 👤 Assign Permission to User

Assigns a claim-action permission to a specific user.

**Endpoint:** `POST /AccountClaimAction/AddAccountClaimAction`  
**Authentication:** Required

**Request Body:**
```json
{
  "accountId": 123,
  "claimActionId": 1
}
```

**Success Response (200 OK):**
```json
{
  "data": {
    "accountId": 123,
    "claimActionId": 1
  }
}
```

**Example cURL:**
```bash
curl -X POST "https://localhost:7001/AccountClaimAction/AddAccountClaimAction" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"accountId": 123, "claimActionId": 1}'
```

---

## Common Response Status Codes

| Status Code | Description |
|-------------|-------------|
| **200 OK** | Request succeeded |
| **400 Bad Request** | Invalid request data or validation error |
| **401 Unauthorized** | Missing or invalid authentication token |
| **404 Not Found** | Requested resource not found |
| **409 Conflict** | Resource conflict (e.g., duplicate username) |
| **500 Internal Server Error** | Server error |

## Error Response Format

All error responses follow the RFC 7807 Problem Details format:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.x.x",
  "title": "Error Title",
  "status": 400,
  "detail": "Detailed error message",
  "instance": "/endpoint/path"
}
```

## Language Support

All endpoints support internationalization. Use the `culture` query parameter:

```bash
# English
https://localhost:7001/Authentication/GenerateToken?culture=en

# Portuguese
https://localhost:7001/Authentication/GenerateToken?culture=pt-BR
```

The selected language is saved in a cookie and persists across requests.

## Rate Limiting

Currently, there is no rate limiting implemented. For production deployments, consider implementing rate limiting middleware.

## Audit Fields in Update Operations (PR #124)

> **Important Changes:** The handling of audit fields has been standardized across all update endpoints.

### Update Request Payloads

All update endpoints now support the following pattern for audit fields:

**Example for Claim Update:**
```json
{
  "id": 1,
  "type": "Permission",
  "value": "users:write",
  "description": "Updated description",
  "updatedBy": "admin"
}
```

**Key Points:**
- ✅ **`updatedBy`** is optional - include it to track who made the update
- ❌ **`createdBy`** should NOT be included - it's automatically preserved from creation
- ⏰ **`dtUpdated`** is automatically set by the system
- 🔒 **`dtCreated`** and the original `createdBy` are never modified

### Update Response Example

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/Claim/UpdateClaim",
  "data": {
    "id": 1,
    "type": "Permission",
    "value": "users:write",
    "description": "Updated description",
    "dtCreated": "2024-01-15T10:30:00Z",
    "dtUpdated": "2024-01-20T15:45:00Z",
    "dtDeleted": null,
    "createdBy": "developer",
    "updatedBy": "admin",
    "deletedBy": null
  }
}
```

**Notice:** The `createdBy` field remains "developer" (original creator) while `updatedBy` is "admin" (who made the update).

### Affected Update Endpoints

The following update endpoints support this pattern:

| Endpoint | Method | Supports updatedBy |
|----------|--------|-------------------|
| `/Account/UpdateAccount` | PUT | ✅ |
| `/Action/UpdateAction` | PUT | ✅ |
| `/Application/UpdateApplication` | PUT | ✅ |
| `/Claim/UpdateClaim` | PUT | ✅ |
| `/ClaimAction/UpdateClaimAction` | PUT | ✅ |
| `/ApplicationClaim/UpdateApplicationClaim` | PUT | ✅ |
| `/AccountClaimAction/UpdateAccountClaimAction` | PUT | ✅ |

For complete documentation on audit field behavior, see [Audit Fields Documentation](AUDIT_FIELDS.md).

## Related Documentation

- [API Reference](API.md) - Complete API documentation
- [Examples](EXAMPLES.md) - Practical integration examples
- [Audit Fields Documentation](AUDIT_FIELDS.md) - Complete audit field behavior
- [RBAC Guide](RBAC.md) - Role-Based Access Control guide
- [Quick Start](../getting-started/QUICK_START.md) - Getting started guide
