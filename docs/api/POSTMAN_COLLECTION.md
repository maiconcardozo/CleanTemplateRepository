# 📬 Postman Collection - Authentication API

## 📋 Overview

This document explains how to import and use the Postman collection to test all endpoints of the Authentication API. The collection includes endpoints organized by controller with example payloads and automatic authentication configuration.

## 📥 Importing the Collection

### Step 1: Download the File

The collection is available at:
- **File**: [`Authentication_API.postman_collection.json`](Authentication_API.postman_collection.json)
- **Location**: `docs/api/Authentication_API.postman_collection.json`

### Step 2: Import into Postman

1. Open **Postman**
2. Click **Import** in the upper left corner
3. Select the file `Authentication_API.postman_collection.json`
4. Click **Import** to finish

## ⚙️ Configuration

### Collection Variables

The collection uses variables to simplify usage across different environments:

| Variable   | Default Value           | Description              |
|------------|------------------------|--------------------------|
| `baseUrl`  | `https://localhost:7001` | API base URL             |
| `authToken`| *(empty)*              | JWT token (auto-filled)  |

### Changing the Base URL

To use the collection in another environment:

1. Right-click the "Authentication API - Complete Collection"
2. Select **Edit**
3. Go to the **Variables** tab
4. Change the value of `baseUrl` to your desired address (e.g., `https://api.production.com`)
5. Click **Save**

## 🚀 Using the Collection

### 1. Authentication (Generate Token)

**Endpoint**: `POST /Authentication/GenerateToken`

The first step is always to generate an authentication token:

1. Expand the **Authentication** folder
2. Select **Generate Token**
3. Adjust the credentials in the body if necessary:
```json
{
  "userName": "admin",
  "password": "AdminPassword123!"
}
```
4. Click **Send**
5. The token will be **automatically saved** to the `authToken` variable

> ⚡ **Automation**: The request contains a test script that automatically saves the returned token to the `authToken` variable, so you do not need to copy and paste manually.

### 2. Using Authenticated Endpoints

All other endpoints in the collection are already configured to use the token automatically via the header:

```
Authorization: Bearer {{authToken}}
```

Just run any endpoint after generating the token!

## 📚 Collection Structure

The collection is organized into 8 endpoint groups:

### 1. 🔐 Authentication
Authentication and JWT token generation.
- `POST /Authentication/GenerateToken` - Generate JWT token

### 2. 👤 Account
User account management.
- `GET /Account/GetAccounts` - List all accounts
- `GET /Account/GetAccountById?id={id}` - Get account by ID
- `POST /Account/AddAccount` - Create new account
- `PUT /Account/UpdateAccount` - Update existing account
- `DELETE /Account/DeleteAccount?id={id}` - Delete account

### 3. 🏷️ Claim
Claim management (permissions based on types and values).
- `GET /Claim/GetClaims` - List all claims
- `GET /Claim/GetClaimById?id={id}` - Get claim by ID
- `POST /Claim/AddClaim` - Create new claim
- `PUT /Claim/UpdateClaim` - Update existing claim
- `DELETE /Claim/DeleteClaim?id={id}` - Delete claim

### 4. ⚡ Action
Action management (available actions/permissions in the system).
- `GET /Action/GetActions` - List all actions
- `GET /Action/GetActionById?id={id}` - Get action by ID
- `POST /Action/AddAction` - Create new action
- `PUT /Action/UpdateAction` - Update existing action
- `DELETE /Action/DeleteAction?id={id}` - Delete action

### 5. 🔗 ClaimAction
Associations between Claims and Actions.
- `GET /ClaimAction/GetClaimActions` - List all associations
- `GET /ClaimAction/GetClaimActionById?id={id}` - Get association by ID
- `POST /ClaimAction/AddClaimAction` - Create new association
- `PUT /ClaimAction/UpdateClaimAction` - Update existing association
- `DELETE /ClaimAction/DeleteClaimAction?id={id}` - Delete association

### 6. 👥 AccountClaimAction
Permissions assigned to users.
- `GET /AccountClaimAction/GetAccountClaimActions` - List permissions (with optional filters)
- `GET /AccountClaimAction/GetAccountClaimActionById` - Get specific permission
- `POST /AccountClaimAction/AddAccountClaimAction` - Assign permission to user
- `PUT /AccountClaimAction/UpdateAccountClaimAction` - Update permission
- `DELETE /AccountClaimAction/DeleteAccountClaimAction` - Remove permission

### 7. 🏢 Application
Application management (multi-tenant discrimination).
- `GET /Application/GetApplications` - List all applications
- `GET /Application/GetApplicationById?id={id}` - Get application by ID
- `POST /Application/AddApplication` - Create new application
- `PUT /Application/UpdateApplication` - Update existing application
- `DELETE /Application/DeleteApplication?id={id}` - Delete application

