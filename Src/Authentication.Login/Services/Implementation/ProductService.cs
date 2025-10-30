using System.Linq.Expressions;
using Authentication.Login.Constants;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Exceptions;
using Authentication.Login.Resource;
using Authentication.Login.Services.Interface;
using Authentication.Login.UnitOfWork.Interface;

namespace Authentication.Login.Services.Implementation
{
    /// <summary>
    /// Service implementation for product management operations.
    /// Handles product CRUD operations with transaction management and business validations.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly ILoginUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work for transaction management and repository access.</param>
        public ProductService(ILoginUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return unitOfWork.ProductRepository.GetAll();
        }

        public Product? GetProductByName(string name)
        {
            return unitOfWork.ProductRepository.GetByName(name);
        }

        public async Task<Product?> GetProductByNameAsync(string name)
        {
            return await unitOfWork.ProductRepository.GetByNameAsync(name);
        }

        public Product? GetById(int id)
        {
            return unitOfWork.ProductRepository.GetById(id);
        }

        public IEnumerable<Product> GetProductsByIds(IEnumerable<int> productIds)
        {
            return unitOfWork.ProductRepository.Find(p => productIds.Contains(p.Id));
        }

        public IEnumerable<Product> GetProducts(Expression<Func<Product, bool>> predicate)
        {
            return unitOfWork.ProductRepository.Find(predicate);
        }

        public Product? GetSingleOrDefaultProduct(Expression<Func<Product, bool>> predicate)
        {
            return unitOfWork.ProductRepository.Find(predicate).SingleOrDefault();
        }

        /// <summary>
        /// Creates a new product with business validations.
        /// Performs name uniqueness check and audit field population.
        /// </summary>
        /// <param name="product">Product entity to create.</param>
        /// <exception cref="ConflictException">When product name already exists in the system.</exception>
        public void AddProduct(Product product)
        {
            // Business rule: Ensure product name uniqueness across the system
            var existingProduct = unitOfWork.ProductRepository.GetByName(product.Name);
            if (existingProduct != null)
            {
                throw new ConflictException(ResourceLogin.DuplicateUserName); // Reusing existing resource string
            }

            // Set audit fields for tracking
            product.DtCreated = DateTime.Now;

            if (string.IsNullOrEmpty(product.CreatedBy))
            {
                product.CreatedBy = ApplicationConstants.DefaultCreatedByUser;
            }

            // Execute within transaction
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductRepository.Add(product);
            });
        }

        public void AddProducts(IEnumerable<Product> products)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductRepository.AddRange(products);
            });
        }

        /// <summary>
        /// Updates an existing product with validation.
        /// Handles name uniqueness validation when name is changed.
        /// </summary>
        /// <param name="product">Product entity with updated information.</param>
        /// <exception cref="ConflictException">When the new name is already taken by another product.</exception>
        public void UpdateProduct(Product product)
        {
            // Business rule: Ensure name uniqueness when updating
            var existingProduct = unitOfWork.ProductRepository.GetByName(product.Name);

            if (existingProduct != null && existingProduct.Id != product.Id)
            {
                throw new ConflictException(ResourceLogin.DuplicateUserName); // Reusing existing resource string
            }

            // Update audit fields
            product.DtUpdated = DateTime.Now;

            if (string.IsNullOrEmpty(product.UpdatedBy))
            {
                product.UpdatedBy = ApplicationConstants.DefaultCreatedByUser;
            }

            // Execute update within transaction
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductRepository.Update(product);
            });
        }

        public void UpdateProductPrice(int productId, decimal newPrice)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductRepository.UpdatePrice(productId, newPrice);
            });
        }

        public void UpdateProductName(string oldName, string newName)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductRepository.UpdateName(oldName, newName);
            });
        }

        public void DeleteProduct(int id)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                var product = unitOfWork.ProductRepository.GetById(id);
                if (product != null)
                {
                    unitOfWork.ProductRepository.Remove(product);
                }
            });
        }

        public void DeleteProduct(Product product)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductRepository.Remove(product);
            });
        }

        public void DeleteProducts(IEnumerable<Product> products)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductRepository.RemoveRange(products);
            });
        }

        public void DeleteProductByName(string name)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductRepository.DeleteByName(name);
            });
        }

        public void DeleteProductsByNames(IEnumerable<string> names)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductRepository.DeleteByNameList(names);
            });
        }
    }
}
