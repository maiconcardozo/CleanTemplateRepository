# 📖 API Documentation

## Overview

The Authentication API provides secure authentication and authorization services using JWT tokens. The API is organized into two main categories:

1. **Authentication API** - Handles user authentication and token generation
2. **Access Control API** - API endpoints for managing access control including accounts, claims, and actions

This documentation covers all available endpoints, request/response formats, and usage examples.

> **⚠️ RFC 7807 Compliance:** All error responses in this API follow the RFC 7807 (Problem Details for HTTP APIs) standard, ensuring consistent and structured error reporting across all endpoints.

## 📬 Postman Collection

**🚀 Quick Start with Postman**: A complete ready-to-use Postman collection is available with all endpoints organized by controller, including automatic token management and example payloads.

- **[Download Collection](Authentication_API.postman_collection.json)** - Import directly into Postman
- **[Collection Guide](POSTMAN_COLLECTION.md)** - How to import and use the collection

The collection includes:
- ✅ All 40+ API endpoints organized by controller
- ✅ Automatic JWT token extraction and injection
- ✅ Pre-configured request bodies with examples
- ✅ Environment variable support for different deployments

## Base Information

- **Base URL**: `https://localhost:7001` (Development) / `https://api.yourdomain.com` (Production)  
- **API Version**: v1
- **Content Type**: `application/json`
- **Authentication**: JWT Bearer Token (for protected endpoints)
- **REST Compliance**: Full HTTP status code compliance (200, 400, 401, 404, 409, 500)
- **Error Format**: RFC 7807 Problem Details

## 📋 Endpoints Summary

Quick reference for all available API endpoints:

| Controller | Endpoint | Method | Description | Auth Required |
|------------|----------|--------|-------------|---------------|
| **Authentication** | `/Authentication/GenerateToken` | POST | Generate JWT token | ❌ |
| **Account** | `/Account/GetAccounts` | GET | List all accounts | ✅ |
| **Account** | `/Account/GetAccountById` | GET | Get account by ID | ✅ |
| **Account** | `/Account/AddAccount` | POST | Create new account | ✅ |
| **Account** | `/Account/UpdateAccount` | PUT | Update account | ✅ |
| **Account** | `/Account/DeleteAccount` | DELETE | Delete account | ✅ |
| **Claim** | `/Claim/GetClaims` | GET | List all claims | ✅ |
| **Claim** | `/Claim/GetClaimById/{id}` | GET | Get claim by ID | ✅ |
| **Claim** | `/Claim/AddClaim` | POST | Create new claim | ✅ |
| **Claim** | `/Claim/UpdateClaim/{id}` | PUT | Update claim | ✅ |
| **Claim** | `/Claim/DeleteClaim/{id}` | DELETE | Delete claim | ✅ |
| **Action** | `/Action/GetActions` | GET | List all actions | ✅ |
| **Action** | `/Action/GetActionById/{id}` | GET | Get action by ID | ✅ |
| **Action** | `/Action/AddAction` | POST | Create new action | ✅ |
| **Action** | `/Action/UpdateAction/{id}` | PUT | Update action | ✅ |
| **Action** | `/Action/DeleteAction/{id}` | DELETE | Delete action | ✅ |
| **ClaimAction** | `/ClaimAction/GetClaimActions` | GET | List claim-action mappings | ✅ |
| **ClaimAction** | `/ClaimAction/AddClaimAction` | POST | Map claim to action | ✅ |
| **AccountClaimAction** | `/AccountClaimAction/GetAccountClaimActions` | GET | List user permissions | ✅ |
| **AccountClaimAction** | `/AccountClaimAction/AddAccountClaimAction` | POST | Assign permission to user | ✅ |
| **Application** | `/Application/GetApplications` | GET | List all applications | ✅ |
| **Application** | `/Application/GetApplicationById/{id}` | GET | Get application by ID | ✅ |
| **Application** | `/Application/AddApplication` | POST | Create new application | ✅ |
| **Application** | `/Application/UpdateApplication` | PUT | Update application | ✅ |
| **Application** | `/Application/DeleteApplication/{id}` | DELETE | Delete application | ✅ |
| **ApplicationClaim** | `/ApplicationClaim/GetApplicationClaims` | GET | List all application-claim mappings | ✅ |
| **ApplicationClaim** | `/ApplicationClaim/GetApplicationClaimById/{id}` | GET | Get mapping by ID | ✅ |
| **ApplicationClaim** | `/ApplicationClaim/GetApplicationClaimsByApplicationId/{applicationId}` | GET | Get mappings by application | ✅ |
| **ApplicationClaim** | `/ApplicationClaim/AddApplicationClaim` | POST | Create application-claim mapping | ✅ |
| **ApplicationClaim** | `/ApplicationClaim/UpdateApplicationClaim` | PUT | Update mapping | ✅ |
| **ApplicationClaim** | `/ApplicationClaim/DeleteApplicationClaim/{id}` | DELETE | Delete mapping | ✅ |

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
- **Controllers**: `AuthenticationController`
- **Endpoints**: Token generation and authentication-related operations
- **Swagger Endpoint**: `/swagger/Authentication/swagger.json`

