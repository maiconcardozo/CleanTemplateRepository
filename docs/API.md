# 📖 API Documentation

## Overview

The Authentication API provides secure authentication and authorization services using JWT tokens. The API is organized into two main categories:

1. **Authentication API** - Handles user authentication and token generation
2. **Access Control API** - API endpoints for managing access control including accounts, claims, and actions

This documentation covers all available endpoints, request/response formats, and usage examples.

## Base Information

- **Base URL**: `https://localhost:7001` (Development) / `https://api.yourdomain.com` (Production)  
- **API Version**: v1
- **Content Type**: `application/json`
- **Authentication**: JWT Bearer Token (for protected endpoints)
- **REST Compliance**: Full HTTP status code compliance (200, 400, 401, 404, 409, 500)

## Swagger Documentation

Interactive API documentation is available through Swagger UI:

- **Main Swagger UI**: `https://localhost:7001/` 
- **Authentication API**: `https://localhost:7001/swagger/Authentication/swagger.json`
- **Access Control API**: `https://localhost:7001/swagger/AccessControl/swagger.json`

## Authentication Header

For protected endpoints, include the JWT token in the Authorization header:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## API Architecture

Based on the Program.cs configuration, the API is structured with two main definitions:

### Authentication API
- **Purpose**: User authentication and token generation
- **Controllers**: `CleanEntityController`
- **Endpoints**: Token generation and authentication-related operations
- **Swagger Endpoint**: `/swagger/Authentication/swagger.json`

### Access Control API  
- **Purpose**: Account management, permissions, and access control
- **Controllers**: `AccountController`, `AccountClaimActionController`, `ActionController`, `ClaimActionController`, `ClaimController`
- **Endpoints**: Account CRUD, claims management, actions, and permission assignments
- **Swagger Endpoint**: `/swagger/AccessControl/swagger.json`

Both APIs are accessible through the main Swagger UI interface at the root URL, with separate documentation for each API category.

---

# 🔐 Authentication API

The Authentication API handles user authentication and token generation.

## Endpoints

### POST /Authentication/GenerateToken

Generates a JWT token for valid user credentials.

**Request:**

```http
POST /Authentication/GenerateToken
Content-Type: application/json

{
  "userName": "admin",
  "password": "password123"
}
```

**Request Body Schema:**

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `userName` | string | ✅ | User's login name (3-50 characters) |
| `password` | string | ✅ | User's password (minimum 6 characters) |

**Response (200 OK):**

```json
{
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbiIsImp0aSI6IjEyMzQ1Njc4LTkwYWItY2RlZi0xMjM0LTU2Nzg5MGFiY2RlZiIsImlhdCI6MTY0MjY4MDAwMCwiZXhwIjoxNjQyNjgzNjAwLCJpc3MiOiJBdXRoZW50aWNhdGlvblNlcnZpY2UiLCJhdWQiOiJBdXRoZW50aWNhdGlvbkNsaWVudHMifQ.signature",
    "expiresIn": 3600,
    "userName": "admin",
    "claims": [
      "user:read",
      "user:write",
      "admin:access"
    ]
  }
}
```

**Response Schema:**

| Field | Type | Description |
|-------|------|-------------|
| `data` | object | Wrapper object containing response data |
| `data.accessToken` | string | JWT access token |
| `data.expiresIn` | number | Token expiration time in seconds |
| `data.userName` | string | Authenticated user's name |
| `data.claims` | string[] | User's permissions/claims |

**Error Responses:**

**400 Bad Request** - Invalid credentials:
```json
{
  "title": "Bad Request",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "instance": "/Authentication/GenerateToken",
  "errors": {
    "UserName": ["UserName is required"],
    "Password": ["Password must be at least 6 characters"]
  }
}
```

**401 Unauthorized** - Authentication failed:
```json
{
  "title": "Unauthorized",
  "status": 401,
  "detail": "Invalid username or password",
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.1",
  "instance": "/Authentication/GenerateToken"
}
```

**500 Internal Server Error** - Server error:
```json
{
  "title": "Internal Server Error",
  "status": 500,
  "detail": "An error occurred while processing your request",
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "instance": "/Authentication/GenerateToken"
}
```

---

# 🛡️ Access Control API

API endpoints for managing access control including accounts, claims, and actions.

## Account Management

### GET /Account/GetAccounts

Retrieves all user accounts in the system.

**Request:**

