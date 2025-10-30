using Authentication.Login.Domain.Implementation;
using System.Linq.Expressions;

namespace Authentication.Login.Services.Interface
{
    /// <summary>
    /// Service interface for product variant management operations.
    /// Defines contract for product variant CRUD operations and business logic.
    /// </summary>
    public interface IProductVariantService
    {
        IEnumerable<ProductVariant> GetAllProductVariants();

        ProductVariant? GetById(int id);

        ProductVariant? GetBySKU(string sku);

        Task<ProductVariant?> GetBySKUAsync(string sku);

        IEnumerable<ProductVariant> GetByIdProduct(int idProduct);

        IEnumerable<ProductVariant> GetProductVariantsByIds(IEnumerable<int> variantIds);

        IEnumerable<ProductVariant> GetProductVariants(Expression<Func<ProductVariant, bool>> predicate);

        ProductVariant? GetSingleOrDefaultProductVariant(Expression<Func<ProductVariant, bool>> predicate);

        void AddProductVariant(ProductVariant variant);

        void AddProductVariants(IEnumerable<ProductVariant> variants);

        void UpdateProductVariant(ProductVariant variant);

        void DeleteProductVariant(int id);

        void DeleteProductVariant(ProductVariant variant);

        void DeleteProductVariants(IEnumerable<ProductVariant> variants);

        void DeleteByIdProduct(int idProduct);
    }
}