### Access Control API  
- **Purpose**: Account management, permissions, and access control
- **Controllers**: `AccountController`, `AccountClaimActionController`, `ActionController`, `ClaimActionController`, `ClaimController`, `ApplicationController`, `ApplicationClaimController`
- **Endpoints**: Account CRUD, claims management, actions, permission assignments, and multi-tenant application discrimination
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

> **🔑 Recommended Claims:** `account:read`, `account:write`, `account:delete`

### GET /Account/GetAccounts

Retrieves all user accounts in the system.

> **💡 Note:** Future implementations may include pagination and filtering capabilities (e.g., filter by username, active status, or creation date).

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
  "instance": "/Account/GetAccounts",
  "data": [
    {
      "id": 123,
      "userName": "example.example",
      "isActive": true,
      "dtCreated": "2024-01-15T10:30:00Z",
      "dtUpdated": "2024-01-16T14:20:00Z",
      "dtDeleted": null,
      "createdBy": "admin",
      "updatedBy": "manager",
      "deletedBy": null
    }
  ]
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

> **🔑 Recommended Claims:** `account:read`

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
  "instance": "/Account/GetAccountById",
  "data": {
    "id": 123,
    "userName": "example.example",
    "isActive": true,
    "dtCreated": "2024-01-15T10:30:00Z",
    "dtUpdated": "2024-01-16T14:20:00Z",
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": "manager",
    "deletedBy": null
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

> **🔑 Recommended Claims:** `account:write`

**Request:**

```http
POST /Account/AddAccount
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
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
  "instance": "/Account/AddAccount",
  "data": {
    "id": 124,
    "userName": "newuser",
    "isActive": true,
    "dtCreated": "2024-01-20T09:00:00Z",
    "dtUpdated": null,
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": null,
    "deletedBy": null
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

> **🔑 Recommended Claims:** `account:write`

**Request:**

```http
PUT /Account/UpdateAccount?id=123
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "userName": "updateduser",
  "password": "newsecurepassword123",
  "updatedBy": "manager"
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
  "instance": "/Account/UpdateAccount",
  "data": {
    "id": 123,
    "userName": "updateduser",
    "isActive": true,
    "dtCreated": "2024-01-15T10:30:00Z",
    "dtUpdated": "2024-01-21T15:30:00Z",
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": "manager",
    "deletedBy": null
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

> **🔑 Recommended Claims:** `account:delete`

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
  "detail": "Account deleted successfully.",
  "instance": "/Account/DeleteAccount",
  "data": {
    "id": 123,
    "userName": "example.example",
    "isActive": false,
    "dtCreated": "2024-01-15T10:30:00Z",
    "dtUpdated": "2024-01-21T15:30:00Z",
    "dtDeleted": "2024-01-22T10:00:00Z",
    "createdBy": "admin",
    "updatedBy": "manager",
    "deletedBy": "admin"
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

## Application Management

### Overview
Applications are used to discriminate and organize claims per application/system in a multi-tenant environment. Each application can have specific claims associated with it.

> **🔑 Recommended Claims:** `application:read`, `application:write`, `application:delete`

### GET /Application/GetApplications

Retrieves all applications in the system.

> **💡 Note:** Future implementations may include pagination and filtering capabilities (e.g., filter by name, creation date, or active status).

**Request:**

```http
GET /Application/GetApplications
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/Application/GetApplications",
  "data": [
    {
      "id": 1,
      "name": "WebApp",
      "description": "Main web application",
      "dtCreated": "2024-01-15T10:30:00Z",
      "dtUpdated": "2024-01-16T14:20:00Z",
      "dtDeleted": null,
      "createdBy": "admin",
      "updatedBy": "manager",
      "deletedBy": null
    }
  ]
}
```

**Error Responses:** Standard RFC 7807 error responses (400, 401, 500).

### GET /Application/GetApplicationById/{id}

Retrieves a specific application by ID.

> **🔑 Recommended Claims:** `application:read`

**Request:**

```http
GET /Application/GetApplicationById/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | integer | ✅ | Application ID to retrieve |

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/Application/GetApplicationById/1",
  "data": {
    "id": 1,
    "name": "WebApp",
    "description": "Main web application",
    "dtCreated": "2024-01-15T10:30:00Z",
    "dtUpdated": "2024-01-16T14:20:00Z",
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": "manager",
    "deletedBy": null
  }
}
```

**Error Responses:** Standard RFC 7807 error responses (400, 401, 404, 500).

### POST /Application/AddApplication

Creates a new application.

> **🔑 Recommended Claims:** `application:write`

**Request:**

```http
POST /Application/AddApplication
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "name": "MobileApp",
  "description": "Mobile application for iOS and Android",
  "createdBy": "admin"
}
```

**Request Body Schema:**

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `name` | string | ✅ | Application name (no spaces, 1-100 characters) |
| `description` | string | ✅ | Application description (1-500 characters) |
| `createdBy` | string | ✅ | Username creating the application (max 100 characters) |

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/Application/AddApplication",
  "data": {
    "id": 2,
    "name": "MobileApp",
    "description": "Mobile application for iOS and Android",
    "dtCreated": "2024-01-20T09:00:00Z",
    "dtUpdated": null,
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": null,
    "deletedBy": null
  }
}
```

**Error Responses:** Standard RFC 7807 error responses (400, 401, 500).

### PUT /Application/UpdateApplication

Updates an existing application.

> **🔑 Recommended Claims:** `application:write`

**Request:**

```http
PUT /Application/UpdateApplication
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "id": 2,
  "name": "MobileApp",
  "description": "Updated mobile application for iOS, Android, and Web",
  "updatedBy": "manager"
}
```

**Request Body Schema:**

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `id` | integer | ✅ | Application ID to update |
| `name` | string | ✅ | Updated application name |
| `description` | string | ✅ | Updated application description |
| `updatedBy` | string | ✅ | Username updating the application |

**Note:** The `createdBy` field is automatically preserved and should NOT be included in the request.

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/Application/UpdateApplication",
  "data": {
    "id": 2,
    "name": "MobileApp",
    "description": "Updated mobile application for iOS, Android, and Web",
    "dtCreated": "2024-01-20T09:00:00Z",
    "dtUpdated": "2024-01-21T15:30:00Z",
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": "manager",
    "deletedBy": null
  }
}
```

**Error Responses:** Standard RFC 7807 error responses (400, 401, 404, 500).

### DELETE /Application/DeleteApplication/{id}

Deletes an application from the system.

> **🔑 Recommended Claims:** `application:delete`

**Request:**

```http
DELETE /Application/DeleteApplication/2
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | integer | ✅ | Application ID to delete |

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Application deleted successfully.",
  "instance": "/Application/DeleteApplication/2",
  "data": {
    "id": 2,
    "name": "MobileApp",
    "description": "Updated mobile application for iOS, Android, and Web",
    "dtCreated": "2024-01-20T09:00:00Z",
    "dtUpdated": "2024-01-21T15:30:00Z",
    "dtDeleted": "2024-01-22T10:00:00Z",
    "createdBy": "admin",
    "updatedBy": "manager",
    "deletedBy": "admin"
  }
}
```

**Error Responses:** Standard RFC 7807 error responses (400, 401, 404, 500).

## Application-Claim Mappings

### Overview
Application-claim mappings link claims to specific applications, enabling multi-tenant permission management where claims can be organized per application/system.

> **🔑 Recommended Claims:** `applicationclaim:read`, `applicationclaim:write`, `applicationclaim:delete`

### GET /ApplicationClaim/GetApplicationClaims

Retrieves all application-claim mappings in the system.

> **💡 Note:** Future implementations may include pagination and filtering capabilities (e.g., filter by application or claim).

**Request:**

```http
GET /ApplicationClaim/GetApplicationClaims
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/ApplicationClaim/GetApplicationClaims",
  "data": [
    {
      "id": 1,
      "idApplication": 1,
      "idClaim": 5,
      "dtCreated": "2024-01-15T10:30:00Z",
      "dtUpdated": null,
      "dtDeleted": null,
      "createdBy": "admin",
      "updatedBy": null,
      "deletedBy": null
    }
  ]
}
```

**Error Responses:** Standard RFC 7807 error responses (400, 401, 500).

### GET /ApplicationClaim/GetApplicationClaimById/{id}

Retrieves a specific application-claim mapping by ID.

> **🔑 Recommended Claims:** `applicationclaim:read`

**Request:**

```http
GET /ApplicationClaim/GetApplicationClaimById/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | integer | ✅ | Application-claim mapping ID |

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/ApplicationClaim/GetApplicationClaimById/1",
  "data": {
    "id": 1,
    "idApplication": 1,
    "idClaim": 5,
    "dtCreated": "2024-01-15T10:30:00Z",
    "dtUpdated": null,
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": null,
    "deletedBy": null
  }
}
```

**Error Responses:** Standard RFC 7807 error responses (400, 401, 404, 500).

### GET /ApplicationClaim/GetApplicationClaimsByApplicationId/{applicationId}

Retrieves all claim mappings for a specific application.

> **🔑 Recommended Claims:** `applicationclaim:read`

**Request:**

```http
GET /ApplicationClaim/GetApplicationClaimsByApplicationId/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `applicationId` | integer | ✅ | Application ID to filter by |

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/ApplicationClaim/GetApplicationClaimsByApplicationId/1",
  "data": [
    {
      "id": 1,
      "idApplication": 1,
      "idClaim": 5,
      "dtCreated": "2024-01-15T10:30:00Z",
      "dtUpdated": null,
      "dtDeleted": null,
      "createdBy": "admin",
      "updatedBy": null,
      "deletedBy": null
    },
    {
      "id": 2,
      "idApplication": 1,
      "idClaim": 8,
      "dtCreated": "2024-01-16T11:00:00Z",
      "dtUpdated": null,
      "dtDeleted": null,
      "createdBy": "admin",
      "updatedBy": null,
      "deletedBy": null
    }
  ]
}
```

**Error Responses:** Standard RFC 7807 error responses (400, 401, 500).

### POST /ApplicationClaim/AddApplicationClaim

Creates a new application-claim mapping.

> **🔑 Recommended Claims:** `applicationclaim:write`

**Request:**

```http
POST /ApplicationClaim/AddApplicationClaim
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "idApplication": 1,
  "idClaim": 10,
  "createdBy": "admin"
}
```

**Request Body Schema:**

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `idApplication` | integer | ✅ | Application ID (must be greater than 0) |
| `idClaim` | integer | ✅ | Claim ID (must be greater than 0) |
| `createdBy` | string | ✅ | Username creating the mapping (max 100 characters) |

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/ApplicationClaim/AddApplicationClaim",
  "data": {
    "id": 3,
    "idApplication": 1,
    "idClaim": 10,
    "dtCreated": "2024-01-20T09:00:00Z",
    "dtUpdated": null,
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": null,
    "deletedBy": null
  }
}
```

**Error Responses:** Standard RFC 7807 error responses (400, 401, 500).

### PUT /ApplicationClaim/UpdateApplicationClaim

Updates an existing application-claim mapping.

> **🔑 Recommended Claims:** `applicationclaim:write`

**Request:**

```http
PUT /ApplicationClaim/UpdateApplicationClaim
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "id": 3,
  "idApplication": 2,
  "idClaim": 10,
  "updatedBy": "manager"
}
```

**Request Body Schema:**

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `id` | integer | ✅ | Mapping ID to update |
| `idApplication` | integer | ✅ | Updated application ID |
| `idClaim` | integer | ✅ | Updated claim ID |
| `updatedBy` | string | ✅ | Username updating the mapping |

**Note:** The `createdBy` field is automatically preserved and should NOT be included in the request.

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/ApplicationClaim/UpdateApplicationClaim",
  "data": {
    "id": 3,
    "idApplication": 2,
    "idClaim": 10,
    "dtCreated": "2024-01-20T09:00:00Z",
    "dtUpdated": "2024-01-21T15:30:00Z",
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": "manager",
    "deletedBy": null
  }
}
```

