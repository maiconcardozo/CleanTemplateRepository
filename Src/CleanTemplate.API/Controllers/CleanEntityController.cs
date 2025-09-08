using CleanTemplate.API.Resource;
using CleanTemplate.API.Swagger;
using CleanTemplate.Application.Domain.Implementation;
using CleanTemplate.Application.DTO;
using CleanTemplate.Application.Mapping;
using CleanTemplate.Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CleanTemplate.API.Controllers
{
    /// <summary>
    /// ResourceAPI.CleanEntityControllerDescription
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CleanEntityController : ControllerBase
    {
        private readonly ICleanEntityService _cleanEntityService;

        /// <summary>
        /// Initializes a new instance of the CleanEntityController.
        /// </summary>
        /// <param name="cleanEntityService">Service for clean entity management operations</param>
        public CleanEntityController(ICleanEntityService cleanEntityService)
        {
            _cleanEntityService = cleanEntityService;
        }

        /// <summary>
        /// ResourceAPI.DocumentationGetCleanEntities
        /// </summary>
        /// <returns>
        /// ResourceAPI.ReturnsListOfCleanEntityObjectsWithTheirNamesAndDescriptionsOnSuccessValidationErrorsUnauthorizedAccessOrInternalServerError
        /// </returns>
        /// <response code="200">ResourceAPI.CleanEntitiesRetrievedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorized</response>
        /// <response code="500">ResourceAPI.InternalServerError</response>
        [HttpGet(CleanEntityRoutes.GetCleanEntities)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<CleanEntityResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetCleanEntities()
        {
            try
            {
                var cleanEntities = _cleanEntityService.GetAllCleanEntities();
                var cleanEntitiesResponse = cleanEntities.Select(c => CleanTemplateApplicationMapperInitializer.Mapper.Map<CleanEntityResponseDTO>(c));
                var successResponse = SuccessResponseExampleFactory.ForSuccess(cleanEntitiesResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.InternalServerError, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationGetCleanEntityById
        /// </summary>
        /// <param name="id">CleanEntity ID to search for</param>
        /// <returns>ResourceAPI.ReturnsCleanEntityMatchingTheSpecifiedID</returns>
        /// <response code="200">ResourceAPI.CleanEntityRetrievedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="404">ResourceAPI.CleanEntityNotFound</response>
        /// <response code="500">ResourceAPI.InternalServerError</response>
        [HttpGet(CleanEntityRoutes.GetCleanEntityById)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CleanEntityResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetCleanEntityById(int id)
        {
            try
            {
                var cleanEntity = _cleanEntityService.GetById(id);
                if (cleanEntity == null)
                {
                    var problemDetails = ProblemDetailsExampleFactory.ForNotFound("CleanEntity not found", HttpContext.Request.Path);
                    return NotFound(problemDetails);
                }

                var cleanEntityResponse = CleanTemplateApplicationMapperInitializer.Mapper.Map<CleanEntityResponseDTO>(cleanEntity);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(cleanEntityResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.InternalServerError, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationAddCleanEntity
        /// </summary>
        /// <param name="cleanEntityPayLoad">CleanEntity data to create</param>
        /// <returns>ResourceAPI.ReturnsCreatedCleanEntityOnSuccessValidationErrorsUnauthorizedAccessOrInternalServerError</returns>
        /// <response code="201">ResourceAPI.CleanEntityCreatedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="409">ResourceAPI.CleanEntityAlreadyExists</response>
        /// <response code="500">ResourceAPI.InternalServerError</response>
        [HttpPost(CleanEntityRoutes.AddCleanEntity)]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CleanEntityResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ProblemDetailsConflictExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult AddCleanEntity([FromBody] CleanEntityPayLoadDTO cleanEntityPayLoad)
        {
            try
            {
                var cleanEntity = CleanTemplateApplicationMapperInitializer.Mapper.Map<CleanEntity>(cleanEntityPayLoad);
                _cleanEntityService.AddCleanEntity(cleanEntity);

                var cleanEntityResponse = CleanTemplateApplicationMapperInitializer.Mapper.Map<CleanEntityResponseDTO>(cleanEntity);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(cleanEntityResponse, "CleanEntity created successfully", HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status201Created, successResponse);
            }
            catch (InvalidOperationException ex)
            {
                var problemDetails = ProblemDetailsExampleFactory.ForConflict(ex.Message, HttpContext.Request.Path);
                return Conflict(problemDetails);
            }
            catch (ArgumentException ex)
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.InternalServerError, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationUpdateCleanEntity
        /// </summary>
        /// <param name="id">CleanEntity ID to update</param>
        /// <param name="cleanEntityPayLoad">Updated clean entity data</param>
        /// <returns>ResourceAPI.ReturnsUpdatedCleanEntityOnSuccessValidationErrorsUnauthorizedAccessOrInternalServerError</returns>
        /// <response code="200">ResourceAPI.CleanEntityUpdatedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="404">ResourceAPI.CleanEntityNotFound</response>
        /// <response code="500">ResourceAPI.InternalServerError</response>
        [HttpPut(CleanEntityRoutes.UpdateCleanEntity)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CleanEntityResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult UpdateCleanEntity(int id, [FromBody] CleanEntityPayLoadDTO cleanEntityPayLoad)
        {
            try
            {
                var existingCleanEntity = _cleanEntityService.GetById(id);
                if (existingCleanEntity == null)
                {
                    var problemDetails = ProblemDetailsExampleFactory.ForNotFound("CleanEntity not found", HttpContext.Request.Path);
                    return NotFound(problemDetails);
                }

                var cleanEntity = CleanTemplateApplicationMapperInitializer.Mapper.Map<CleanEntity>(cleanEntityPayLoad);
                cleanEntity.Id = id;
                _cleanEntityService.UpdateCleanEntity(cleanEntity);

                var cleanEntityResponse = CleanTemplateApplicationMapperInitializer.Mapper.Map<CleanEntityResponseDTO>(cleanEntity);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(cleanEntityResponse, "CleanEntity updated successfully", HttpContext.Request.Path);
                return Ok(successResponse);
            }
            catch (ArgumentException ex)
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.InternalServerError, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }

        /// <summary>
        /// ResourceAPI.DocumentationDeleteCleanEntity
        /// </summary>
        /// <param name="id">CleanEntity ID to delete</param>
        /// <returns>ResourceAPI.ReturnsConfirmationMessageOnSuccessValidationErrorsUnauthorizedAccessOrInternalServerError</returns>
        /// <response code="200">ResourceAPI.CleanEntityDeletedSuccessfully</response>
        /// <response code="400">ResourceAPI.ResponseInvalidRequestParameters</response>
        /// <response code="401">ResourceAPI.ResponseUnauthorizedAccess</response>
        /// <response code="404">ResourceAPI.CleanEntityNotFound</response>
        /// <response code="500">ResourceAPI.InternalServerError</response>
        [HttpDelete(CleanEntityRoutes.DeleteCleanEntity)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult DeleteCleanEntity(int id)
        {
            try
            {
                var existingCleanEntity = _cleanEntityService.GetById(id);
                if (existingCleanEntity == null)
                {
                    var problemDetails = ProblemDetailsExampleFactory.ForNotFound("CleanEntity not found", HttpContext.Request.Path);
                    return NotFound(problemDetails);
                }

                _cleanEntityService.DeleteCleanEntity(id);
                var successResponse = SuccessResponseExampleFactory.ForSuccess("CleanEntity deleted successfully", "CleanEntity deleted successfully", HttpContext.Request.Path);
                return Ok(successResponse);
            }
            catch (ArgumentException ex)
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
                var problemDetails = ProblemDetailsExampleFactory.ForInternalServerError(ResourceAPI.InternalServerError, HttpContext.Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }
    }
}