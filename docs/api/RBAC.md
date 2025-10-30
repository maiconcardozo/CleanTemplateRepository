# 🔐 RBAC System - Role-Based Access Control Guide

## Overview

The Authentication Service implements a comprehensive Role-Based Access Control (RBAC) system that allows fine-grained permission management through Claims, Actions, and their mappings.

## RBAC Components

### 1. Claims
**Claims** represent permissions or roles in the system.

- **Type**: The category of the claim (e.g., "Permission", "Role")
- **Value**: The actual permission value (e.g., "user:manage", "admin:full")
- **Description**: Human-readable description of the claim

**Example:**
```json
{
  "type": "Permission",
  "value": "user:manage",
  "description": "Manage users"
}
```

### 2. Actions
**Actions** represent operations that can be performed in the system.

- **Name**: The action identifier (e.g., "CreateUser", "DeleteUser", "ReadReport")
- **Description**: What the action does

**Example:**
```json
{
  "name": "CreateUser",
  "description": "Create a new user account"
}
```

### 3. ClaimAction
**ClaimAction** maps which claims allow which actions.

- **ClaimId**: Reference to a claim
- **ActionId**: Reference to an action

This mapping defines: "Claim X allows Action Y"

### 4. AccountClaimAction
**AccountClaimAction** assigns permissions to specific users.

- **AccountId**: Reference to a user account
- **ClaimActionId**: Reference to a claim-action mapping

This assignment defines: "User X has permission Y (which allows actions Z)"

## RBAC Flow

```
User (Account)
    ↓ (assigned)
AccountClaimAction (user permission)
    ↓ (references)
ClaimAction (permission-action mapping)
    ↓ (allows)
Action (operation)
```

## Step-by-Step Setup Guide

### Step 1: Authenticate

First, authenticate to get a JWT token:

```bash
curl -X POST "https://localhost:7001/Authentication/GenerateToken" \
  -H "Content-Type: application/json" \
  -d '{"userName": "admin", "password": "password123"}'
```

**Response:**
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

Save the `accessToken` for subsequent requests.

### Step 2: Create a Claim

Create a permission claim:

```bash
curl -X POST "https://localhost:7001/Claim/AddClaim" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "type": "Permission",
    "value": "user:manage",
    "description": "Manage users"
  }'
```

**Response:**
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

### Step 3: Create an Action

Create an action that represents an operation:

```bash
curl -X POST "https://localhost:7001/Action/AddAction" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "CreateUser",
    "description": "Create a new user account"
  }'
```

**Response:**
```json
{
  "data": {
    "id": 1,
    "name": "CreateUser",
    "description": "Create a new user account"
  }
}
```

### Step 4: Map Claim to Action

Link the claim to the action (define that the claim allows the action):

```bash
curl -X POST "https://localhost:7001/ClaimAction/AddClaimAction" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "claimId": 1,
    "actionId": 1
  }'
```

**Response:**
```json
{
  "data": {
    "id": 1,
    "claimId": 1,
    "actionId": 1
  }
}
```

### Step 5: Assign Permission to User

Finally, assign the permission to a specific user:

```bash
curl -X POST "https://localhost:7001/AccountClaimAction/AddAccountClaimAction" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "accountId": 123,
    "claimActionId": 1
  }'
```

**Response:**
```json
{
  "data": {
    "accountId": 123,
    "claimActionId": 1
  }
}
```

## Permission Verification Flow

When a user attempts to perform an action:

1. **User logs in** → Receives JWT token with user ID
2. **System checks permissions** → Queries `AccountClaimAction` for the user
3. **Action validation** → Verifies if any of the user's claims allow the desired action
4. **Authorized execution** → If permission exists, user can execute the operation
5. **Denied execution** → If no permission, return 403 Forbidden

## Common RBAC Scenarios

### Scenario 1: Administrator with Full Access

```bash
# 1. Create admin claim
curl -X POST "https://localhost:7001/Claim/AddClaim" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"type": "Role", "value": "admin:full", "description": "Full administrator"}'

# 2. Create all necessary actions
# CreateUser, DeleteUser, UpdateUser, ReadUser, etc.

# 3. Map admin claim to all actions
# For each action, create a ClaimAction mapping

# 4. Assign admin claim to user
curl -X POST "https://localhost:7001/AccountClaimAction/AddAccountClaimAction" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"accountId": 1, "claimActionId": <admin-claim-action-id>}'
```

### Scenario 2: Read-Only User

