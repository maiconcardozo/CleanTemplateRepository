using Authentication.API.Resource;
using Authentication.API.Swagger;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Domain.Interface;
using Authentication.Login.DTO;
using Authentication.Login.Exceptions;
using Authentication.Login.Mapping;
using Authentication.Login.Services.Interface;
using Foundation.Base.Util;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Authentication.API.Controllers
{
    /// <summary>
    /// ResourceAPI.AccountControllerDescription.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// Initializes a new instance of the AccountController.
        /// </summary>
        /// <param name="accountService">Service for account management operations.</param>
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// ResourceAPI.DocumentationGetAccounts.
        /// </summary>
        /// <returns>ResourceAPI.ReturnsListOfAllUserAccountsInTheSystem.</returns>
        /// <response code="200">ResourceAPI.AccountsRetrievedSuccessfully.</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters.</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess.</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError.</response>
        [HttpGet(AccountRoutes.GetAccounts)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<AccountResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetAccounts()
        {
            try
            {
                var accounts = accountService.GetAllAccounts();
                var accountsResponse = accounts.Select(a => AuthenticationLoginProfileMapperInitializer.Mapper.Map<AccountResponseDTO>(a));
                var successResponse = SuccessResponseExampleFactory.ForSuccess(accountsResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredAccountsCouldNotBeRetrieved, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationGetAccountById
        /// </summary>
        /// <param name="id">Account ID to search for.</param>
        /// <returns>ResourceAPI.ReturnsAccountMatchingTheSpecifiedID.</returns>
        /// <response code="200">ResourceAPI.AccountRetrievedSuccessfully.</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters.</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess.</response>
        /// <response code="404">ResourceAPI.AccountNotFound.</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError.</response>
        [HttpGet(AccountRoutes.GetAccountById)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AccountResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetAccountById(int id)
        {
            try
            {
                var account = accountService.GetById(id);
                if (account == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.AccountNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var accountResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<AccountResponseDTO>(account);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(accountResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredAccountCouldNotBeRetrieved, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationAddAccount
        /// </summary>
        /// <param name="authenticationDTO">Account data for creation.</param>
        /// <param name="serviceProvider">Service provider for dependency injection.</param>
        /// <returns>ResourceAPI.ReturnsCreatedAccountInformation.</returns>
        /// <response code="200">ResourceAPI.AccountCreatedSuccessfully.</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestDataOrValidationErrors.</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess.</response>
        /// <response code="409">ResourceAPI.ResponseAccountUsernameAlreadyExists.</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError.</response>
        [HttpPost(AccountRoutes.AddAccount)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AccountResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ProblemDetailsConflictExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public async Task<IActionResult> AddAccount([FromBody] AccountPayLoadDTO authenticationDTO, [FromServices] IServiceProvider serviceProvider)
        {
            var validationResult = await ValidationHelper.ValidateEntityAsync(authenticationDTO, serviceProvider, this);

            if (validationResult != null)
            {
                return validationResult;
            }

            var account = AuthenticationLoginProfileMapperInitializer.Mapper.Map<Account>(authenticationDTO);

            try
            {
                accountService.AddAccount(account);
                var accountResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<AccountResponseDTO>(account);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(accountResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
                return Ok(successResponse);
            }
            catch (ConflictException ex)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForConflict(ex.Message, HttpContext.Request.Path);
                return Conflict(problemDetails);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredAccountCouldInserted, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationUpdateAccount.
        /// </summary>
        /// <param name="accountDTO">Updated account data.</param>
        /// <param name="serviceProvider">Service provider for dependency injection.</param>
        /// <returns>ResourceAPI.ReturnsUpdatedAccountInformation.</returns>
        /// <response code="200">ResourceAPI.AccountUpdatedSuccessfully.</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestDataOrValidationErrors.</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess.</response>
        /// <response code="404">ResourceAPI.AccountNotFound.</response>
        /// <response code="409">ResourceAPI.ResponseAccountUsernameAlreadyExists.</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError.</response>
        [HttpPut(AccountRoutes.UpdateAccount)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(AccountResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ProblemDetailsConflictExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountPayLoadDTO accountDTO, [FromServices] IServiceProvider serviceProvider)
        {
            try
            {
                var existingAccount = accountService.GetById(accountDTO.Id);

                if (existingAccount == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.AccountNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var validationResult = await ValidationHelper.ValidateEntityAsync(accountDTO, serviceProvider, this);
                if (validationResult != null)
                {
                    return validationResult;
                }

                // Preserve CreatedBy and DtCreated before mapping
                var originalCreatedBy = existingAccount.CreatedBy;
                var originalDtCreated = existingAccount.DtCreated;
                
                AuthenticationLoginProfileMapperInitializer.Mapper.Map(accountDTO, existingAccount);
                
                // Restore preserved values
                existingAccount.CreatedBy = originalCreatedBy;
                existingAccount.DtCreated = originalDtCreated;

                accountService.UpdateAccount(existingAccount);

                var accountResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<AccountResponseDTO>(existingAccount);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(accountResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
                return Ok(successResponse);
            }
            catch (ConflictException ex)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForConflict(ex.Message, HttpContext.Request.Path);
                return Conflict(problemDetails);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredAccountCouldNotBeUpdated, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationDeleteAccount.
        /// </summary>
        /// <param name="id">ID of the account to delete.</param>
        /// <returns>ResourceAPI.ReturnsDeletedAccountDetails.</returns>
        /// <response code="200">ResourceAPI.AccountDeletedSuccessfully.</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters.</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess.</response>
        /// <response code="404">ResourceAPI.AccountNotFound.</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError.</response>
        [HttpDelete(AccountRoutes.DeleteAccount)]
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
        public IActionResult DeleteAccount(int id)
        {
            try
            {
                var existingAccount = accountService.GetById(id);

                if (existingAccount == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.AccountNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var accountResponseDTO = AuthenticationLoginProfileMapperInitializer.Mapper.Map<AccountResponseDTO>(existingAccount);

                accountService.DeleteAccount(id);

                var successResponse = new SucessDetails
                {
                    Detail = ResourceAPI.AccountDeletedSuccessfully,
                    Data = accountResponseDTO,
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredAccountCouldNotBeDeleted, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }
    }
}
