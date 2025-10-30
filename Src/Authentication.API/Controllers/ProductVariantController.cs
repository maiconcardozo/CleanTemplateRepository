using Authentication.API.Resource;
using Authentication.API.Swagger;
using Authentication.Login.Domain.Implementation;
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
    /// Product variant management controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductVariantController : ControllerBase
    {
        private readonly IProductVariantService productVariantService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductVariantController"/> class.
        /// </summary>
        /// <param name="productVariantService">Service for product variant management operations.</param>
        public ProductVariantController(IProductVariantService productVariantService)
        {
            this.productVariantService = productVariantService;
        }

        /// <summary>
        /// Get all product variants.
        /// </summary>
        /// <returns>List of all product variants in the system.</returns>
        /// <response code="200">Product variants retrieved successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet(ProductVariantRoutes.GetProductVariants)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductVariantResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetProductVariants()
        {
            try
            {
                var variants = productVariantService.GetAllProductVariants();
                var variantsResponse = variants.Select(v => AuthenticationLoginProfileMapperInitializer.Mapper.Map<ProductVariantResponseDTO>(v));
                var successResponse = SuccessResponseExampleFactory.ForSuccess(variantsResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
        /// Get product variant by ID.
        /// </summary>
        /// <param name="id">Product variant ID to search for.</param>
        /// <returns>Product variant matching the specified ID.</returns>
        /// <response code="200">Product variant retrieved successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Product variant not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet(ProductVariantRoutes.GetProductVariantById)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductVariantResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetProductVariantById(int id)
        {
            try
            {
                var variant = productVariantService.GetById(id);
                if (variant == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.AccountNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var variantResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ProductVariantResponseDTO>(variant);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(variantResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
        /// Get product variants by product ID.
        /// </summary>
        /// <param name="productId">Product ID to search variants for.</param>
        /// <returns>List of product variants for the specified product.</returns>
        /// <response code="200">Product variants retrieved successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet(ProductVariantRoutes.GetProductVariantsByProductId)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductVariantResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetProductVariantsByProductId(int productId)
        {
            try
            {
                var variants = productVariantService.GetByIdProduct(productId);
                var variantsResponse = variants.Select(v => AuthenticationLoginProfileMapperInitializer.Mapper.Map<ProductVariantResponseDTO>(v));
                var successResponse = SuccessResponseExampleFactory.ForSuccess(variantsResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
        /// Add new product variant.
        /// </summary>
        /// <param name="variantDTO">Product variant data for creation.</param>
        /// <param name="serviceProvider">Service provider for dependency injection.</param>
        /// <returns>Created product variant information.</returns>
        /// <response code="200">Product variant created successfully.</response>
        /// <response code="400">Invalid request data or validation errors.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="409">Product variant SKU already exists.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost(ProductVariantRoutes.AddProductVariant)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductVariantResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ProblemDetailsConflictExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public async Task<IActionResult> AddProductVariant([FromBody] ProductVariantPayLoadDTO variantDTO, [FromServices] IServiceProvider serviceProvider)
        {
            var validationResult = await ValidationHelper.ValidateEntityAsync(variantDTO, serviceProvider, this);

            if (validationResult != null)
            {
                return validationResult;
            }

            var variant = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ProductVariant>(variantDTO);

            try
            {
                productVariantService.AddProductVariant(variant);
                var variantResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ProductVariantResponseDTO>(variant);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(variantResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
        /// Update product variant.
        /// </summary>
        /// <param name="variantDTO">Updated product variant data.</param>
        /// <param name="serviceProvider">Service provider for dependency injection.</param>
        /// <returns>Updated product variant information.</returns>
        /// <response code="200">Product variant updated successfully.</response>
        /// <response code="400">Invalid request data or validation errors.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Product variant not found.</response>
        /// <response code="409">Product variant SKU already exists.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut(ProductVariantRoutes.UpdateProductVariant)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductVariantResponseDTO))]
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
        public async Task<IActionResult> UpdateProductVariant([FromBody] ProductVariantPayLoadDTO variantDTO, [FromServices] IServiceProvider serviceProvider)
        {
            try
            {
                var existingVariant = productVariantService.GetById(variantDTO.Id);

                if (existingVariant == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.AccountNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var validationResult = await ValidationHelper.ValidateEntityAsync(variantDTO, serviceProvider, this);
                if (validationResult != null)
                {
                    return validationResult;
                }

                // Preserve CreatedBy and DtCreated before mapping
                var originalCreatedBy = existingVariant.CreatedBy;
                var originalDtCreated = existingVariant.DtCreated;
                
                AuthenticationLoginProfileMapperInitializer.Mapper.Map(variantDTO, existingVariant);
                
                // Restore preserved values
                existingVariant.CreatedBy = originalCreatedBy;
                existingVariant.DtCreated = originalDtCreated;

                productVariantService.UpdateProductVariant(existingVariant);

                var variantResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ProductVariantResponseDTO>(existingVariant);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(variantResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
        /// Delete product variant.
        /// </summary>
        /// <param name="id">ID of the product variant to delete.</param>
        /// <returns>Deleted product variant details.</returns>
        /// <response code="200">Product variant deleted successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Product variant not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete(ProductVariantRoutes.DeleteProductVariant)]
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
        public IActionResult DeleteProductVariant(int id)
        {
            try
            {
                var existingVariant = productVariantService.GetById(id);

                if (existingVariant == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.AccountNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var variantResponseDTO = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ProductVariantResponseDTO>(existingVariant);

                productVariantService.DeleteProductVariant(id);

                var successResponse = new SucessDetails
                {
                    Detail = ResourceAPI.AccountDeletedSuccessfully,
                    Data = variantResponseDTO,
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
