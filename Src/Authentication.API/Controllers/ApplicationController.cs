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
    /// Controller responsible for managing application entities.
    /// Applications are used to discriminate and organize claims per application/system.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService applicationService;
        private readonly IValidator<ApplicationPayLoadDTO> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationController"/> class.
        /// </summary>
        /// <param name="applicationService">Service for application management operations.</param>
        /// <param name="validator">Validator for ApplicationPayLoadDTO.</param>
        public ApplicationController(IApplicationService applicationService, IValidator<ApplicationPayLoadDTO> validator)
        {
            this.applicationService = applicationService;
            this.validator = validator;
        }

        /// <summary>
        /// Retrieves all applications in the system.
        /// </summary>
        /// <returns>List of all applications with their details.</returns>
        /// <response code="200">Applications retrieved successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet(ApplicationRoutes.GetApplications)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ApplicationResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetApplications()
        {
            try
            {
                var applications = applicationService.GetAll();
                var applicationsResponse = applications.Select(a => AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationResponseDTO>(a));
                var successResponse = SuccessResponseExampleFactory.ForSuccess(applicationsResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError("An unexpected error occurred. Applications could not be retrieved.", HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// Retrieves a specific application by ID.
        /// </summary>
        /// <param name="id">Application ID to search for.</param>
        /// <returns>Application matching the specified ID.</returns>
        /// <response code="200">Application retrieved successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Application not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet(ApplicationRoutes.GetApplicationById)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApplicationResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetApplicationById(int id)
        {
            try
            {
                var application = applicationService.GetById(id);
                if (application == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound("Application not found.", HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var applicationResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationResponseDTO>(application);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(applicationResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError("An unexpected error occurred. Application could not be retrieved.", HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// Creates a new application.
        /// </summary>
        /// <param name="applicationDTO">Application data for creation.</param>
        /// <param name="serviceProvider">Service provider for dependency injection.</param>
        /// <returns>Created application information.</returns>
        /// <response code="200">Application created successfully.</response>
        /// <response code="400">Invalid request data or validation errors.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost(ApplicationRoutes.AddApplication)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApplicationResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult AddApplication([FromBody] ApplicationPayLoadDTO applicationDTO, [FromServices] IServiceProvider serviceProvider)
        {
            // Validate the DTO using FluentValidation
            var validationResult = validator.Validate(applicationDTO);
            if (!validationResult.IsValid)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForBadRequest(
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    HttpContext.Request.Path);
                return BadRequest(problemDetails);
            }

            var application = AuthenticationLoginProfileMapperInitializer.Mapper.Map<Application>(applicationDTO);

            try
            {
                applicationService.AddApplication(application);
                var applicationResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationResponseDTO>(application);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(applicationResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError("An unexpected error occurred. Application could not be inserted.", HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// Updates an existing application.
        /// </summary>
        /// <param name="applicationDTO">Updated application data.</param>
        /// <param name="serviceProvider">Service provider for dependency injection.</param>
        /// <returns>Updated application information.</returns>
        /// <response code="200">Application updated successfully.</response>
        /// <response code="400">Invalid request data or validation errors.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Application not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut(ApplicationRoutes.UpdateApplication)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApplicationResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult UpdateApplication([FromBody] ApplicationPayLoadDTO applicationDTO, [FromServices] IServiceProvider serviceProvider)
        {
            // Validate the DTO using FluentValidation
            var validationResult = validator.Validate(applicationDTO);
            if (!validationResult.IsValid)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForBadRequest(
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    HttpContext.Request.Path);
                return BadRequest(problemDetails);
            }

            try
            {
                var existingApplication = applicationService.GetById(applicationDTO.Id);
                if (existingApplication == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound("Application not found.", HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var application = AuthenticationLoginProfileMapperInitializer.Mapper.Map<Application>(applicationDTO);
                
                // Preserve CreatedBy and DtCreated from existing entity
                application.CreatedBy = existingApplication.CreatedBy;
                application.DtCreated = existingApplication.DtCreated;

                applicationService.UpdateApplication(application);
                var applicationResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationResponseDTO>(application);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(applicationResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError("An unexpected error occurred. Application could not be updated.", HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// Deletes an application.
        /// </summary>
        /// <param name="id">ID of the application to delete.</param>
        /// <returns>Deleted application details.</returns>
        /// <response code="200">Application deleted successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Application not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete(ApplicationRoutes.DeleteApplication)]
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
        public IActionResult DeleteApplication(int id)
        {
            try
            {
                var existingApplication = applicationService.GetById(id);
                if (existingApplication == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound("Application not found.", HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var applicationResponseDTO = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationResponseDTO>(existingApplication);
                applicationService.DeleteApplication(id);
                var successResponse = new SucessDetails
                {
                    Detail = "Application deleted successfully.",
                    Data = applicationResponseDTO,
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError("An unexpected error occurred. Application could not be deleted.", HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }
    }
}
