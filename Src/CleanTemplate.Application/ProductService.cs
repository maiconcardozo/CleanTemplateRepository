using CleanTemplate.Application.DTOs;
using CleanTemplate.Application.Services;
using CleanTemplate.Domain.Entities;

namespace CleanTemplate.Application.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products = new();
    private readonly object _lock = new();

    public Task<IEnumerable<ProductResponseDTO>> GetAllProductsAsync()
    {
        lock (_lock)
        {
            var result = _products.Select(MapToResponseDTO);
            return Task.FromResult(result);
        }
    }

    public Task<ProductResponseDTO?> GetProductByIdAsync(Guid id)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product != null ? MapToResponseDTO(product) : null);
        }
    }

    public Task<ProductResponseDTO> CreateProductAsync(ProductPayloadDTO productPayload)
    {
        lock (_lock)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = productPayload.Name,
                Description = productPayload.Description,
                Price = productPayload.Price,
                Stock = productPayload.Stock,
                IsActive = productPayload.IsActive,
                CreatedBy = productPayload.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            _products.Add(product);
            return Task.FromResult(MapToResponseDTO(product));
        }
    }

    public Task<ProductResponseDTO?> UpdateProductAsync(Guid id, ProductPayloadDTO productPayload)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return Task.FromResult<ProductResponseDTO?>(null);

            product.Name = productPayload.Name;
            product.Description = productPayload.Description;
            product.Price = productPayload.Price;
            product.Stock = productPayload.Stock;
            product.IsActive = productPayload.IsActive;
            product.UpdatedBy = productPayload.UpdatedBy;
            product.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult<ProductResponseDTO?>(MapToResponseDTO(product));
        }
    }

    public Task<bool> DeleteProductAsync(Guid id)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return Task.FromResult(false);

            _products.Remove(product);
            return Task.FromResult(true);
        }
    }

    private static ProductResponseDTO MapToResponseDTO(Product product)
    {
        return new ProductResponseDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CreatedBy = product.CreatedBy,
            CreatedAt = product.CreatedAt,
            UpdatedBy = product.UpdatedBy,
            UpdatedAt = product.UpdatedAt
        };
    }
}