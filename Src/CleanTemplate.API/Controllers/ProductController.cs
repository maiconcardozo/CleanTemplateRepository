using CleanTemplate.Application.DTOs;
using CleanTemplate.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CleanTemplate.API.Controllers;

/// <summary>
/// Product management controller providing CRUD operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    /// <summary>
    /// Initializes a new instance of the ProductController.
    /// </summary>
    /// <param name="productService">Service for product management operations.</param>
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Get all products.
    /// </summary>
    /// <returns>List of all products in the system.</returns>
    /// <response code="200">Products retrieved successfully.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetAllProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get product by ID.
    /// </summary>
    /// <param name="id">Product unique identifier.</param>
    /// <returns>Product details.</returns>
    /// <response code="200">Product found and returned.</response>
    /// <response code="404">Product not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductResponseDTO>> GetProductById(Guid id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product == null ? NotFound($"Product with ID {id} not found") : Ok(product);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new product.
    /// </summary>
    /// <param name="productPayload">Product creation data.</param>
    /// <returns>Created product.</returns>
    /// <response code="201">Product created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductResponseDTO>> CreateProduct([FromBody] ProductPayloadDTO productPayload)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productService.CreateProductAsync(productPayload);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing product.
    /// </summary>
    /// <param name="id">Product unique identifier.</param>
    /// <param name="productPayload">Product update data.</param>
    /// <returns>Updated product.</returns>
    /// <response code="200">Product updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Product not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProductResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductResponseDTO>> UpdateProduct(Guid id, [FromBody] ProductPayloadDTO productPayload)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productService.UpdateProductAsync(id, productPayload);
            return product == null ? NotFound($"Product with ID {id} not found") : Ok(product);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a product.
    /// </summary>
    /// <param name="id">Product unique identifier.</param>
    /// <returns>Confirmation of deletion.</returns>
    /// <response code="204">Product deleted successfully.</response>
    /// <response code="404">Product not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        try
        {
            var deleted = await _productService.DeleteProductAsync(id);
            return !deleted ? NotFound($"Product with ID {id} not found") : NoContent();
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}