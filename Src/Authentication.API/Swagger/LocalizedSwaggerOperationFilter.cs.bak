using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Authentication.API.Resource;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

public class LocalizedSwaggerOperationFilter : IOperationFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalizedSwaggerOperationFilter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        switch (context.MethodInfo.Name)
        {
            case "GenerateToken":
                operation.Summary = ResourceAPI.GeneratesJWTToken;
                operation.Description = ResourceAPI.DocumentationGenerateToken;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.TokenGeneratedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredTokenCouldGenerated);
                break;

            case "AddAccount":
                operation.Summary = ResourceAPI.AddAccount;
                operation.Description = ResourceAPI.DocumentationAddAccount;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.AccountCreatedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredAccountCouldInserted);
                break;

            // Account Controller methods
            case "GetAccounts":
                operation.Summary = ResourceAPI.GetAccounts;
                operation.Description = ResourceAPI.DocumentationGetAccounts;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.AccountsRetrievedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredAccountsCouldNotBeRetrieved);
                break;

            case "GetAccountById":
                operation.Summary = ResourceAPI.GetAccountById;
                operation.Description = ResourceAPI.DocumentationGetAccountById;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.AccountRetrievedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.AccountNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredAccountCouldNotBeRetrieved);
                break;

            case "UpdateAccount":
                operation.Summary = ResourceAPI.UpdateAccount;
                operation.Description = ResourceAPI.DocumentationUpdateAccount;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.AccountUpdatedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.AccountNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredAccountCouldNotBeUpdated);
                break;

            case "DeleteAccount":
                operation.Summary = ResourceAPI.DeleteAccount;
                operation.Description = ResourceAPI.DocumentationDeleteAccount;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.AccountDeletedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.AccountNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredAccountCouldNotBeDeleted);
                break;

            // Action Controller methods
            case "GetActions":
                operation.Summary = ResourceAPI.GetActions;
                operation.Description = ResourceAPI.DocumentationGetActions;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ActionsRetrievedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredActionsCouldNotBeRetrieved);
                break;

            case "GetActionById":
                operation.Summary = ResourceAPI.GetActionById;
                operation.Description = ResourceAPI.DocumentationGetActionById;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ActionRetrievedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.ActionNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredActionCouldNotBeRetrieved);
                break;

            case "AddAction":
                operation.Summary = ResourceAPI.AddAction;
                operation.Description = ResourceAPI.DocumentationAddAction;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ActionCreatedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredActionCouldNotBeInserted);
                break;

            case "UpdateAction":
                operation.Summary = ResourceAPI.UpdateAction;
                operation.Description = ResourceAPI.DocumentationUpdateAction;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ActionUpdatedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.ActionNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredActionCouldNotBeUpdated);
                break;

            case "DeleteAction":
                operation.Summary = ResourceAPI.DeleteAction;
                operation.Description = ResourceAPI.DocumentationDeleteAction;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ActionDeletedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.ActionNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredActionCouldNotBeDeleted);
                break;

            // Claim Controller methods
            case "GetClaims":
                operation.Summary = ResourceAPI.GetClaims;
                operation.Description = ResourceAPI.DocumentationGetClaims;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ClaimsRetrievedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredClaimsCouldNotBeRetrieved);
                break;

            case "GetClaimById":
                operation.Summary = ResourceAPI.GetClaimById;
                operation.Description = ResourceAPI.DocumentationGetClaimById;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ClaimRetrievedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.ClaimNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredClaimCouldNotBeRetrieved);
                break;

            case "AddClaim":
                operation.Summary = ResourceAPI.AddClaim;
                operation.Description = ResourceAPI.DocumentationAddClaim;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ClaimCreatedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredClaimCouldNotBeInserted);
                break;

            case "UpdateClaim":
                operation.Summary = ResourceAPI.UpdateClaim;
                operation.Description = ResourceAPI.DocumentationUpdateClaim;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ClaimUpdatedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.ClaimNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredClaimCouldNotBeUpdated);
                break;

            case "DeleteClaim":
                operation.Summary = ResourceAPI.DeleteClaim;
                operation.Description = ResourceAPI.DocumentationDeleteClaim;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ClaimDeletedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.ClaimNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredClaimCouldNotBeDeleted);
                break;

            // Claim Action Controller methods
            case "GetClaimActions":
                operation.Summary = ResourceAPI.GetClaimActions;
                operation.Description = ResourceAPI.DocumentationGetClaimActions;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ClaimActionsRetrievedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredClaimActionsCouldNotBeRetrieved);
                break;

            case "GetClaimActionById":
                operation.Summary = ResourceAPI.GetClaimActionById;
                operation.Description = ResourceAPI.DocumentationGetClaimActionById;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ClaimActionRetrievedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.ClaimActionNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredClaimActionCouldNotBeRetrieved);
                break;

            case "AddClaimAction":
                operation.Summary = ResourceAPI.AddClaimAction;
                operation.Description = ResourceAPI.DocumentationAddClaimAction;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ClaimActionCreatedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredClaimActionCouldNotBeInserted);
                break;

            case "UpdateClaimAction":
                operation.Summary = ResourceAPI.UpdateClaimAction;
                operation.Description = ResourceAPI.DocumentationUpdateClaimAction;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ClaimActionUpdatedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.ClaimActionNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredClaimActionCouldNotBeUpdated);
                break;

            case "DeleteClaimAction":
                operation.Summary = ResourceAPI.DeleteClaimAction;
                operation.Description = ResourceAPI.DocumentationDeleteClaimAction;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.ClaimActionDeletedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.ClaimActionNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredClaimActionCouldNotBeDeleted);
                break;

            // Account Claim Action Controller methods
            case "GetAccountClaimActions":
                operation.Summary = ResourceAPI.GetAccountClaimActions;
                operation.Description = ResourceAPI.DocumentationGetAccountClaimActions;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.AccountClaimActionsRetrievedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredAccountClaimActionsCouldNotBeRetrieved);
                break;

            case "GetAccountClaimActionById":
                operation.Summary = ResourceAPI.GetAccountClaimActionById;
                operation.Description = ResourceAPI.DocumentationGetAccountClaimActionById;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.AccountClaimActionRetrievedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.AccountClaimActionNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredAccountClaimActionCouldNotBeRetrieved);
                break;

            case "AddAccountClaimAction":
                operation.Summary = ResourceAPI.AddAccountClaimAction;
                operation.Description = ResourceAPI.DocumentationAddAccountClaimAction;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.AccountClaimActionCreatedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredAccountClaimActionCouldNotBeInserted);
                break;

            case "UpdateAccountClaimAction":
                operation.Summary = ResourceAPI.UpdateAccountClaimAction;
                operation.Description = ResourceAPI.DocumentationUpdateAccountClaimAction;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.AccountClaimActionUpdatedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.AccountClaimActionNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredAccountClaimActionCouldNotBeUpdated);
                break;

            case "DeleteAccountClaimAction":
                operation.Summary = ResourceAPI.DeleteAccountClaimAction;
                operation.Description = ResourceAPI.DocumentationDeleteAccountClaimAction;
                SetResponseDescription(operation, StatusCodes.Status200OK, ResourceAPI.AccountClaimActionDeletedSuccessfully);
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, ResourceAPI.InvalidDataValidationError);
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, ResourceAPI.UserNotAuthorized);
                SetResponseDescription(operation, StatusCodes.Status404NotFound, ResourceAPI.AccountClaimActionNotFound);
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, ResourceAPI.AnUnexpectedErrorOccurredAccountClaimActionCouldNotBeDeleted);
                break;
        }
    }

    private void SetResponseDescription(OpenApiOperation operation, int statusCode, string description)
    {
        var key = statusCode.ToString();
        if (operation.Responses.ContainsKey(key))
        {
            operation.Responses[key].Description = description;
        }
    }

    private CultureInfo GetCurrentCulture()
    {
        // Try to get culture from current HTTP request context first
        if (_httpContextAccessor.HttpContext != null)
        {
            var requestCultureFeature = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
            if (requestCultureFeature?.RequestCulture?.Culture != null)
            {
                return requestCultureFeature.RequestCulture.Culture;
            }
        }

        // Fall back to CurrentUICulture if no HTTP context or request culture available
        return CultureInfo.CurrentUICulture;
    }
}