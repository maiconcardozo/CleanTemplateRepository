using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using CleanTemplate.API.Resource;
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
            // CleanEntity Controller methods
            case "GetCleanEntities":
                operation.Summary = "Get all CleanEntities";
                operation.Description = "Retrieves all CleanEntity objects in the system.";
                SetResponseDescription(operation, StatusCodes.Status200OK, "CleanEntities retrieved successfully.");
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, "Invalid request parameters");
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, "Unauthorized access");
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, "Internal server error");
                break;

            case "GetCleanEntityById":
                operation.Summary = "Get CleanEntity by ID";
                operation.Description = "Retrieves a specific CleanEntity by its unique identifier.";
                SetResponseDescription(operation, StatusCodes.Status200OK, "CleanEntity retrieved successfully.");
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, "Invalid request parameters");
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, "Unauthorized access");
                SetResponseDescription(operation, StatusCodes.Status404NotFound, "CleanEntity not found.");
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, "Internal server error");
                break;

            case "AddCleanEntity":
                operation.Summary = "Add new CleanEntity";
                operation.Description = "Creates a new CleanEntity with the provided information.";
                SetResponseDescription(operation, StatusCodes.Status201Created, "CleanEntity created successfully.");
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, "Invalid request parameters");
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, "Unauthorized access");
                SetResponseDescription(operation, StatusCodes.Status409Conflict, "CleanEntity already exists.");
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, "Internal server error");
                break;

            case "UpdateCleanEntity":
                operation.Summary = "Update CleanEntity";
                operation.Description = "Updates an existing CleanEntity with the provided information.";
                SetResponseDescription(operation, StatusCodes.Status200OK, "CleanEntity updated successfully.");
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, "Invalid request parameters");
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, "Unauthorized access");
                SetResponseDescription(operation, StatusCodes.Status404NotFound, "CleanEntity not found.");
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, "Internal server error");
                break;

            case "DeleteCleanEntity":
                operation.Summary = "Delete CleanEntity";
                operation.Description = "Removes a CleanEntity from the system by its unique identifier.";
                SetResponseDescription(operation, StatusCodes.Status200OK, "CleanEntity deleted successfully.");
                SetResponseDescription(operation, StatusCodes.Status400BadRequest, "Invalid request parameters");
                SetResponseDescription(operation, StatusCodes.Status401Unauthorized, "Unauthorized access");
                SetResponseDescription(operation, StatusCodes.Status404NotFound, "CleanEntity not found.");
                SetResponseDescription(operation, StatusCodes.Status500InternalServerError, "Internal server error");
                break;
        }

        // Apply localization to all text if needed
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var feature = httpContext.Features.Get<IRequestCultureFeature>();
            var culture = feature?.RequestCulture.Culture ?? CultureInfo.CurrentCulture;
            
            // Apply culture-specific formatting if needed
            // ResourceAPI.Culture = culture; // Will be enabled once resource files are regenerated
        }
    }

    private static void SetResponseDescription(OpenApiOperation operation, int statusCode, string description)
    {
        var statusCodeString = statusCode.ToString();
        if (operation.Responses.ContainsKey(statusCodeString))
        {
            operation.Responses[statusCodeString].Description = description;
        }
    }
}