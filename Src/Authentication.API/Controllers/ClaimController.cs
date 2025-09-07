using Authentication.API.Resource;
using Authentication.API.Swagger;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.DTO;
using Authentication.Login.Mapping;
using Authentication.Login.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Authentication.API.Controllers
{
    /// <summary>
    /// ResourceAPI.ClaimControllerDescription
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _claimService;

        /// <summary>
        /// Initializes a new instance of the ClaimController.
        /// </summary>
        /// <param name="claimService">Service for claim management operations</param>
        public ClaimController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        /// <summary>
        /// ResourceAPI.DocumentationGetClaims
        /// </summary>
        /// <returns>
        /// ResourceAPI.ReturnsListOfClaimObjectsWithTheirTypesValuesAndDescriptionsOnSuccessValidationErrorsUnauthorizedAccessOrInternalServerError
        /// </returns>
        /// <response code="200">ResourceAPI.ClaimsRetrievedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccessValidJWTTokenRequired</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpGet(ClaimRoutes.GetClaims)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ClaimResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetClaims()
        {
            try
            {
                var claims = _claimService.GetAll();
                var claimsResponse = claims.Select(c => AuthenticationLoginProfileMapperInitializer.Mapper.Map<ClaimResponseDTO>(c));
                var successResponse = SuccessResponseExampleFactory.ForSuccess(claimsResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredClaimsCouldNotBeRetrieved, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationGetClaimById
        /// </summary>
        /// <param name="id">Claim ID to search for</param>
        /// <returns>ResourceAPI.ReturnsClaimMatchingTheSpecifiedID</returns>
        /// <response code="200">ResourceAPI.ClaimRetrievedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="404">ResourceAPI.ClaimNotFound</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpGet(ClaimRoutes.GetClaimById)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ClaimResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetClaimById(int id)
        {
            try
            {
                var claim = _claimService.GetById(id);
                if (claim == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.ClaimNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var claimResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ClaimResponseDTO>(claim);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(claimResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredClaimCouldNotBeRetrieved, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationAddClaim
        /// </summary>
        /// <param name="claimDTO">Claim data for creation</param>
        /// <param name="serviceProvider">Service provider for dependency injection</param>
        /// <returns>ResourceAPI.ReturnsCreatedClaimInformation</returns>
        /// <response code="200">ResourceAPI.ClaimCreatedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestDataOrValidationErrors</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpPost(ClaimRoutes.AddClaim)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ClaimResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult AddClaim([FromBody] ClaimPayLoadDTO claimDTO, [FromServices] IServiceProvider serviceProvider)
        {
            // TODO: Validation would go here if needed

            var claim = AuthenticationLoginProfileMapperInitializer.Mapper.Map<Claim>(claimDTO);

            try
            {
                _claimService.AddClaim(claim);
                var claimResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ClaimResponseDTO>(claim);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(claimResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredClaimCouldNotBeInserted, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationUpdateClaim
        /// </summary>
        /// <param name="id">ID of the claim to update</param>
        /// <param name="claimDTO">Updated claim data</param>
        /// <param name="serviceProvider">Service provider for dependency injection</param>
        /// <returns>ResourceAPI.ReturnsUpdatedClaimInformation</returns>
        /// <response code="200">ResourceAPI.ClaimUpdatedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestDataOrValidationErrors</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="404">ResourceAPI.ClaimNotFound</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpPut(ClaimRoutes.UpdateClaim)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ClaimResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult UpdateClaim(int id, [FromBody] ClaimPayLoadDTO claimDTO, [FromServices] IServiceProvider serviceProvider)
        {
            try
            {
                var existingClaim = _claimService.GetById(id);
                if (existingClaim == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.ClaimNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var claim = AuthenticationLoginProfileMapperInitializer.Mapper.Map<Claim>(claimDTO);
                claim.Id = id;

                _claimService.UpdateClaim(claim);
                var claimResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ClaimResponseDTO>(claim);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(claimResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredClaimCouldNotBeUpdated, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationDeleteClaim
        /// </summary>
        /// <param name="id">ID of the claim to delete</param>
        /// <returns>ResourceAPI.ReturnsDeletedClaimDetails</returns>
        /// <response code="200">ResourceAPI.ClaimDeletedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="404">ResourceAPI.ClaimNotFound</response>
        /// <response code="500">ResourceAPI.ResponseInternalServerError</response>
        [HttpDelete(ClaimRoutes.DeleteClaim)]
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
        public IActionResult DeleteClaim(int id)
        {
            try
            {
                var existingClaim = _claimService.GetById(id);
                if (existingClaim == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.ClaimNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var claimResponseDTO = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ClaimResponseDTO>(existingClaim);
                _claimService.DeleteClaim(id);
                var successResponse = new SucessDetails
                {
                    Detail = ResourceAPI.ClaimDeletedSuccessfully,
                    Data = claimResponseDTO
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.AnUnexpectedErrorOccurredClaimCouldNotBeDeleted, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }
    }
}