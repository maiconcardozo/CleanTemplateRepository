using Authentication.Login.Domain.Implementation;
using Foundation.Base.Repository.Interface;

namespace Authentication.Login.Repository.Interface
{
    public interface IProductVariantRepository : IEntityRepository<ProductVariant>
    {
        IEnumerable<ProductVariant> GetByIdProduct(int idProduct);

        ProductVariant? GetBySKU(string sku);

        Task<ProductVariant?> GetBySKUAsync(string sku);

        IEnumerable<ProductVariant> GetByIdProductList(IEnumerable<int> productIds);

        void DeleteByIdProduct(int idProduct);
    }
}