```http
GET /Account/GetAccounts
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/example/instance",
  "data": {
    "userId": 123,
    "userName": "example.example",
    "email": "example.example@example.com"
  }
}
```

**Error Responses:**

**400 Bad Request**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Invalid request.",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/example/instance"
}
```

**401 Unauthorized**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized.",
  "status": 401,
  "detail": "Authentication failed - Invalid credentials.",
  "instance": "/example/instance"
}
```

**500 Internal Server Error**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Internal server error.",
  "status": 500,
  "detail": "An unexpected error occurred.",
  "instance": "/example/instance"
}
```

### GET /Account/GetAccountById

Retrieves a specific account by its ID.

**Request:**

```http
GET /Account/GetAccountById?id=123
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | integer | ✅ | The account ID to retrieve |

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/example/instance",
  "data": {
    "userId": 123,
    "userName": "example.example",
    "email": "example.example@example.com"
  }
}
```

**Error Responses:**

**400 Bad Request**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Invalid request.",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/example/instance"
}
```

**401 Unauthorized**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized.",
  "status": 401,
  "detail": "Authentication failed - Invalid credentials.",
  "instance": "/example/instance"
}
```

**404 Not Found**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not found.",
  "status": 404,
  "detail": "The requested resource was not found.",
  "instance": "/example/instance"
}
```

**500 Internal Server Error**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Internal server error.",
  "status": 500,
  "detail": "An unexpected error occurred.",
  "instance": "/example/instance"
}
```

### POST /Account/AddAccount

Add a new account. Performs the account creation process using the provided user information.

**Request:**

```http
POST /Account/AddAccount
Content-Type: application/json

{
  "userName": "newuser",
  "password": "securepassword123"
}
```

**Request Body Schema:**

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `userName` | string | ✅ | Unique username (3-50 characters) |
| `password` | string | ✅ | User's password (minimum 6 characters) |

**Response (200 OK)** - Account created successfully:

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/example/instance",
  "data": {
    "userId": 123,
    "userName": "example.example",
    "email": "example.example@example.com"
  }
}
```

**Error Responses:**

**400 Bad Request** - Invalid data or validation error:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Invalid request.",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/example/instance"
}
```

**401 Unauthorized** - User not authorized:
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized.",
  "status": 401,
  "detail": "Authentication failed - Invalid credentials.",
  "instance": "/example/instance"
}
```

**500 Internal Server Error** - An unexpected error occurred and the account could not be inserted:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Internal server error.",
  "status": 500,
  "detail": "An unexpected error occurred.",
  "instance": "/example/instance"
}
```

### PUT /Account/UpdateAccount

Updates an existing account.

**Request:**

```http
PUT /Account/UpdateAccount?id=123
Content-Type: application/json

{
  "userName": "updateduser",
  "password": "newsecurepassword123"
}
```

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | integer | ✅ | The account ID to update |

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/example/instance",
  "data": {
    "userId": 123,
    "userName": "example.example",
    "email": "example.example@example.com"
  }
}
```

**Error Responses:**

**400 Bad Request**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Invalid request.",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/example/instance"
}
```

**401 Unauthorized**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized.",
  "status": 401,
  "detail": "Authentication failed - Invalid credentials.",
  "instance": "/example/instance"
}
```

**404 Not Found**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not found.",
  "status": 404,
  "detail": "The requested resource was not found.",
  "instance": "/example/instance"
}
```

**500 Internal Server Error**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Internal server error.",
  "status": 500,
  "detail": "An unexpected error occurred.",
  "instance": "/example/instance"
}
```

### DELETE /Account/DeleteAccount

Deletes an account from the system.

**Request:**

```http
DELETE /Account/DeleteAccount?id=123
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | integer | ✅ | The account ID to delete |

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/example/instance",
  "data": {
    "userId": 123,
    "userName": "example.example",
    "email": "example.example@example.com"
  }
}
```

**Error Responses:**

**400 Bad Request**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Invalid request.",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/example/instance"
}
```

**401 Unauthorized**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized.",
  "status": 401,
  "detail": "Authentication failed - Invalid credentials.",
  "instance": "/example/instance"
}
```

**404 Not Found**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not found.",
  "status": 404,
  "detail": "The requested resource was not found.",
  "instance": "/example/instance"
}
```

**500 Internal Server Error**:
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Internal server error.",
  "status": 500,
  "detail": "An unexpected error occurred.",
  "instance": "/example/instance"
}
```

