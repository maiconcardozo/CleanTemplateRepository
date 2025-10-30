using Authentication.Login.Domain.Implementation;
using Foundation.Base.Repository.Interface;

namespace Authentication.Login.Repository.Interface
{
    public interface IProductRepository : IEntityRepository<Product>
    {
        Product? GetByName(string name);

        Task<Product?> GetByNameAsync(string name);

        IEnumerable<Product> GetByNameList(IEnumerable<string> names);

        void UpdateName(string oldName, string newName);

        void UpdatePrice(int productId, decimal newPrice);

        void DeleteByName(string name);

        void DeleteByNameList(IEnumerable<string> names);
    }
}
