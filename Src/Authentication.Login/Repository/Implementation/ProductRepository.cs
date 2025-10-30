using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Repository.Implementation
{
    public class ProductRepository : EntityRepository<Product>, IProductRepository
    {
        private readonly DbContext context;

        public ProductRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }

        public Product? GetByName(string name)
        {
            return Context.Set<Product>().FirstOrDefault(p => p.Name == name);
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await Context.Set<Product>().FirstOrDefaultAsync(p => p.Name == name);
        }

        public IEnumerable<Product> GetByNameList(IEnumerable<string> names)
        {
            return Context.Set<Product>().Where(p => names.Contains(p.Name)).ToList();
        }

        public void UpdateName(string oldName, string newName)
        {
            var product = GetByName(oldName);
            if (product != null)
            {
                product.Name = newName;
                Context.Set<Product>().Update(product);
            }
        }

        public void UpdatePrice(int productId, decimal newPrice)
        {
            var product = GetById(productId);
            if (product != null)
            {
                product.Price = newPrice;
                Context.Set<Product>().Update(product);
            }
        }

        public void DeleteByName(string name)
        {
            var product = GetByName(name);
            if (product != null)
            {
                Remove(product); // Use base soft delete method
            }
        }

        public void DeleteByNameList(IEnumerable<string> names)
        {
            var products = GetByNameList(names);
            if (products != null && products.Any())
            {
                RemoveRange(products); // Use base soft delete method
            }
        }
    }
}