## Claims Management

#### GET /Claim/GetClaims

Retrieves all available claims (permissions) in the system.

**Request:**

```http
GET /Claim/GetClaims
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**

```json
[
  {
    "id": 1,
    "type": "Permission",
    "value": "user:read",
    "description": "Permission to read user data",
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-15T10:30:00Z"
  },
  {
    "id": 2,
    "type": "Role",
    "value": "admin",
    "description": "Administrator role",
    "createdAt": "2024-01-15T10:35:00Z",
    "updatedAt": "2024-01-15T10:35:00Z"
  }
]
```

#### GET /Claim/GetClaimById/{id}

Retrieves a specific claim by its ID.

**Request:**

```http
GET /Claim/GetClaimById/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**

```json
{
  "id": 1,
  "type": "Permission",
  "value": "user:read",
  "description": "Permission to read user data",
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-15T10:30:00Z"
}
```

#### POST /Claim/AddClaim

Creates a new claim in the system.

**Request:**

```http
POST /Claim/AddClaim
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "type": "Permission",
  "value": "user:write",
  "description": "Permission to modify user data"
}
```

**Response (200 OK):**

```json
{
  "id": 3,
  "type": "Permission",
  "value": "user:write",
  "description": "Permission to modify user data",
  "createdAt": "2024-01-15T11:00:00Z",
  "updatedAt": "2024-01-15T11:00:00Z"
}
```

#### PUT /Claim/UpdateClaim/{id}

Updates an existing claim.

**Request:**

```http
PUT /Claim/UpdateClaim/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "type": "Permission",
  "value": "user:read",
  "description": "Updated: Permission to read user data"
}
```

#### DELETE /Claim/DeleteClaim/{id}

Deletes a claim from the system.

**Request:**

```http
DELETE /Claim/DeleteClaim/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**

```json
{
  "message": "Claim deleted successfully",
  "success": true
}
```

## Actions Management

#### GET /Action/GetActions

Retrieves all system actions that can be performed.

**Request:**

```http
GET /Action/GetActions
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**

```json
[
  {
    "id": 1,
    "name": "CreateUser",
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-15T10:30:00Z"
  },
  {
    "id": 2,
    "name": "DeleteUser", 
    "createdAt": "2024-01-15T10:35:00Z",
    "updatedAt": "2024-01-15T10:35:00Z"
  }
]
```

#### GET /Action/GetActionById/{id}

Retrieves a specific action by its ID.

#### POST /Action/AddAction

Creates a new system action.

**Request:**

```http
POST /Action/AddAction
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "name": "UpdateUser"
}
```

#### PUT /Action/UpdateAction/{id}

Updates an existing action.

#### DELETE /Action/DeleteAction/{id}

Deletes an action from the system.

## Claim-Action Relationships

#### GET /ClaimAction/GetClaimActions

Retrieves all mappings between claims and actions.

**Response (200 OK):**

```json
[
  {
    "id": 1,
    "claimId": 1,
    "actionId": 1,
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-15T10:30:00Z"
  }
]
```

#### POST /ClaimAction/AddClaimAction

Maps a claim to an action, defining what actions a claim can perform.

**Request:**

```http
POST /ClaimAction/AddClaimAction
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "claimId": 1,
  "actionId": 2
}
```

## User Permission Assignments

#### GET /AccountClaimAction/GetAccountClaimActions

Retrieves user permission assignments with optional filtering.

**Request:**

```http
GET /AccountClaimAction/GetAccountClaimActions?accountId=123
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### POST /AccountClaimAction/AddAccountClaimAction

Assigns permissions to a user account.

**Request:**

```http
POST /AccountClaimAction/AddAccountClaimAction
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "accountId": 123,
  "claimActionId": 1
}
```

## 📝 Data Transfer Objects

### AccountPayLoadDTO

Used for authentication and account creation requests.

```json
{
  "userName": "string",
  "password": "string"
}
```

**Validation Rules:**
- `userName`: Required, 3-50 characters, alphanumeric and underscore only
- `password`: Required, minimum 6 characters

### TokenResponseDTO

Response object for successful token generation.

```json
{
  "accessToken": "string",
  "expiresIn": "number",
  "userName": "string",
  "claims": ["string"]
}
```

### AccountResponseDTO

Response object for account operations. Contains the standard response format with account data.

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/example/instance",
  "data": {
    "userId": 123,
    "userName": "example.example",
    "email": "example.example@example.com"
  }
}
```

