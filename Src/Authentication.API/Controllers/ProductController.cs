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
    /// Product management controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productService">Service for product management operations.</param>
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        /// <returns>List of all products in the system.</returns>
        /// <response code="200">Products retrieved successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet(ProductRoutes.GetProducts)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetProducts()
        {
            try
            {
                var products = productService.GetAllProducts();
                var productsResponse = products.Select(p => AuthenticationLoginProfileMapperInitializer.Mapper.Map<ProductResponseDTO>(p));
                var successResponse = SuccessResponseExampleFactory.ForSuccess(productsResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
        /// Get product by ID.
        /// </summary>
        /// <param name="id">Product ID to search for.</param>
        /// <returns>Product matching the specified ID.</returns>
        /// <response code="200">Product retrieved successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Product not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet(ProductRoutes.GetProductById)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProblemDetailsNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public IActionResult GetProductById(int id)
        {
            try
            {
                var product = productService.GetById(id);
                if (product == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.AccountNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var productResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ProductResponseDTO>(product);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(productResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
        /// Add new product.
        /// </summary>
        /// <param name="productDTO">Product data for creation.</param>
        /// <param name="serviceProvider">Service provider for dependency injection.</param>
        /// <returns>Created product information.</returns>
        /// <response code="200">Product created successfully.</response>
        /// <response code="400">Invalid request data or validation errors.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="409">Product name already exists.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost(ProductRoutes.AddProduct)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SucessDetailsExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProblemDetailsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(ProblemDetailsUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ProblemDetailsConflictExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ProblemDetailsInternalServerErrorExample))]
        public async Task<IActionResult> AddProduct([FromBody] ProductPayLoadDTO productDTO, [FromServices] IServiceProvider serviceProvider)
        {
            var validationResult = await ValidationHelper.ValidateEntityAsync(productDTO, serviceProvider, this);

            if (validationResult != null)
            {
                return validationResult;
            }

            var product = AuthenticationLoginProfileMapperInitializer.Mapper.Map<Product>(productDTO);

            try
            {
                productService.AddProduct(product);
                var productResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ProductResponseDTO>(product);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(productResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
        /// Update product.
        /// </summary>
        /// <param name="productDTO">Updated product data.</param>
        /// <param name="serviceProvider">Service provider for dependency injection.</param>
        /// <returns>Updated product information.</returns>
        /// <response code="200">Product updated successfully.</response>
        /// <response code="400">Invalid request data or validation errors.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Product not found.</response>
        /// <response code="409">Product name already exists.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut(ProductRoutes.UpdateProduct)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProductResponseDTO))]
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
        public async Task<IActionResult> UpdateProduct([FromBody] ProductPayLoadDTO productDTO, [FromServices] IServiceProvider serviceProvider)
        {
            try
            {
                var existingProduct = productService.GetById(productDTO.Id);

                if (existingProduct == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.AccountNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var validationResult = await ValidationHelper.ValidateEntityAsync(productDTO, serviceProvider, this);
                if (validationResult != null)
                {
                    return validationResult;
                }

                // Preserve CreatedBy and DtCreated before mapping
                var originalCreatedBy = existingProduct.CreatedBy;
                var originalDtCreated = existingProduct.DtCreated;
                
                AuthenticationLoginProfileMapperInitializer.Mapper.Map(productDTO, existingProduct);
                
                // Restore preserved values
                existingProduct.CreatedBy = originalCreatedBy;
                existingProduct.DtCreated = originalDtCreated;

                productService.UpdateProduct(existingProduct);

                var productResponse = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ProductResponseDTO>(existingProduct);
                var successResponse = SuccessResponseExampleFactory.ForSuccess(productResponse, ResourceAPI.RequestWasSuccessful, HttpContext.Request.Path);
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
        /// Delete product.
        /// </summary>
        /// <param name="id">ID of the product to delete.</param>
        /// <returns>Deleted product details.</returns>
        /// <response code="200">Product deleted successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Product not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete(ProductRoutes.DeleteProduct)]
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
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var existingProduct = productService.GetById(id);

                if (existingProduct == null)
                {
                    var notFoundDetails = ProblemDetailsExampleFactory.ForNotFound(ResourceAPI.AccountNotFound, HttpContext.Request.Path);
                    return NotFound(notFoundDetails);
                }

                var productResponseDTO = AuthenticationLoginProfileMapperInitializer.Mapper.Map<ProductResponseDTO>(existingProduct);

                productService.DeleteProduct(id);

                var successResponse = new SucessDetails
                {
                    Detail = ResourceAPI.AccountDeletedSuccessfully,
                    Data = productResponseDTO,
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
