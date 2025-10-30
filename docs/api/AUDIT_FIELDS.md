# 📝 Audit Fields Documentation

## Overview

This document describes how audit fields (`CreatedBy`, `UpdatedBy`, `DeletedBy`, `DtCreated`, `DtUpdated`, `DtDeleted`) are handled in the Authentication API, including recent changes from PR #124.

## Audit Fields in API Operations

### Response DTOs

All response DTOs include complete audit information:

```json
{
  "id": 1,
  "name": "Example",
  "dtCreated": "2024-01-15T10:30:00Z",
  "dtUpdated": "2024-01-16T14:20:00Z",
  "dtDeleted": null,
  "createdBy": "admin",
  "updatedBy": "manager",
  "deletedBy": null
}
```

**Response Fields:**

| Field | Type | Description |
|-------|------|-------------|
| `dtCreated` | DateTime | Timestamp when the entity was created |
| `dtUpdated` | DateTime? | Timestamp when the entity was last updated (null if never updated) |
| `dtDeleted` | DateTime? | Timestamp when the entity was deleted (null if not deleted) |
| `createdBy` | string | Username of the user who created the entity |
| `updatedBy` | string? | Username of the user who last updated the entity (null if never updated) |
| `deletedBy` | string? | Username of the user who deleted the entity (null if not deleted) |

### PayLoad DTOs (Create Operations)

When **creating** an entity, you don't need to provide audit fields:

```json
{
  "name": "New Entity"
}
```

**Behavior:**
- `createdBy` is automatically set by the system based on the authenticated user
- `dtCreated` is automatically set to the current timestamp
- Other audit fields are initialized as null

### PayLoad DTOs (Update Operations)

When **updating** an entity, you can optionally provide the `updatedBy` field:

```json
{
  "id": 1,
  "name": "Updated Entity",
  "updatedBy": "manager"
}
```

**Behavior (PR #124 Changes):**
- `updatedBy` is **optional** - if provided, it will be set to the specified value
- `updatedBy` can be omitted - the system will handle it based on your business logic
- `dtUpdated` is automatically set to the current timestamp
- **`createdBy` is NEVER sent in update payloads** - the original creator is always preserved
- The system automatically ignores any `createdBy` field in the payload to ensure data integrity

## PR #124: Update Operations Fix

### What Changed

**Issue:** Previously, the handling of `CreatedBy` and `UpdatedBy` fields during update operations was inconsistent across controllers.

**Solution:** PR #124 standardized the behavior across all controllers:

1. **Removed `CreatedBy` from all PayLoadDTOs** - This field is only set during creation and should never be modified
2. **Added `UpdatedBy` to all PayLoadDTOs** - API consumers can now specify who is making the update
3. **Updated AutoMapper configuration** - Mappings now explicitly ignore `CreatedBy` when converting PayLoadDTOs to domain entities

### Affected Controllers

All controllers with update operations were standardized:

- ✅ **AccountController** - `PUT /Account/UpdateAccount`
- ✅ **ActionController** - `PUT /Action/UpdateAction`
- ✅ **ApplicationController** - `PUT /Application/UpdateApplication`
- ✅ **ClaimController** - `PUT /Claim/UpdateClaim`
- ✅ **ClaimActionController** - `PUT /ClaimAction/UpdateClaimAction`
- ✅ **ApplicationClaimController** - `PUT /ApplicationClaim/UpdateApplicationClaim`
- ✅ **AccountClaimActionController** - `PUT /AccountClaimAction/UpdateAccountClaimAction`

### Migration Guide for API Consumers

If you were previously sending `createdBy` in update requests:

**Before PR #124 (Not Recommended):**
```json
{
  "id": 1,
  "name": "Updated Entity",
  "createdBy": "admin",
  "updatedBy": "manager"
}
```

**After PR #124 (Correct):**
```json
{
  "id": 1,
  "name": "Updated Entity",
  "updatedBy": "manager"
}
```

**Impact:** 
- Sending `createdBy` in update requests will be **ignored** by the API
- No breaking changes - existing update requests will continue to work
- The original `createdBy` value is always preserved automatically

## Examples by Entity Type

### Account Update Example

**Request:**
```bash
curl -X PUT "https://localhost:7001/Account/UpdateAccount" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 123,
    "userName": "updateduser",
    "password": "newpassword123",
    "updatedBy": "admin"
  }'
```

**Response:**
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
    "dtUpdated": "2024-01-20T15:45:00Z",
    "dtDeleted": null,
    "createdBy": "initialadmin",
    "updatedBy": "admin",
    "deletedBy": null
  }
}
```

**Note:** The `createdBy` field remains "initialadmin" (the original creator), while `updatedBy` is set to "admin" (who made the update).

### Action Update Example

**Request:**
```bash
curl -X PUT "https://localhost:7001/Action/UpdateAction" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 5,
    "name": "UpdatedAction",
    "updatedBy": "systemadmin"
  }'
