# Swagger API Documentation Configuration

## Overview
The Authentication API project has been configured to separate Swagger documentation into two distinct API definitions with comprehensive status code examples and error response documentation.

## Enhanced Documentation Features (PR #40)

The Swagger documentation has been significantly enhanced to include:

### Status Code Documentation
- **409 Conflict**: Comprehensive examples for duplicate username scenarios
- **404 Not Found**: Examples for resource not found scenarios
- **401 Unauthorized**: Examples for authentication failures
- **400 Bad Request**: Enhanced validation error examples
- **500 Internal Server Error**: Server error examples

### Response Examples
- **ProblemDetailsConflictExample**: RFC 7807 compliant conflict responses
- **Enhanced validation examples**: Detailed field-level validation errors
- **Standardized error format**: Consistent error response structure across all endpoints

### Internationalization Support
- Error messages in English and Portuguese
- Localized response examples in Swagger documentation

### 1. Authentication API
- **Definition Name**: `Authentication`
- **Endpoint**: `/swagger/Authentication/swagger.json`
- **Display Name**: "Authentication API"
- **Description**: "API endpoints for user authentication and token management"
- **Controllers**: AuthenticationController
- **Endpoints**: `/Authentication/*`

### 2. Access Control API
- **Definition Name**: `AccessControl`
- **Endpoint**: `/swagger/AccessControl/swagger.json`
- **Display Name**: "Access Control API"
- **Description**: "API endpoints for managing access control including accounts, claims, and actions"
- **Controllers**: 
  - AccountController (with enhanced 409 Conflict handling)
  - AccountClaimActionController
  - ActionController
  - ClaimActionController
  - ClaimController
- **Endpoints**: `/Account/*`, `/AccountClaimAction/*`, `/Action/*`, `/ClaimAction/*`, `/Claim/*`
- **Enhanced Features**: Comprehensive status code documentation with examples for all CRUD operations

## Configuration Files Modified

### ApplicationConstants.cs (Authentication.Login project)
Added `SwaggerDefinitions` nested class with constants for:
- Definition names
- Endpoint URLs
- Display names

### Program.cs (Authentication.API project)
Modified SwaggerGen configuration to:
- Define two separate SwaggerDoc instances
- Implement DocInclusionPredicate logic to filter controllers by definition
- Update SwaggerUI to display both definitions in dropdown

## Usage
1. Run the application
2. Navigate to the root URL (protected by basic auth: admin/senha123)
3. Use the dropdown in Swagger UI to switch between "Authentication API" and "Access Control API"

## Testing
Run the verification script to ensure proper separation:
```bash
./verify_swagger_separation.sh
```

## Future Maintenance
- To add new controllers to Authentication API: Update the DocInclusionPredicate in Program.cs
- To add new controllers to AccessControl API: Update the DocInclusionPredicate in Program.cs
- To modify display names or descriptions: Update constants in ApplicationConstants.cs