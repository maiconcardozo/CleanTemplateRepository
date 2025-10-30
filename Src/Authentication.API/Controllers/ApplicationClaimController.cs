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
    /// Controller responsible for managing application-claim mappings.
    /// Manages which claims are associated with specific applications for discrimination.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ApplicationClaimController : ControllerBase
    {
        private readonly IApplicationClaimService applicationClaimService;
        private readonly IValidator<ApplicationClaimPayLoadDTO> validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationClaimController"/> class.
        /// </summary>
        /// <param name="applicationClaimService">Service for application-claim management operations.</param>
        /// <param name="validator">Validator for ApplicationClaimPayLoadDTO.</param>
        public ApplicationClaimController(IApplicationClaimService applicationClaimService, IValidator<ApplicationClaimPayLoadDTO> validator)
        {
            this.applicationClaimService = applicationClaimService;
            this.validator = validator;
        }

        /// <summary>
        /// Retrieves all application-claim mappings in the system.
        /// </summary>
        /// <returns>List of all application-claim mappings.</returns>
        /// <response code="200">Application-claim mappings retrieved successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet(ApplicationClaimRoutes.GetApplicationClaims)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ApplicationClaimResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetApplicationClaims()
        {
            try
            {
                var applicationClaims = applicationClaimService.GetAll();
                var applicationClaimsResponse = applicationClaims.Select(ac => AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationClaimResponseDTO>(ac));
                var successResponse = SuccessResponseExampleFactory.ForSuccess(applicationClaimsResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError("An unexpected error occurred. Application-claim mappings could not be retrieved.", HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// Retrieves a specific application-claim mapping by ID.
        /// </summary>
        /// <param name="id">Application-claim mapping ID to search for.</param>
        /// <returns>Application-claim mapping matching the specified ID.</returns>
        /// <response code="200">Application-claim mapping retrieved successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Application-claim mapping not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet(ApplicationClaimRoutes.GetApplicationClaimById)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApplicationClaimResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetApplicationClaimById(int id)
        {
            try
            {
                var applicationClaim = applicationClaimService.GetById(id);
                if (applicationClaim == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound("Application-claim mapping not found.", HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var applicationClaimResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationClaimResponseDTO>(applicationClaim);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(applicationClaimResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError("An unexpected error occurred. Application-claim mapping could not be retrieved.", HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// Retrieves application-claim mappings for a specific application.
        /// </summary>
        /// <param name="applicationId">Application ID to filter by.</param>
        /// <returns>List of application-claim mappings for the specified application.</returns>
        /// <response code="200">Application-claim mappings retrieved successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet(ApplicationClaimRoutes.GetApplicationClaimsByApplicationId)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ApplicationClaimResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetApplicationClaimsByApplicationId(int applicationId)
        {
            try
            {
                var applicationClaims = applicationClaimService.GetByApplicationId(applicationId);
                var applicationClaimsResponse = applicationClaims.Select(ac => AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationClaimResponseDTO>(ac));
                var successResponse = SuccessResponseExampleFactory.ForSuccess(applicationClaimsResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError("An unexpected error occurred. Application-claim mappings could not be retrieved.", HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// Creates a new application-claim mapping.
        /// </summary>
        /// <param name="applicationClaimDTO">Application-claim mapping data for creation.</param>
        /// <param name="serviceProvider">Service provider for dependency injection.</param>
        /// <returns>Created application-claim mapping information.</returns>
        /// <response code="200">Application-claim mapping created successfully.</response>
        /// <response code="400">Invalid request data or validation errors.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost(ApplicationClaimRoutes.AddApplicationClaim)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApplicationClaimResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult AddApplicationClaim([FromBody] ApplicationClaimPayLoadDTO applicationClaimDTO, [FromServices] IServiceProvider serviceProvider)
        {
            // Validate the DTO using FluentValidation
            var validationResult = validator.Validate(applicationClaimDTO);
            if (!validationResult.IsValid)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForBadRequest(
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    HttpContext.Request.Path);
                return BadRequest(problemDetails);
            }

            var applicationClaim = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationClaim>(applicationClaimDTO);

            try
            {
                applicationClaimService.AddApplicationClaim(applicationClaim);
                var applicationClaimResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationClaimResponseDTO>(applicationClaim);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(applicationClaimResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError("An unexpected error occurred. Application-claim mapping could not be inserted.", HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// Updates an existing application-claim mapping.
        /// </summary>
        /// <param name="applicationClaimDTO">Updated application-claim mapping data.</param>
        /// <param name="serviceProvider">Service provider for dependency injection.</param>
        /// <returns>Updated application-claim mapping information.</returns>
        /// <response code="200">Application-claim mapping updated successfully.</response>
        /// <response code="400">Invalid request data or validation errors.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Application-claim mapping not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut(ApplicationClaimRoutes.UpdateApplicationClaim)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApplicationClaimResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult UpdateApplicationClaim([FromBody] ApplicationClaimPayLoadDTO applicationClaimDTO, [FromServices] IServiceProvider serviceProvider)
        {
            // Validate the DTO using FluentValidation
            var validationResult = validator.Validate(applicationClaimDTO);
            if (!validationResult.IsValid)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForBadRequest(
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    HttpContext.Request.Path);
                return BadRequest(problemDetails);
            }

            try
            {
                var existingApplicationClaim = applicationClaimService.GetById(applicationClaimDTO.Id);
                if (existingApplicationClaim == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound("Application-claim mapping not found.", HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var applicationClaim = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationClaim>(applicationClaimDTO);
                
                // Preserve CreatedBy and DtCreated from existing entity
                applicationClaim.CreatedBy = existingApplicationClaim.CreatedBy;
                applicationClaim.DtCreated = existingApplicationClaim.DtCreated;

                applicationClaimService.UpdateApplicationClaim(applicationClaim);
                var applicationClaimResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationClaimResponseDTO>(applicationClaim);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(applicationClaimResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError("An unexpected error occurred. Application-claim mapping could not be updated.", HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// Deletes an application-claim mapping.
        /// </summary>
        /// <param name="id">ID of the application-claim mapping to delete.</param>
        /// <returns>Deleted application-claim mapping details.</returns>
        /// <response code="200">Application-claim mapping deleted successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Application-claim mapping not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete(ApplicationClaimRoutes.DeleteApplicationClaim)]
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
        public IActionResult DeleteApplicationClaim(int id)
        {
            try
            {
                var existingApplicationClaim = applicationClaimService.GetById(id);
                if (existingApplicationClaim == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound("Application-claim mapping not found.", HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var applicationClaimResponseDTO = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ApplicationClaimResponseDTO>(existingApplicationClaim);
                applicationClaimService.DeleteApplicationClaim(id);
                var successResponse = new SucessDetails
                {
                    Detail = "Application-claim mapping deleted successfully.",
                    Data = applicationClaimResponseDTO,
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError("An unexpected error occurred. Application-claim mapping could not be deleted.", HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }
    }
}
