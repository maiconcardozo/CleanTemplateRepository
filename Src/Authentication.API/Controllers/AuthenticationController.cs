using Authentication.API.Resource;
using Authentication.API.Swagger;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Domain.Interface;
using Authentication.Login.DTO;
using Authentication.Login.Mapping;
using Authentication.Login.Services.Interface;
using Foundation.Base.Util;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Authentication.API.Controllers
{
    /// <summary>
    /// ResourceAPI.AuthenticationControllerDescription
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IJwtSettings _jwtSettings;

        /// <summary>
        /// Initializes a new instance of the AuthenticationController.
        /// </summary>
        /// <param name="accountService">Service for account management operations</param>
        /// <param name="jwtSettings">JWT configuration settings</param>
        public AuthenticationController(
            IAccountService accountService, IJwtSettings jwtSettings)
        {
            _accountService = accountService;
            _jwtSettings = jwtSettings;
        }

        /// <summary>
        /// ResourceAPI.DocumentationGenerateToken
        /// </summary>
        /// <param name="authenticationDTO">User credentials (username and password)</param>
        /// <param name="serviceProvider">Service provider for dependency injection</param>
        /// <returns>
        /// ResourceAPI.ReturnsJWTTokenWithUserInformationOnSuccessValidationErrorsUnauthorizedAccessOrInternalServerError
        /// </returns>
        /// <response code="200">ResourceAPI.TokenGeneratedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestDataOrValidationErrors</response>
        /// <response code="401">ResourceAPI.ResponseInvalidCredentials</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpPost(AuthenticationRoutes.GenerateToken)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TokenResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public async Task<IActionResult> GenerateToken([FromBody] AccountPayLoadDTO authenticationDTO, [FromServices] IServiceProvider serviceProvider)
        {
            var validationResult = await ValidationHelper.ValidateEntityAsync(authenticationDTO, serviceProvider, this);
            if (validationResult != null)
                return validationResult;

            var account = AuthenticationLoginProfileMapperInitializer.Mapper.Map<Account>(authenticationDTO);

            try
            {
                var token = await _accountService.GenerateTokenAsync(account, _jwtSettings);
                var tokenDTO = AuthenticationLoginProfileMapperInitializer.Mapper.Map<TokenResponseDTO>(token);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(tokenDTO, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
                return Ok(successResponse);
            }
            catch (InvalidOperationException ex)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForBadRequest(ex.Message, HttpContext.Request.Path);
                return BadRequest(problemDetails);
            }
            catch (UnauthorizedAccessException ex)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForUnauthorized(ex.Message, HttpContext.Request.Path);
                return Unauthorized(problemDetails);
            }
            catch (Exception)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredTokenCouldGenerated, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }


    }
}