### 8. 🔐 ApplicationClaim
Associations between Applications and Claims.
- `GET /ApplicationClaim/GetApplicationClaims` - List all associations
- `GET /ApplicationClaim/GetApplicationClaimById?id={id}` - Get association by ID
- `GET /ApplicationClaim/GetApplicationClaimsByApplicationId?applicationId={id}` - Get claims by application
- `POST /ApplicationClaim/AddApplicationClaim` - Create new association
- `PUT /ApplicationClaim/UpdateApplicationClaim` - Update existing association
- `DELETE /ApplicationClaim/DeleteApplicationClaim?id={id}` - Delete association

## 💡 Usage Examples

### Example 1: Create a New User

1. Generate a token using **Authentication > Generate Token**
2. Open **Account > Add Account**
3. Modify the body as needed:
```json
{
  "id": 0,
  "userName": "john.doe",
  "password": "SecurePass123!",
  "email": "john.doe@example.com"
}
```
4. Click **Send**

### Example 2: Set Up RBAC Permissions

1. **Create a Claim**: Use **Claim > Add Claim**
```json
{
  "id": 0,
  "type": "Role",
  "value": "Manager",
  "description": "Manager role"
}
```

2. **Create an Action**: Use **Action > Add Action**
```json
{
  "id": 0,
  "name": "ManageUsers",
  "description": "Permission to manage users"
}
```

3. **Associate Claim with Action**: Use **ClaimAction > Add ClaimAction**
```json
{
  "id": 0,
  "idClaim": 1,
  "idAction": 1
}
```

4. **Assign to User**: Use **AccountClaimAction > Add AccountClaimAction**
```json
{
  "id": 0,
  "idAccount": 1,
  "idClaimAction": 1
}
```

### Example 3: Set Up Multi-Tenant

1. **Create Application**: Use **Application > Add Application**
```json
{
  "id": 0,
  "name": "WebPortal",
  "description": "Main web portal"
}
```

2. **Associate Claims**: Use **ApplicationClaim > Add ApplicationClaim**
```json
{
  "id": 0,
  "idApplication": 1,
  "idClaim": 1
}
```

## 🔍 Tips & Tricks

### 1. Request Organization
- Use **folders** in Postman to organize different test scenarios
- Duplicate requests to create variations (e.g., "Add Account - Admin", "Add Account - User")

### 2. Test Scripts
The **Generate Token** request already includes a script that:
- Checks for status code 200
- Extracts the token from the response
- Automatically saves it in the `authToken` variable

You can add similar scripts to other requests to automate your workflow.

### 3. Environments
For working with multiple environments (dev, staging, prod):
1. Create **Environments** in Postman
2. Define the `baseUrl` variable for each environment
3. Switch between environments as needed

### 4. Collections Runner
To run batch tests:
1. Click the collection menu (...)
2. Select **Run collection**
3. Choose the requests to execute
4. Set iterations and delays
5. Click **Run Authentication API**

## 🐛 Troubleshooting

### Error: 401 Unauthorized
- **Cause**: Expired or invalid token
- **Solution**: Re-run **Generate Token** to get a new token

### Error: 400 Bad Request
- **Cause**: Invalid payload or missing required fields
- **Solution**: Check the request body and API documentation

### Error: 404 Not Found
- **Cause**: Endpoint not found or non-existent ID
- **Solution**: Check if the URL is correct and if the resource exists

### Connection Error
- **Cause**: API not running or incorrect URL
- **Solution**: 
  - Check if the API is running at `https://localhost:7001`
  - Check the `baseUrl` variable in the collection
  - For local HTTPS, disable SSL verification: Settings > General > SSL certificate verification = OFF

## 📖 Additional Resources

### Related Documentation
- [API Documentation](API.md) - Complete API documentation
- [Endpoints Reference](ENDPOINTS.md) - Details on all endpoints
- [RBAC Guide](RBAC.md) - Role-based access control guide
- [Testing Guide](../guides/TESTING.md) - Testing strategies

### Useful Links
- [Postman Documentation](https://learning.postman.com/docs/getting-started/introduction/)
- [Postman Collection Format](https://schema.postman.com/json/collection/v2.1.0/docs/index.html)
- [JWT.io](https://jwt.io/) - For decoding and inspecting JWT tokens

## 🤝 Contributing

Found an issue or have suggestions to improve the collection?
- Open an issue on GitHub
- Suggest new examples or test scenarios
- Contribute automation scripts

---

**Version**: 1.0.0  
**Last Update**: 2025-10-13  
**Compatible with**: Authentication API v1.x
