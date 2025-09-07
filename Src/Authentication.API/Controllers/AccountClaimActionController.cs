using Authentication.API.Resource;
using Authentication.API.Swagger;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.DTO;
using Authentication.Login.Mapping;
using Authentication.Login.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using FluentValidation;

namespace Authentication.API.Controllers
{
    /// <summary>
    /// ResourceAPI.AccountClaimActionControllerDescription
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AccountClaimActionController : ControllerBase
    {
        private readonly IAccountClaimActionService _accountClaimActionService;
        private readonly IValidator<AccountClaimActionPayLoadDTO> _validator;

        /// <summary>
        /// Initializes a new instance of the AccountClaimActionController.
        /// </summary>
        /// <param name="accountClaimActionService">Service for account-claim-action association management operations</param>
        /// <param name="validator">Validator for AccountClaimActionPayLoadDTO</param>
        public AccountClaimActionController(IAccountClaimActionService accountClaimActionService, IValidator<AccountClaimActionPayLoadDTO> validator)
        {
            _accountClaimActionService = accountClaimActionService;
            _validator = validator;
        }

        /// <summary>
        /// ResourceAPI.DocumentationGetAccountClaimActions
        /// </summary>
        /// <param name="idAccount">Optional account ID for filtering account claim actions</param>
        /// <param name="idClaimAction">Optional claim action ID for filtering account claim actions</param>
        /// <returns>ResourceAPI.ReturnsListOfAccountClaimActionsMatchingTheSpecifiedFilters</returns>
        /// <response code="200">ResourceAPI.AccountClaimActionsRetrievedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpGet(AccountClaimActionRoutes.GetAccountClaimActions)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<AccountClaimActionResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetAccountClaimActions([FromQuery] int? idAccount = null, [FromQuery] int? idClaimAction = null)
        {
            try
            {
                IEnumerable<AccountClaimAction> accountClaimActions;


                // For now, return empty list - could implement GetAll if needed
                accountClaimActions = new List<AccountClaimAction>();
                // Replace the selected code block with the following:
                if (idAccount.HasValue)
                {
                    accountClaimActions = _accountClaimActionService.GetByIdAccount(idAccount.Value);
                }
                if (!idAccount.HasValue && idClaimAction.HasValue)
                {
                    accountClaimActions = _accountClaimActionService.GetByIdClaimAction(idClaimAction.Value);
                }
                if (!idAccount.HasValue && !idClaimAction.HasValue)
                {
                    // For now, return empty list - could implement GetAll if needed
                    accountClaimActions = new List<AccountClaimAction>();
                }


                var accountClaimActionsResponse = accountClaimActions.Select(aca => AuthenticationLoginProfileMapperInitializer.Mapper.Map<AccountClaimActionResponseDTO>(aca));
                var successResponse = SuccessResponseExampleFactory.ForSuccess(accountClaimActionsResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(
                    ResourceAPI.AnUnexpectedErrorOccurredAccountClaimActionsCouldNotBeRetrieved,
                    HttpContext.Request.Path
                );
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationGetAccountClaimActionById
        /// </summary>
        /// <param name="idAccount">Account ID to search for</param>
        /// <param name="idClaimAction">Claim action ID to search for</param>
        /// <returns>ResourceAPI.ReturnsAccountClaimActionMatchingTheSpecifiedAccountAndClaimActionIDs</returns>
        /// <response code="200">ResourceAPI.AccountClaimActionRetrievedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="404">ResourceAPI.AccountClaimActionNotFound</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpGet(AccountClaimActionRoutes.GetAccountClaimActionById)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AccountClaimActionResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetAccountClaimActionById(int idAccount, int idClaimAction)
        {
            try
            {
                var accountClaimAction = _accountClaimActionService.GetByAccountAndClaimAction(idAccount, idClaimAction);
                if (accountClaimAction == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(
                        ResourceAPI.AccountClaimActionNotFound,
                        HttpContext.Request.Path
                    );
                    return NotFound(notFoundDetails);
                }

                var accountClaimActionResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<AccountClaimActionResponseDTO>(accountClaimAction);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(accountClaimActionResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(
                    ResourceAPI.AnUnexpectedErrorOccurredAccountClaimActionCouldNotBeRetrieved,
                    HttpContext.Request.Path
                );
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationAddAccountClaimAction
        /// </summary>
        /// <param name="accountClaimActionDTO">Account claim action data for creation</param>
        /// <param name="serviceProvider">Service provider for dependency injection</param>
        /// <returns>ResourceAPI.ReturnsCreatedAccountClaimAction</returns>
        /// <response code="200">ResourceAPI.AccountClaimActionCreatedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestData</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpPost(AccountClaimActionRoutes.AddAccountClaimAction)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AccountClaimActionResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult AddAccountClaimAction([FromBody] AccountClaimActionPayLoadDTO accountClaimActionDTO, [FromServices] IServiceProvider serviceProvider)
        {
            // Validate the DTO using FluentValidation
            var validationResult = _validator.Validate(accountClaimActionDTO);
            if (!validationResult.IsValid)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForBadRequest(
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    HttpContext.Request.Path
                );
                return BadRequest(problemDetails);
            }

            var accountClaimAction = AuthenticationLoginProfileMapperInitializer.Mapper.Map<AccountClaimAction>(accountClaimActionDTO);

            try
            {
                _accountClaimActionService.AddAccountClaimAction(accountClaimAction);
                var accountClaimActionResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<AccountClaimActionResponseDTO>(accountClaimAction);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(accountClaimActionResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(
                    ResourceAPI.AnUnexpectedErrorOccurredAccountClaimActionCouldNotBeInserted,
                    HttpContext.Request.Path
                );
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationUpdateAccountClaimAction
        /// </summary>
        /// <param name="id">ID of the account claim action to update</param>
        /// <param name="accountClaimActionDTO">Updated account claim action data</param>
        /// <param name="serviceProvider">Service provider for dependency injection</param>
        /// <returns>ResourceAPI.ReturnsUpdatedAccountClaimAction</returns>
        /// <response code="200">ResourceAPI.AccountClaimActionUpdatedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestData</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="404">ResourceAPI.AccountClaimActionNotFound</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpPut(AccountClaimActionRoutes.UpdateAccountClaimAction)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AccountClaimActionResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult UpdateAccountClaimAction(int id, [FromBody] AccountClaimActionPayLoadDTO accountClaimActionDTO, [FromServices] IServiceProvider serviceProvider)
        {
            // Validate the DTO using FluentValidation
            var validationResult = _validator.Validate(accountClaimActionDTO);
            if (!validationResult.IsValid)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForBadRequest(
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    HttpContext.Request.Path
                );
                return BadRequest(problemDetails);
            }

            try
            {
                // For simplicity, we'll use the DTO values to find the existing record
                var existingAccountClaimAction = _accountClaimActionService.GetByAccountAndClaimAction(accountClaimActionDTO.IdAccount, accountClaimActionDTO.IdClaimAction);
                if (existingAccountClaimAction == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(
                        ResourceAPI.AccountClaimActionNotFound,
                        HttpContext.Request.Path
                    );
                    return NotFound(notFoundDetails);
                }

                var accountClaimAction = AuthenticationLoginProfileMapperInitializer.Mapper.Map<AccountClaimAction>(accountClaimActionDTO);
                accountClaimAction.Id = existingAccountClaimAction.Id;

                _accountClaimActionService.UpdateAccountClaimAction(accountClaimAction);
                var accountClaimActionResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<AccountClaimActionResponseDTO>(accountClaimAction);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(accountClaimActionResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(
                    ResourceAPI.AnUnexpectedErrorOccurredAccountClaimActionCouldNotBeUpdated,
                    HttpContext.Request.Path
                );
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationDeleteAccountClaimAction
        /// </summary>
        /// <param name="idAccount">Account ID of the association to delete</param>
        /// <param name="idClaimAction">Claim action ID of the association to delete</param>
        /// <returns>ResourceAPI.ReturnsDeletedAccountClaimActionDetails</returns>
        /// <response code="200">ResourceAPI.AccountClaimActionDeletedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="404">ResourceAPI.AccountClaimActionNotFound</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpDelete(AccountClaimActionRoutes.DeleteAccountClaimAction)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(SucessDetails))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult DeleteAccountClaimAction(int idAccount, int idClaimAction)
        {
            try
            {
                var existingAccountClaimAction = _accountClaimActionService.GetByAccountAndClaimAction(idAccount, idClaimAction);
                if (existingAccountClaimAction == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(
                        ResourceAPI.AccountClaimActionNotFound,
                        HttpContext.Request.Path
                    );
                    return NotFound(notFoundDetails);
                }

                var accountClaimActionResponseDTO = AuthenticationLoginProfileMapperInitializer.Mapper.Map<AccountClaimActionResponseDTO>(existingAccountClaimAction);
                _accountClaimActionService.DeleteAccountClaimAction(existingAccountClaimAction.Id);
                var successResponse = new SucessDetails
                {
                    Detail = ResourceAPI.AccountClaimActionDeletedSuccessfully,
                    Data = accountClaimActionResponseDTO
                };
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(
                    ResourceAPI.AnUnexpectedErrorOccurredAccountClaimActionCouldNotBeDeleted,
                    HttpContext.Request.Path
                );
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }
    }
}