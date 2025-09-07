using CleanTemplate.Application.DTOs;

namespace CleanTemplate.Application.Services;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDTO>> GetAllProductsAsync();
    Task<ProductResponseDTO?> GetProductByIdAsync(Guid id);
    Task<ProductResponseDTO> CreateProductAsync(ProductPayloadDTO productPayload);
    Task<ProductResponseDTO?> UpdateProductAsync(Guid id, ProductPayloadDTO productPayload);
    Task<bool> DeleteProductAsync(Guid id);
}