**Error Responses:** Standard RFC 7807 error responses (400, 401, 404, 500).

### DELETE /ApplicationClaim/DeleteApplicationClaim/{id}

Deletes an application-claim mapping.

> **🔑 Recommended Claims:** `applicationclaim:delete`

**Request:**

```http
DELETE /ApplicationClaim/DeleteApplicationClaim/3
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | integer | ✅ | Mapping ID to delete |

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Application-claim mapping deleted successfully.",
  "instance": "/ApplicationClaim/DeleteApplicationClaim/3",
  "data": {
    "id": 3,
    "idApplication": 2,
    "idClaim": 10,
    "dtCreated": "2024-01-20T09:00:00Z",
    "dtUpdated": "2024-01-21T15:30:00Z",
    "dtDeleted": "2024-01-22T10:00:00Z",
    "createdBy": "admin",
    "updatedBy": "manager",
    "deletedBy": "admin"
  }
}
```

**Error Responses:** Standard RFC 7807 error responses (400, 401, 404, 500).

## Claims Management

#### GET /Claim/GetClaims

Retrieves all available claims (permissions) in the system.

> **🔑 Recommended Claims:** `claim:read`

> **💡 Note:** Future implementations may include pagination and filtering capabilities (e.g., filter by type or value).

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

> **🔑 Recommended Claims:** `claim:read`

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
  "dtCreated": "2024-01-15T10:30:00Z",
  "dtUpdated": "2024-01-15T10:30:00Z",
  "dtDeleted": null,
  "createdBy": "admin",
  "updatedBy": null,
  "deletedBy": null
}
```

#### POST /Claim/AddClaim

Creates a new claim in the system.

> **🔑 Recommended Claims:** `claim:write`

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
  "dtCreated": "2024-01-15T11:00:00Z",
  "dtUpdated": null,
  "dtDeleted": null,
  "createdBy": "admin",
  "updatedBy": null,
  "deletedBy": null
}
```