```bash
# 1. Create read-only claim
curl -X POST "https://localhost:7001/Claim/AddClaim" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"type": "Permission", "value": "user:read", "description": "Read users"}'

# 2. Create read action
curl -X POST "https://localhost:7001/Action/AddAction" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"name": "ReadUser", "description": "Read user information"}'

# 3. Map claim to read action only
curl -X POST "https://localhost:7001/ClaimAction/AddClaimAction" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"claimId": <claim-id>, "actionId": <action-id>}'

# 4. Assign to user
curl -X POST "https://localhost:7001/AccountClaimAction/AddAccountClaimAction" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"accountId": <user-id>, "claimActionId": <claim-action-id>}'
```

### Scenario 3: Department-Specific Permissions

```bash
# 1. Create department-specific claims
curl -X POST "https://localhost:7001/Claim/AddClaim" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"type": "Permission", "value": "hr:manage", "description": "Manage HR data"}'

curl -X POST "https://localhost:7001/Claim/AddClaim" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"type": "Permission", "value": "finance:manage", "description": "Manage finance data"}'

# 2. Create department-specific actions
# HR actions: ManageEmployee, ProcessPayroll
# Finance actions: ProcessInvoice, GenerateReport

# 3. Map department claims to respective actions

# 4. Assign to users based on their department
```

## Best Practices

### 1. Claim Naming Convention

Use a consistent naming convention for claims:

```
<resource>:<action>
```

Examples:
- `user:read` - Read user data
- `user:write` - Create/update user data
- `user:delete` - Delete user data
- `report:generate` - Generate reports
- `admin:full` - Full administrator access

### 2. Granular Permissions

Create granular permissions instead of broad ones:

**Good:**
```json
{ "value": "user:create" }
{ "value": "user:update" }
{ "value": "user:delete" }
```

**Avoid:**
```json
{ "value": "user:all" }
```

### 3. Use Roles for Common Permission Sets

Create role-based claims that group common permissions:

```json
{
  "type": "Role",
  "value": "user_manager",
  "description": "Standard user manager role"
}
```

Then map this role to multiple actions: Create, Update, Read users.

### 4. Separate Admin Permissions

Keep administrative permissions separate from regular permissions:

```json
{ "value": "admin:users" }
{ "value": "admin:system" }
{ "value": "admin:security" }
```

### 5. Document Permissions

Always provide clear descriptions for claims and actions:

```json
{
  "type": "Permission",
  "value": "invoice:approve",
  "description": "Approve invoices up to $10,000"
}
```

## Checking User Permissions

To check if a user has a specific permission:

```bash
# Get all permissions for a user
curl -X GET "https://localhost:7001/AccountClaimAction/GetAccountClaimActions?accountId=123" \
  -H "Authorization: Bearer <token>"
```

**Response:**
```json
{
  "data": [
    {
      "accountId": 123,
      "claimActionId": 1,
      "claim": {
        "type": "Permission",
        "value": "user:manage"
      },
      "action": {
        "name": "CreateUser"
      }
    }
  ]
}
```

## Revoking Permissions

To remove a permission from a user:

```bash
curl -X DELETE "https://localhost:7001/AccountClaimAction/DeleteAccountClaimAction/{accountId}/{claimActionId}" \
  -H "Authorization: Bearer <token>"
```

## Updating Permissions

To update a user's permissions:

1. Remove old permission (DELETE AccountClaimAction)
2. Add new permission (POST AccountClaimAction)

Or:

1. Update the ClaimAction mapping to point to different actions
2. User automatically gets updated permissions

## Security Considerations

### 1. Protect RBAC Endpoints

Ensure that only administrators can access RBAC management endpoints:
- `/Claim/*`
- `/Action/*`
- `/ClaimAction/*`
- `/AccountClaimAction/*`

### 2. Validate Permission Hierarchy

Implement logic to prevent users from granting permissions they don't have.

### 3. Audit Permission Changes

Log all permission changes for security auditing:
- Who made the change
- What was changed
- When it was changed

### 4. Regular Permission Review

Periodically review and clean up:
- Unused claims
- Orphaned ClaimActions
- Users with excessive permissions

## Troubleshooting

### Issue: User Has Permission But Can't Perform Action

Check:
1. User is authenticated with valid JWT token
2. AccountClaimAction exists for the user
3. ClaimAction mapping is correct
4. Action ID matches the operation being performed

### Issue: Permission Changes Not Taking Effect

- JWT tokens contain permissions at generation time
- User must log out and log back in to get updated permissions
- Or implement refresh token mechanism

### Issue: Too Many Permission Combinations

- Review claim structure
- Consider consolidating similar permissions
- Use role-based claims for common permission sets

## Related Documentation

- [API Endpoints](ENDPOINTS.md) - Detailed endpoint documentation
- [Examples](EXAMPLES.md) - Practical integration examples
- [Security Guide](../architecture/SECURITY.md) - Security best practices
- [API Reference](API.md) - Complete API documentation
