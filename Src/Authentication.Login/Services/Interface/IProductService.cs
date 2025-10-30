using Authentication.Login.Domain.Implementation;
using System.Linq.Expressions;

namespace Authentication.Login.Services.Interface
{
    /// <summary>
    /// Service interface for product management operations.
    /// Defines contract for product CRUD operations and business logic.
    /// </summary>
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();

        Product? GetProductByName(string name);

        Task<Product?> GetProductByNameAsync(string name);

        Product? GetById(int id);

        IEnumerable<Product> GetProductsByIds(IEnumerable<int> productIds);

        IEnumerable<Product> GetProducts(Expression<Func<Product, bool>> predicate);

        Product? GetSingleOrDefaultProduct(Expression<Func<Product, bool>> predicate);

        void AddProduct(Product product);

        void AddProducts(IEnumerable<Product> products);

        void UpdateProduct(Product product);

        void UpdateProductPrice(int productId, decimal newPrice);

        void UpdateProductName(string oldName, string newName);

        void DeleteProduct(int id);

        void DeleteProduct(Product product);

        void DeleteProducts(IEnumerable<Product> products);

        void DeleteProductByName(string name);

        void DeleteProductsByNames(IEnumerable<string> names);
    }
}