#### PUT /Claim/UpdateClaim/{id}

Updates an existing claim.

> **🔑 Recommended Claims:** `claim:write`

**Request:**

```http
PUT /Claim/UpdateClaim/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "type": "Permission",
  "value": "user:read",
  "description": "Updated: Permission to read user data",
  "updatedBy": "manager"
}
```

#### DELETE /Claim/DeleteClaim/{id}

Deletes a claim from the system.

> **🔑 Recommended Claims:** `claim:delete`

**Request:**

```http
DELETE /Claim/DeleteClaim/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Claim deleted successfully.",
  "instance": "/Claim/DeleteClaim/1",
  "data": {
    "id": 1,
    "type": "Permission",
    "value": "user:read",
    "description": "Permission to read user data",
    "dtCreated": "2024-01-15T10:30:00Z",
    "dtUpdated": null,
    "dtDeleted": "2024-01-22T10:00:00Z",
    "createdBy": "admin",
    "updatedBy": null,
    "deletedBy": "admin"
  }
}
```

## Actions Management

> **🔑 Recommended Claims:** `action:read`, `action:write`, `action:delete`

#### GET /Action/GetActions

Retrieves all system actions that can be performed.

> **💡 Note:** Future implementations may include pagination and filtering capabilities (e.g., filter by name or creation date).

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
    "dtCreated": "2024-01-15T10:30:00Z",
    "dtUpdated": null,
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": null,
    "deletedBy": null
  },
  {
    "id": 2,
    "name": "DeleteUser", 
    "dtCreated": "2024-01-15T10:35:00Z",
    "dtUpdated": null,
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": null,
    "deletedBy": null
  }
]
```

#### GET /Action/GetActionById/{id}

Retrieves a specific action by its ID.

> **🔑 Recommended Claims:** `action:read`

#### POST /Action/AddAction

Creates a new system action.

> **🔑 Recommended Claims:** `action:write`

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

> **🔑 Recommended Claims:** `action:write`

#### DELETE /Action/DeleteAction/{id}

Deletes an action from the system.

> **🔑 Recommended Claims:** `action:delete`

## Claim-Action Relationships

> **🔑 Recommended Claims:** `claimaction:read`, `claimaction:write`, `claimaction:delete`

#### GET /ClaimAction/GetClaimActions

Retrieves all mappings between claims and actions.

> **💡 Note:** Future implementations may include pagination and filtering capabilities (e.g., filter by claim or action).

**Response (200 OK):**

```json
[
  {
    "id": 1,
    "idClaim": 1,
    "idAction": 1,
    "dtCreated": "2024-01-15T10:30:00Z",
    "dtUpdated": null,
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": null,
    "deletedBy": null
  }
]
```

#### POST /ClaimAction/AddClaimAction

Maps a claim to an action, defining what actions a claim can perform.

> **🔑 Recommended Claims:** `claimaction:write`

**Request:**

```http
POST /ClaimAction/AddClaimAction
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "idClaim": 1,
  "idAction": 2
}
```

## User Permission Assignments

> **🔑 Recommended Claims:** `accountclaimaction:read`, `accountclaimaction:write`, `accountclaimaction:delete`

#### GET /AccountClaimAction/GetAccountClaimActions

Retrieves user permission assignments with optional filtering.

> **💡 Note:** Future implementations may include pagination and filtering capabilities (e.g., filter by account, claim, or action).

**Request:**

```http
GET /AccountClaimAction/GetAccountClaimActions?accountId=123
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### POST /AccountClaimAction/AddAccountClaimAction