**Response Schema:**

| Field | Type | Description |
|-------|------|-------------|
| `type` | string | URI reference to RFC documentation |
| `title` | string | Short human-readable title |
| `status` | number | HTTP status code |
| `detail` | string | Detailed description of the response |
| `instance` | string | Reference to specific instance |
| `data` | object | Container for response data |
| `data.userId` | number | Unique user identifier |
| `data.userName` | string | User's login name |
| `data.email` | string | User's email address |

### ProblemDetails

Standard error response format following RFC 7807 (Problem Details for HTTP APIs).

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Invalid request.",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/example/instance"
}
```

**Schema:**

| Field | Type | Description |
|-------|------|-------------|
| `type` | string | URI reference to RFC documentation |
| `title` | string | Short human-readable title |
| `status` | number | HTTP status code |
| `detail` | string | Detailed description of the problem |
| `instance` | string | Reference to specific instance where the problem occurred |

### SucessDetails

Standard success response format for operations like account deletion.

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/example/instance",
  "data": {
    "userId": 123,
    "userName": "example.example",
    "email": "example.example@example.com"
  }
}
```

**Schema:**

| Field | Type | Description |
|-------|------|-------------|
| `type` | string | URI reference to RFC documentation |
| `title` | string | Short human-readable title |
| `status` | number | HTTP status code |
| `detail` | string | Detailed description of the success |
| `instance` | string | Reference to specific instance |
| `data` | object | Container for response data |

### ClaimPayLoadDTO

Used for creating and updating claims.

```json
{
  "type": "Permission|Role|Feature",
  "value": "string",
  "description": "string"
}
```

**Validation Rules:**
- `type`: Required, must be valid ClaimType enum value
- `value`: Required, unique claim value identifier
- `description`: Optional, claim description

### ClaimResponseDTO

Response object for claim operations.

```json
{
  "id": "number",
  "type": "Permission|Role|Feature",
  "value": "string",
  "description": "string",
  "createdAt": "datetime",
  "updatedAt": "datetime"
}
```

### ActionPayLoadDTO

Used for creating and updating actions.

```json
{
  "name": "string"
}
```

**Validation Rules:**
- `name`: Required, unique action name

### ActionResponseDTO

Response object for action operations.

```json
{
  "id": "number",
  "name": "string",
  "createdAt": "datetime",
  "updatedAt": "datetime"
}
```

### ClaimActionPayLoadDTO

Used for mapping claims to actions.

```json
{
  "claimId": "number",
  "actionId": "number"
}
```

**Validation Rules:**
- `claimId`: Required, must exist in Claims table
- `actionId`: Required, must exist in Actions table

### ClaimActionResponseDTO

Response object for claim-action mappings.

```json
{
  "id": "number",
  "claimId": "number",
  "actionId": "number",
  "createdAt": "datetime",
  "updatedAt": "datetime"
}
```

### AccountClaimActionPayLoadDTO

Used for assigning permissions to user accounts.

```json
{
  "accountId": "number",
  "claimActionId": "number"
}
```

**Validation Rules:**
- `accountId`: Required, must exist in Accounts table
- `claimActionId`: Required, must exist in ClaimActions table

### AccountClaimActionResponseDTO

Response object for user permission assignments.

```json
{
  "id": "number",
  "accountId": "number",
  "claimActionId": "number",
  "createdAt": "datetime",
  "updatedAt": "datetime"
}
```

## 🔒 Security

### JWT Token Structure

The JWT token contains the following claims:

```json
{
  "sub": "username",           // Subject (username)
  "jti": "unique-token-id",    // JWT ID
  "iat": 1642680000,           // Issued at (timestamp)
  "exp": 1642683600,           // Expiration (timestamp)
  "iss": "CleanEntityService",     // Issuer
  "aud": "AuthenticationClients",     // Audience
  "claims": ["user:read", "user:write"] // User permissions
}
```

### Password Security

- Passwords are hashed using Argon2 algorithm
- Minimum password length: 6 characters
- Passwords are never returned in API responses
- Salt is automatically generated for each password

### Rate Limiting

To prevent brute force attacks, the following rate limits apply:

