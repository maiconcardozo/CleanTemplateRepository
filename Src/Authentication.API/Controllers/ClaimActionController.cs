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
    /// ResourceAPI.ClaimActionControllerDescription
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ClaimActionController : ControllerBase
    {
        private readonly IClaimActionService _claimActionService;
        private readonly IValidator<ClaimActionPayLoadDTO> _validator;

        /// <summary>
        /// Initializes a new instance of the ClaimActionController.
        /// </summary>
        /// <param name="claimActionService">Service for claim-action association management operations</param>
        /// <param name="validator">Validator for ClaimActionPayLoadDTO</param>
        public ClaimActionController(IClaimActionService claimActionService, IValidator<ClaimActionPayLoadDTO> validator)
        {
            _claimActionService = claimActionService;
            _validator = validator;
        }

        /// <summary>
        /// ResourceAPI.DocumentationGetClaimActions
        /// </summary>
        /// <returns>ResourceAPI.ReturnsListOfAllClaimActionsInTheSystem</returns>
        /// <response code="200">ResourceAPI.ClaimActionsRetrievedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpGet(ClaimActionRoutes.GetClaimActions)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ClaimActionResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetClaimActions()
        {
            try
            {
                var claimActions = _claimActionService.GetAll();
                var claimActionsResponse = claimActions.Select(ca => AuthenticationLoginProfileMapperInitializer.Mapper.Map<ClaimActionResponseDTO>(ca));
                var successResponse = SuccessResponseExampleFactory.ForSuccess(claimActionsResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredClaimActionsCouldNotBeRetrieved, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationGetClaimActionById
        /// </summary>
        /// <param name="id">Claim action ID to search for</param>
        /// <returns>ResourceAPI.ReturnsClaimActionMatchingTheSpecifiedID</returns>
        /// <response code="200">ResourceAPI.ClaimActionRetrievedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="404">ResourceAPI.ClaimActionNotFound</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpGet(ClaimActionRoutes.GetClaimActionById)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ClaimActionResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetClaimActionById(int id)
        {
            try
            {
                var claimAction = _claimActionService.GetById(id);
                if (claimAction == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.ClaimActionNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var claimActionResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ClaimActionResponseDTO>(claimAction);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(claimActionResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredClaimActionCouldNotBeRetrieved, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationAddClaimAction
        /// </summary>
        /// <param name="claimActionDTO">Claim action data for creation</param>
        /// <param name="serviceProvider">Service provider for dependency injection</param>
        /// <returns>ResourceAPI.ReturnsCreatedClaimActionInformation</returns>
        /// <response code="200">ResourceAPI.ClaimActionCreatedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestDataOrValidationErrors</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpPost(ClaimActionRoutes.AddClaimAction)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ClaimActionResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult AddClaimAction([FromBody] ClaimActionPayLoadDTO claimActionDTO, [FromServices] IServiceProvider serviceProvider)
        {
            // Validate the DTO using FluentValidation
            var validationResult = _validator.Validate(claimActionDTO);
            if (!validationResult.IsValid)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForBadRequest(
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    HttpContext.Request.Path
                );
                return BadRequest(problemDetails);
            }

            var claimAction = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ClaimAction>(claimActionDTO);

            try
            {
                _claimActionService.AddClaimAction(claimAction);
                var claimActionResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ClaimActionResponseDTO>(claimAction);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(claimActionResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredClaimActionCouldNotBeInserted, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationUpdateClaimAction
        /// </summary>
        /// <param name="id">ID of the claim action to update</param>
        /// <param name="claimActionDTO">Updated claim action data</param>
        /// <param name="serviceProvider">Service provider for dependency injection</param>
        /// <returns>ResourceAPI.ReturnsUpdatedClaimActionInformation</returns>
        /// <response code="200">ResourceAPI.ClaimActionUpdatedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestDataOrValidationErrors</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="404">ResourceAPI.ClaimActionNotFound</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpPut(ClaimActionRoutes.UpdateClaimAction)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ClaimActionResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult UpdateClaimAction(int id, [FromBody] ClaimActionPayLoadDTO claimActionDTO, [FromServices] IServiceProvider serviceProvider)
        {
            // Validate the DTO using FluentValidation
            var validationResult = _validator.Validate(claimActionDTO);
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
                var existingClaimAction = _claimActionService.GetById(id);
                if (existingClaimAction == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.ClaimActionNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var claimAction = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ClaimAction>(claimActionDTO);
                claimAction.Id = id;

                _claimActionService.UpdateClaimAction(claimAction);
                var claimActionResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ClaimActionResponseDTO>(claimAction);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(claimActionResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredClaimActionCouldNotBeUpdated, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationDeleteClaimAction
        /// </summary>
        /// <param name="id">ID of the claim action to delete</param>
        /// <returns>ResourceAPI.ReturnsDeletedClaimActionDetails</returns>
        /// <response code="200">ResourceAPI.ClaimActionDeletedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="404">ResourceAPI.ClaimActionNotFound</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpDelete(ClaimActionRoutes.DeleteClaimAction)]
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
        public IActionResult DeleteClaimAction(int id)
        {
            try
            {
                var existingClaimAction = _claimActionService.GetById(id);
                if (existingClaimAction == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.ClaimActionNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var claimActionResponseDTO = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ClaimActionResponseDTO>(existingClaimAction);
                _claimActionService.DeleteClaimAction(id);
                var successResponse = new SucessDetails
                {
                    Detail = ResourceAPI.ClaimActionDeletedSuccessfully,
                    Data = claimActionResponseDTO
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredClaimActionCouldNotBeDeleted, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }
    }
}