```

**Response:**
```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/Action/UpdateAction",
  "data": {
    "id": 5,
    "name": "UpdatedAction",
    "dtCreated": "2024-01-10T08:00:00Z",
    "dtUpdated": "2024-01-20T16:30:00Z",
    "dtDeleted": null,
    "createdBy": "developer",
    "updatedBy": "systemadmin",
    "deletedBy": null
  }
}
```

### Application Update Example

**Request:**
```bash
curl -X PUT "https://localhost:7001/Application/UpdateApplication" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 2,
    "name": "UpdatedApp",
    "description": "Updated application description",
    "updatedBy": "appadmin"
  }'
```

**Response:**
```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/Application/UpdateApplication",
  "data": {
    "id": 2,
    "name": "UpdatedApp",
    "description": "Updated application description",
    "dtCreated": "2024-01-05T12:00:00Z",
    "dtUpdated": "2024-01-20T17:00:00Z",
    "dtDeleted": null,
    "createdBy": "projectmanager",
    "updatedBy": "appadmin",
    "deletedBy": null
  }
}
```

### Claim Update Example

**Request:**
```bash
curl -X PUT "https://localhost:7001/Claim/UpdateClaim" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 3,
    "type": "Permission",
    "value": "users:write",
    "description": "Updated permission to write user data",
    "updatedBy": "securityadmin"
  }'
```

**Response:**
```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/Claim/UpdateClaim",
  "data": {
    "id": 3,
    "type": "Permission",
    "value": "users:write",
    "description": "Updated permission to write user data",
    "dtCreated": "2024-01-12T09:00:00Z",
    "dtUpdated": "2024-01-20T18:15:00Z",
    "dtDeleted": null,
    "createdBy": "securityteam",
    "updatedBy": "securityadmin",
    "deletedBy": null
  }
}
```

### ClaimAction Update Example

**Request:**
```bash
curl -X PUT "https://localhost:7001/ClaimAction/UpdateClaimAction" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 7,
    "idClaim": 3,
    "idAction": 5,
    "updatedBy": "rbacadmin"
  }'
```

**Response:**
```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc7231#section-6.3.1",
  "title": "OK.",
  "status": 200,
  "detail": "Request was successful.",
  "instance": "/ClaimAction/UpdateClaimAction",
  "data": {
    "id": 7,
    "idClaim": 3,
    "idAction": 5,
    "dtCreated": "2024-01-14T11:30:00Z",
    "dtUpdated": "2024-01-20T19:00:00Z",
    "dtDeleted": null,
    "createdBy": "securityteam",
    "updatedBy": "rbacadmin",
    "deletedBy": null
  }
}
```

## Complete PayLoad DTO Reference

### AccountPayLoadDTO
```json
{
  "id": 0,
  "userName": "string",
  "password": "string",
  "updatedBy": "string"  // Optional for updates
}
```

### ActionPayLoadDTO
```json
{
  "id": 0,
  "name": "string",
  "updatedBy": "string"  // Optional for updates
}
```

### ApplicationPayLoadDTO
```json
{
  "id": 0,
  "name": "string",
  "description": "string",
  "updatedBy": "string"  // Optional for updates
}
```

### ClaimPayLoadDTO
```json
{
  "id": 0,
  "type": "Permission|Role|Feature",
  "value": "string",
  "description": "string",
  "updatedBy": "string"  // Optional for updates
}
```

### ClaimActionPayLoadDTO
```json
{
  "id": 0,
  "idClaim": 0,
  "idAction": 0,
  "updatedBy": "string"  // Optional for updates
}
```

### ApplicationClaimPayLoadDTO
```json
{
  "id": 0,
  "idApplication": 0,
  "idClaim": 0,
  "updatedBy": "string"  // Optional for updates
}
```

### AccountClaimActionPayLoadDTO
```json
{
  "id": 0,
  "idAccount": 0,
  "idClaimAction": 0,
  "updatedBy": "string"  // Optional for updates
}
```

## Best Practices

### ✅ Do's

1. **Always omit `createdBy` in update requests** - It's automatically preserved
2. **Include `updatedBy` when you want to track who made the change** - This is optional but recommended
3. **Check response DTOs for complete audit trail** - All audit fields are included in responses
4. **Use consistent user identifiers** - Use the same format for `updatedBy` across your application

### ❌ Don'ts

1. **Don't try to modify `createdBy` in updates** - It will be ignored
2. **Don't rely on client-side timestamps** - All timestamps are set server-side
3. **Don't send audit fields in create operations** - They're set automatically

## Troubleshooting

### Issue: `createdBy` is not changing in updates

**Solution:** This is expected behavior after PR #124. The `createdBy` field is preserved from the original creation and cannot be modified through update operations.

### Issue: `updatedBy` is not being set

**Solution:** Ensure you're including the `updatedBy` field in your update payload:
```json
{
  "id": 1,
  "name": "Updated",
  "updatedBy": "username"  // Add this field
}
```

### Issue: Receiving validation errors about audit fields

**Solution:** Remove `createdBy`, `dtCreated`, `dtUpdated`, and other audit fields from your request payload. Only include `updatedBy` if needed.

## Related Documentation

- [API Reference](API.md) - Complete API documentation
- [Examples](EXAMPLES.md) - Practical integration examples
- [Endpoints Reference](ENDPOINTS.md) - All available endpoints
- [RBAC Guide](RBAC.md) - Role-Based Access Control guide