| Endpoint | Rate Limit | Window |
|----------|------------|--------|
| `/Authentication/GenerateToken` | 5 requests | 1 minute |
| `/Authentication/AddAccount` | 3 requests | 5 minutes |

## 🧪 Examples

### cURL Examples

#### Generate Token

```bash
curl -X POST "https://localhost:7001/Authentication/GenerateToken" \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "admin",
    "password": "password123"
  }'
```

#### Create Account

```bash
curl -X POST "https://localhost:7001/Account/AddAccount" \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "newuser",
    "password": "securepassword123"
  }'
```

#### Get All Accounts

```bash
curl -X GET "https://localhost:7001/Account/GetAccounts" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

#### Get Account By ID

```bash
curl -X GET "https://localhost:7001/Account/GetAccountById?id=123" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

#### Update Account

```bash
curl -X PUT "https://localhost:7001/Account/UpdateAccount?id=123" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "updateduser",
    "password": "newsecurepassword123"
  }'
```

#### Delete Account

```bash
curl -X DELETE "https://localhost:7001/Account/DeleteAccount?id=123" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

#### Create Claim

```bash
curl -X POST "https://localhost:7001/Claim/AddClaim" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -H "Content-Type: application/json" \
  -d '{
    "type": "Permission",
    "value": "user:write",
    "description": "Permission to modify user data"
  }'
```

#### Get All Claims

```bash
curl -X GET "https://localhost:7001/Claim/GetClaims" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

#### Create Action

```bash
curl -X POST "https://localhost:7001/Action/AddAction" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -H "Content-Type: application/json" \
  -d '{
    "name": "UpdateUser"
  }'
```

#### Map Claim to Action

```bash
curl -X POST "https://localhost:7001/ClaimAction/AddClaimAction" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -H "Content-Type: application/json" \
  -d '{
    "claimId": 1,
    "actionId": 2
  }'
```

#### Assign Permission to User

```bash
curl -X POST "https://localhost:7001/AccountClaimAction/AddAccountClaimAction" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -H "Content-Type: application/json" \
  -d '{
    "accountId": 123,
    "claimActionId": 1
  }'
```

### JavaScript Examples

#### Generate Token (Fetch API)

```javascript
const response = await fetch('https://localhost:7001/Authentication/GenerateToken', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    userName: 'admin',
    password: 'password123'
  })
});

const data = await response.json();
console.log('Token:', data.accessToken);
```

#### Create Account (Axios)

```javascript
import axios from 'axios';

try {
const response = await axios.post('https://localhost:7001/Account/AddAccount', {
    userName: 'newuser',
    password: 'securepassword123'
  });
  
  console.log('Account created:', response.data);
} catch (error) {
  console.error('Error:', error.response.data);
}
```

#### Complete RBAC Setup Example

```javascript
// Complete workflow to set up a user with specific permissions
const setupUserPermissions = async (token) => {
  try {
    // 1. Create a claim
    const claimResponse = await axios.post('/Claim/AddClaim', {
      type: 'Permission',
      value: 'reports:view',
      description: 'Permission to view reports'
    }, {
      headers: { Authorization: `Bearer ${token}` }
    });

    // 2. Create an action  
    const actionResponse = await axios.post('/Action/AddAction', {
      name: 'ViewReports'
    }, {
      headers: { Authorization: `Bearer ${token}` }
    });

    // 3. Map claim to action
    const claimActionResponse = await axios.post('/ClaimAction/AddClaimAction', {
      claimId: claimResponse.data.id,
      actionId: actionResponse.data.id
    }, {
      headers: { Authorization: `Bearer ${token}` }
    });

    // 4. Assign permission to user
    const userPermissionResponse = await axios.post('/AccountClaimAction/AddAccountClaimAction', {
      accountId: 123,
      claimActionId: claimActionResponse.data.id
    }, {
      headers: { Authorization: `Bearer ${token}` }
    });

    console.log('User permissions setup completed:', userPermissionResponse.data);
  } catch (error) {
    console.error('Setup failed:', error.response.data);
  }
};
```

### C# Examples

#### Generate Token

```csharp
using System.Text;
using System.Text.Json;

var client = new HttpClient();
var payload = new
{
    userName = "admin",
    password = "password123"
};

var json = JsonSerializer.Serialize(payload);
var content = new StringContent(json, Encoding.UTF8, "application/json");

var response = await client.PostAsync("https://localhost:7001/Authentication/GenerateToken", content);
var result = await response.Content.ReadAsStringAsync();

Console.WriteLine(result);
```

