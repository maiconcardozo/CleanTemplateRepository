using Authentication.API.Resource;
using Authentication.API.Swagger;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.DTO;
using Authentication.Login.Mapping;
using Authentication.Login.Services.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Authentication.API.Controllers
{
    /// <summary>
    /// ResourceAPI.ActionControllerDescription.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ActionController : ControllerBase
    {
        private readonly IActionService actionService;
        private readonly IValidator<ActionPayLoadDTO> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionController"/> class.
        /// Initializes a new instance of the ActionController.
        /// </summary>
        /// <param name="actionService">Service for action management operations.</param>
        /// <param name="validator">Validator for ActionPayLoadDTO.</param>
        public ActionController(IActionService actionService, IValidator<ActionPayLoadDTO> validator)
        {
            this.actionService = actionService;
            this.validator = validator;
        }

        /// <summary>
        /// ResourceAPI.DocumentationGetActions.
        /// </summary>
        /// <returns>ResourceAPI.ReturnsListOfAllActionsInTheSystem.</returns>
        /// <response code="200">ResourceAPI.ActionsRetrievedSuccessfully.</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters.</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess.</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError.</response>
        [HttpGet(ActionRoutes.GetActions)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ActionResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetActions()
        {
            try
            {
                var actions = actionService.GetAll();
                var actionsResponse = actions.Select(a => AuthenticationLoginProfileMapperInitializer.Mapper.Map<ActionResponseDTO>(a));
                var successResponse = SuccessResponseExampleFactory.ForSuccess(actionsResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredActionsCouldNotBeRetrieved, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationGetActionById.
        /// </summary>
        /// <param name="id">Action ID to search for.</param>
        /// <returns>ResourceAPI.ReturnsActionMatchingTheSpecifiedID.</returns>
        /// <response code="200">ResourceAPI.ActionRetrievedSuccessfully.</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters.</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess.</response>
        /// <response code="404">ResourceAPI.ActionNotFound.</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError.</response>
        [HttpGet(ActionRoutes.GetActionById)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ActionResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetActionById(int id)
        {
            try
            {
                var action = actionService.GetById(id);
                if (action == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.ActionNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var actionResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ActionResponseDTO>(action);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(actionResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredActionCouldNotBeRetrieved, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationAddAction.
        /// </summary>
        /// <param name="actionDTO">Action data for creation.</param>
        /// <param name="serviceProvider">Service provider for dependency injection.</param>
        /// <returns>ResourceAPI.ReturnsCreatedActionInformation.</returns>
        /// <response code="200">ResourceAPI.ActionCreatedSuccessfully.</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestDataOrValidationErrors.</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess.</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError.</response>
        [HttpPost(ActionRoutes.AddAction)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ActionResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult AddAction([FromBody] ActionPayLoadDTO actionDTO, [FromServices] IServiceProvider serviceProvider)
        {
            // Validate the DTO using FluentValidation
            var validationResult = validator.Validate(actionDTO);
            if (!validationResult.IsValid)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForBadRequest(
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    HttpContext.Request.Path);
                return BadRequest(problemDetails);
            }

            var action = AuthenticationLoginProfileMapperInitializer.Mapper.Map<Authentication.Login.Domain.Implementation.Action>(actionDTO);

            try
            {
                actionService.AddAction(action);
                var actionResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ActionResponseDTO>(action);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(actionResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredActionCouldNotBeInserted, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationUpdateAction.
        /// </summary>
        /// <param name="actionDTO">Updated action data.</param>
        /// <param name="serviceProvider">Service provider for dependency injection.</param>
        /// <returns>ResourceAPI.ReturnsUpdatedActionInformation.</returns>
        /// <response code="200">ResourceAPI.ActionUpdatedSuccessfully.</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestDataOrValidationErrors.</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess.</response>
        /// <response code="404">ResourceAPI.ActionNotFound.</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError.</response>
        [HttpPut(ActionRoutes.UpdateAction)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ActionResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult UpdateAction([FromBody] ActionPayLoadDTO actionDTO, [FromServices] IServiceProvider serviceProvider)
        {
            // Validate the DTO using FluentValidation
            var validationResult = validator.Validate(actionDTO);
            if (!validationResult.IsValid)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForBadRequest(
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    HttpContext.Request.Path);
                return BadRequest(problemDetails);
            }

            try
            {
                var existingAction = actionService.GetById(actionDTO.Id);
                if (existingAction == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.ActionNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var action = AuthenticationLoginProfileMapperInitializer.Mapper.Map<Authentication.Login.Domain.Implementation.Action>(actionDTO);
                
                // Preserve CreatedBy and DtCreated from existing entity
                action.CreatedBy = existingAction.CreatedBy;
                action.DtCreated = existingAction.DtCreated;

                actionService.UpdateAction(action);
                var actionResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ActionResponseDTO>(action);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(actionResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredActionCouldNotBeUpdated, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationDeleteAction.
        /// </summary>
        /// <param name="id">ID of the action to delete.</param>
        /// <returns>ResourceAPI.ReturnsDeletedActionDetails.</returns>
        /// <response code="200">ResourceAPI.ActionDeletedSuccessfully.</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters.</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess.</response>
        /// <response code="404">ResourceAPI.ActionNotFound.</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError.</response>
        [HttpDelete(ActionRoutes.DeleteAction)]
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
        public IActionResult DeleteAction(int id)
        {
            try
            {
                var existingAction = actionService.GetById(id);
                if (existingAction == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.ActionNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var actionResponseDTO = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ActionResponseDTO>(existingAction);
                actionService.DeleteAction(id);
                var successResponse = new SucessDetails
                {
                    Detail = ResourceAPI.ActionDeletedSuccessfully,
                    Data = actionResponseDTO,
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredActionCouldNotBeDeleted, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }
    }
}
