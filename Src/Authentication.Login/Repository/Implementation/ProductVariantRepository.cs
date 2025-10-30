using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Repository.Implementation
{
    public class ProductVariantRepository : EntityRepository<ProductVariant>, IProductVariantRepository
    {
        private readonly DbContext context;

        public ProductVariantRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }

        public IEnumerable<ProductVariant> GetByIdProduct(int idProduct)
        {
            return Context.Set<ProductVariant>()
                .Include(pv => pv.Product)
                .Where(pv => pv.IdProduct == idProduct)
                .ToList();
        }

        public ProductVariant? GetBySKU(string sku)
        {
            return Context.Set<ProductVariant>()
                .Include(pv => pv.Product)
                .FirstOrDefault(pv => pv.SKU == sku);
        }

        public async Task<ProductVariant?> GetBySKUAsync(string sku)
        {
            return await Context.Set<ProductVariant>()
                .Include(pv => pv.Product)
                .FirstOrDefaultAsync(pv => pv.SKU == sku);
        }

        public IEnumerable<ProductVariant> GetByIdProductList(IEnumerable<int> productIds)
        {
            return Context.Set<ProductVariant>()
                .Include(pv => pv.Product)
                .Where(pv => productIds.Contains(pv.IdProduct))
                .ToList();
        }

        public void DeleteByIdProduct(int idProduct)
        {
            var variants = GetByIdProduct(idProduct);
            if (variants != null && variants.Any())
            {
                RemoveRange(variants); // Use base soft delete method
            }
        }
    }
}