#### Using Generated Token

```csharp
var client = new HttpClient();
client.DefaultRequestHeaders.Authorization = 
    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

var response = await client.GetAsync("https://localhost:7001/api/protected-endpoint");
```

## 🚨 Error Handling

### Standard Error Response Format

All error responses follow RFC 7807 (Problem Details for HTTP APIs):

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "The request could not be understood by the server",
  "instance": "/Authentication/GenerateToken",
  "errors": {
    "fieldName": ["Error message"]
  }
}
```

### HTTP Status Codes

| Status Code | Description | When Used |
|-------------|-------------|-----------|
| **200** | OK | Successful operation |
| **400** | Bad Request | Invalid request data or validation errors |
| **401** | Unauthorized | Authentication failed or invalid credentials |
| **403** | Forbidden | Valid authentication but insufficient permissions |
| **404** | Not Found | Resource not found |
| **409** | Conflict | Resource conflict (e.g., username already exists) |
| **422** | Unprocessable Entity | Valid syntax but semantic errors |
| **429** | Too Many Requests | Rate limit exceeded |
| **500** | Internal Server Error | Server-side error |

#### Enhanced Error Responses (PR #40)

The API now follows RFC 7807 (Problem Details for HTTP APIs) standard for all error responses:

**409 Conflict Example:**
```json
{
  "title": "Conflict",
  "status": 409,
  "detail": "An account with this username already exists.",
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.8",
  "instance": "/Account/AddAccount"
}
```

**404 Not Found Example:**
```json
{
  "title": "Not Found",
  "status": 404,
  "detail": "The requested resource was not found",
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "instance": "/Account/GetAccountById/999"
}
```

**401 Unauthorized Example:**
```json
{
  "title": "Unauthorized",
  "status": 401,
  "detail": "Invalid username or password",
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.1",
  "instance": "/Authentication/GenerateToken"
}
```

### Validation Errors

Validation errors return detailed field-level error messages:

```json
{
  "title": "Validation Error",
  "status": 400,
  "errors": {
    "UserName": [
      "UserName is required",
      "UserName must be between 3 and 50 characters"
    ],
    "Password": [
      "Password is required",
      "Password must be at least 6 characters"
    ]
  }
}
```

## 🔍 Testing

### Swagger UI

Interactive API documentation is available with two separate API definitions:

- **Main Swagger UI**: `https://localhost:7001/`
- **Authentication API**: For authentication and token generation endpoints
- **Access Control API**: For account management, claims, actions, and permissions

The Swagger UI allows you to:
- Explore all available endpoints organized by API category
- Test API calls directly from the browser  
- View request/response schemas
- Copy cURL commands for each endpoint
- Switch between Authentication and Access Control API documentation

### Health Check

Monitor API health using the health check endpoint:

```bash
curl -X GET "https://localhost:7001/health"
```

**Response:**
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0123456",
  "entries": {
    "database": {
      "status": "Healthy",
      "duration": "00:00:00.0012345"
    }
  }
}
```

## 📚 SDKs and Client Libraries

### Official SDKs

- **.NET SDK**: NuGet package `Authentication.Client`
- **JavaScript SDK**: NPM package `@yourorg/authentication-client`
- **Python SDK**: PyPI package `authentication-client`

### Community SDKs

- **PHP SDK**: Composer package `yourorg/authentication-php`
- **Go SDK**: Go module `github.com/yourorg/authentication-go`
- **Java SDK**: Maven artifact `com.yourorg:authentication-java`

## 🔄 Versioning

The API uses semantic versioning (SemVer) with the following strategy:

- **Major Version** (v1, v2): Breaking changes
- **Minor Version** (v1.1, v1.2): New features, backward compatible
- **Patch Version** (v1.1.1, v1.1.2): Bug fixes, backward compatible

### API Versioning Strategy

- URL path versioning: `/v1/Authentication/GenerateToken`
- Header versioning: `API-Version: 1.0`
- Accept header versioning: `Accept: application/vnd.api+json;version=1`

## 📞 Support

For API support and questions:

- **Documentation**: [API Docs](../../docs/)
- **Issues**: [GitHub Issues](../../issues)
- **Support Email**: api-support@yourdomain.com
- **Developer Forum**: [Community Forum](https://forum.yourdomain.com)