using CleanTemplate.Application.DTOs;
using CleanTemplate.Application.Services;

namespace CleanTemplate.Tests;

public class ProductServiceTests
{
    private readonly IProductService _productService;

    public ProductServiceTests()
    {
        _productService = new ProductService();
    }

    [Fact]
    public async Task CreateProduct_Should_Return_CreatedProduct()
    {
        // Arrange
        var productPayload = new ProductPayloadDTO
        {
            Name = "Test Product",
            Description = "A test product",
            Price = 29.99m,
            Stock = 100,
            IsActive = true,
            CreatedBy = "TestUser"
        };

        // Act
        var result = await _productService.CreateProductAsync(productPayload);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(productPayload.Name, result.Name);
        Assert.Equal(productPayload.Description, result.Description);
        Assert.Equal(productPayload.Price, result.Price);
        Assert.Equal(productPayload.Stock, result.Stock);
        Assert.Equal(productPayload.IsActive, result.IsActive);
        Assert.Equal(productPayload.CreatedBy, result.CreatedBy);
        Assert.True(result.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task GetAllProducts_Should_Return_EmptyList_Initially()
    {
        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetProductById_Should_Return_Null_For_NonExistentProduct()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _productService.GetProductByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAndRetrieveProduct_Should_Work_Successfully()
    {
        // Arrange
        var productPayload = new ProductPayloadDTO
        {
            Name = "Integration Test Product",
            Description = "Product for integration testing",
            Price = 99.99m,
            Stock = 50,
            IsActive = true,
            CreatedBy = "IntegrationTest"
        };

        // Act
        var createdProduct = await _productService.CreateProductAsync(productPayload);
        var retrievedProduct = await _productService.GetProductByIdAsync(createdProduct.Id);
        var allProducts = await _productService.GetAllProductsAsync();

        // Assert
        Assert.NotNull(retrievedProduct);
        Assert.Equal(createdProduct.Id, retrievedProduct.Id);
        Assert.Equal(createdProduct.Name, retrievedProduct.Name);
        Assert.Single(allProducts);
        Assert.Contains(allProducts, p => p.Id == createdProduct.Id);
    }
}