Assigns permissions to a user account.

> **🔑 Recommended Claims:** `accountclaimaction:write`

**Request:**

```http
POST /AccountClaimAction/AddAccountClaimAction
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "idAccount": 123,
  "idClaimAction": 1
}
```

## 📝 Data Transfer Objects

> **⚠️ Important Changes (PR #124):** The handling of audit fields has been standardized across all controllers:
> - **`createdBy`** is NO LONGER included in PayLoadDTOs for update operations - it's automatically preserved
> - **`updatedBy`** is NOW included in all PayLoadDTOs as an optional field for tracking who makes updates
> - All update operations automatically ignore any `createdBy` sent in the request
> 
> For complete documentation on audit field behavior, see [Audit Fields Documentation](AUDIT_FIELDS.md).

### AccountPayLoadDTO

Used for account creation and update requests.

**For Creating Accounts:**
```json
{
  "userName": "string",
  "password": "string"
}
```

**For Updating Accounts:**
```json
{
  "id": 0,
  "userName": "string",
  "password": "string",
  "updatedBy": "string"
}
```

**Validation Rules:**
- `userName`: Required, 3-50 characters, alphanumeric and underscore only
- `password`: Required, minimum 6 characters
- `updatedBy`: Optional for updates, specifies who is making the update

**Note:** When updating, the `createdBy` field is automatically preserved and should NOT be included in the request.

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
    "id": 123,
    "userName": "example.example",
    "isActive": true,
    "dtCreated": "2024-01-15T10:30:00Z",
    "dtUpdated": "2024-01-16T14:20:00Z",
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": "manager",
    "deletedBy": null
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
| `data.id` | number | Unique user identifier |
| `data.userName` | string | User's login name |
| `data.isActive` | boolean | Whether the account is active |
| `data.dtCreated` | DateTime | When the account was created |
| `data.dtUpdated` | DateTime? | When the account was last updated |
| `data.dtDeleted` | DateTime? | When the account was deleted |
| `data.createdBy` | string | Username who created the account |
| `data.updatedBy` | string? | Username who last updated the account |
| `data.deletedBy` | string? | Username who deleted the account |

### ApplicationPayLoadDTO

Used for creating and updating applications.

**For Creating Applications:**
```json
{
  "name": "string",
  "description": "string"
}
```

**For Updating Applications:**
```json
{
  "id": 0,
  "name": "string",
  "description": "string",
  "updatedBy": "string"
}
```

**Validation Rules:**
- `name`: Required, application name
- `description`: Required, application description
- `updatedBy`: Optional for updates, specifies who is making the update

### ApplicationResponseDTO

Response object for application operations.

```json
{
  "id": "number",
  "name": "string",
  "description": "string",
  "dtCreated": "datetime",
  "dtUpdated": "datetime",
  "dtDeleted": "datetime",
  "createdBy": "string",
  "updatedBy": "string",
  "deletedBy": "string"
}
```

### ApplicationClaimPayLoadDTO

Used for mapping applications to claims.

**For Creating Application-Claim Mappings:**
```json
{
  "idApplication": "number",
  "idClaim": "number"
}
```

**For Updating Application-Claim Mappings:**
```json
{
  "id": "number",
  "idApplication": "number",
  "idClaim": "number",
  "updatedBy": "string"
}
```

**Validation Rules:**
- `idApplication`: Required, must exist in Applications table
- `idClaim`: Required, must exist in Claims table
- `updatedBy`: Optional for updates, specifies who is making the update

### ApplicationClaimResponseDTO

Response object for application-claim mappings.

```json
{
  "id": "number",
  "idApplication": "number",
  "idClaim": "number",
  "dtCreated": "datetime",
  "dtUpdated": "datetime",
  "dtDeleted": "datetime",
  "createdBy": "string",
  "updatedBy": "string",
  "deletedBy": "string"
}
```

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

Standard success response format for operations. All successful responses follow RFC 7807 structure.

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/Account/GetAccountById",
  "data": {
    "id": 123,
    "userName": "example.example",
    "isActive": true,
    "dtCreated": "2024-01-15T10:30:00Z",
    "dtUpdated": "2024-01-16T14:20:00Z",
    "dtDeleted": null,
    "createdBy": "admin",
    "updatedBy": "manager",
    "deletedBy": null
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

**For Creating Claims:**
```json
{
  "type": "Permission|Role|Feature",
  "value": "string",
  "description": "string"
}
```

**For Updating Claims:**
```json
{
  "id": 0,
  "type": "Permission|Role|Feature",
  "value": "string",
  "description": "string",
  "updatedBy": "string"
}
```

**Validation Rules:**
- `type`: Required, must be valid ClaimType enum value
- `value`: Required, unique claim value identifier
- `description`: Optional, claim description
- `updatedBy`: Optional for updates, specifies who is making the update

### ClaimResponseDTO

Response object for claim operations.

```json
{
  "id": "number",
  "type": "Permission|Role|Feature",
  "value": "string",
  "description": "string",
  "dtCreated": "datetime",
  "dtUpdated": "datetime",
  "dtDeleted": "datetime",
  "createdBy": "string",
  "updatedBy": "string",
  "deletedBy": "string"
}
```

### ActionPayLoadDTO

Used for creating and updating actions.

**For Creating Actions:**
```json
{
  "name": "string"
}
```

**For Updating Actions:**
```json
{
  "id": 0,
  "name": "string",
  "updatedBy": "string"
}
```

**Validation Rules:**
- `name`: Required, unique action name
- `updatedBy`: Optional for updates, specifies who is making the update

### ActionResponseDTO

Response object for action operations.

```json
{
  "id": "number",
  "name": "string",
  "dtCreated": "datetime",
  "dtUpdated": "datetime",
  "dtDeleted": "datetime",
  "createdBy": "string",
  "updatedBy": "string",
  "deletedBy": "string"
}
```

### ClaimActionPayLoadDTO

Used for mapping claims to actions.

**For Creating Claim-Action Mappings:**
```json
{
  "idClaim": "number",
  "idAction": "number"
}
```

**For Updating Claim-Action Mappings:**
```json
{
  "id": "number",
  "idClaim": "number",
  "idAction": "number",
  "updatedBy": "string"
}
```

**Validation Rules:**
- `idClaim`: Required, must exist in Claims table
- `idAction`: Required, must exist in Actions table
- `updatedBy`: Optional for updates, specifies who is making the update

### ClaimActionResponseDTO

Response object for claim-action mappings.

```json
{
  "id": "number",
  "idClaim": "number",
  "idAction": "number",
  "dtCreated": "datetime",
  "dtUpdated": "datetime",
  "dtDeleted": "datetime",
  "createdBy": "string",
  "updatedBy": "string",
  "deletedBy": "string"
}
```

### AccountClaimActionPayLoadDTO

Used for assigning permissions to user accounts.

**For Creating User Permission Assignments:**
```json
{
  "idAccount": "number",
  "idClaimAction": "number"
}
```

**For Updating User Permission Assignments:**
```json
{
  "id": "number",
  "idAccount": "number",
  "idClaimAction": "number",
  "updatedBy": "string"
}
```

**Validation Rules:**
- `idAccount`: Required, must exist in Accounts table
- `idClaimAction`: Required, must exist in ClaimActions table
- `updatedBy`: Optional for updates, specifies who is making the update

### AccountClaimActionResponseDTO

Response object for user permission assignments.

```json
{
  "id": "number",
  "idAccount": "number",
  "idClaimAction": "number",
  "dtCreated": "datetime",
  "dtUpdated": "datetime",
  "dtDeleted": "datetime",
  "createdBy": "string",
  "updatedBy": "string",
  "deletedBy": "string"
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
  "iss": "AuthenticationService",     // Issuer
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

> **✅ RFC 7807 Compliance:** All error responses in this API strictly follow RFC 7807 (Problem Details for HTTP APIs) standard. This ensures consistent, machine-readable error responses across all endpoints, making error handling predictable and easy to implement in client applications.

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

**RFC 7807 Fields:**

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `type` | string | ✅ | URI reference that identifies the problem type |
| `title` | string | ✅ | Short, human-readable summary of the problem |
| `status` | integer | ✅ | HTTP status code |
| `detail` | string | ✅ | Human-readable explanation specific to this occurrence |
| `instance` | string | ✅ | URI reference to the specific occurrence of the problem |
| `errors` | object | ❌ | Optional validation errors (field-level details) |

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

## 🧪 Development & Testing Endpoints

### ExampleController

> **⚠️ Note:** The `ExampleController` provides sample endpoints for development and testing purposes. These endpoints are not intended for production use and may be disabled or removed in production environments. They serve as:
> - Reference implementations for development teams
> - Testing endpoints for integration tests
> - Examples of proper controller structure and error handling

For more information on testing, see the [Detailed Test Documentation](../tests/DETAILED_TEST_DOCUMENTATION.md).

## 📚 SDKs and Client Libraries

Currently, the API is accessible through standard HTTP clients. For production usage:

- Use the **Swagger/OpenAPI** specification available at `https://localhost:7001/swagger` to generate client libraries for your preferred language
- Most modern languages have OpenAPI code generators available (e.g., `openapi-generator`, `NSwag`, `AutoRest`)
- The API follows REST principles and uses standard JSON request/response formats

## 🔄 Versioning

The API currently uses **version 1 (v1)** and follows semantic versioning principles:

- **Current Version**: v1
- **Stability**: The v1 API is stable and maintains backward compatibility
- **Breaking Changes**: Will be introduced in v2 when needed
- **Deprecation Policy**: Features will be marked deprecated for at least one major version before removal

The API version is implicitly v1. Future versions will be clearly indicated when introduced.

## 📞 Support

For API support and questions:

- **Documentation**: [API Docs](../../docs/)
- **Issues**: [GitHub Issues](../../issues)
- **Support Email**: api-support@yourdomain.com
- **Developer Forum**: [Community Forum](https://forum.yourdomain.com)