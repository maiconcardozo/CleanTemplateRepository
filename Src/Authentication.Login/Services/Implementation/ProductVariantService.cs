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
    /// Service implementation for product variant management operations.
    /// Handles product variant CRUD operations with transaction management and business validations.
    /// </summary>
    public class ProductVariantService : IProductVariantService
    {
        private readonly ILoginUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductVariantService"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work for transaction management and repository access.</param>
        public ProductVariantService(ILoginUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<ProductVariant> GetAllProductVariants()
        {
            return unitOfWork.ProductVariantRepository.GetAll();
        }

        public ProductVariant? GetById(int id)
        {
            return unitOfWork.ProductVariantRepository.GetById(id);
        }

        public ProductVariant? GetBySKU(string sku)
        {
            return unitOfWork.ProductVariantRepository.GetBySKU(sku);
        }

        public async Task<ProductVariant?> GetBySKUAsync(string sku)
        {
            return await unitOfWork.ProductVariantRepository.GetBySKUAsync(sku);
        }

        public IEnumerable<ProductVariant> GetByIdProduct(int idProduct)
        {
            return unitOfWork.ProductVariantRepository.GetByIdProduct(idProduct);
        }

        public IEnumerable<ProductVariant> GetProductVariantsByIds(IEnumerable<int> variantIds)
        {
            return unitOfWork.ProductVariantRepository.Find(pv => variantIds.Contains(pv.Id));
        }

        public IEnumerable<ProductVariant> GetProductVariants(Expression<Func<ProductVariant, bool>> predicate)
        {
            return unitOfWork.ProductVariantRepository.Find(predicate);
        }

        public ProductVariant? GetSingleOrDefaultProductVariant(Expression<Func<ProductVariant, bool>> predicate)
        {
            return unitOfWork.ProductVariantRepository.Find(predicate).SingleOrDefault();
        }

        /// <summary>
        /// Creates a new product variant with business validations.
        /// Performs SKU uniqueness check and audit field population.
        /// </summary>
        /// <param name="variant">ProductVariant entity to create.</param>
        /// <exception cref="ConflictException">When SKU already exists in the system.</exception>
        public void AddProductVariant(ProductVariant variant)
        {
            // Business rule: Ensure SKU uniqueness across the system
            var existingVariant = unitOfWork.ProductVariantRepository.GetBySKU(variant.SKU);
            if (existingVariant != null)
            {
                throw new ConflictException(ResourceLogin.DuplicateUserName); // Reusing existing resource string
            }

            // Set audit fields for tracking
            variant.DtCreated = DateTime.Now;

            if (string.IsNullOrEmpty(variant.CreatedBy))
            {
                variant.CreatedBy = ApplicationConstants.DefaultCreatedByUser;
            }

            // Execute within transaction
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductVariantRepository.Add(variant);
            });
        }

        public void AddProductVariants(IEnumerable<ProductVariant> variants)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductVariantRepository.AddRange(variants);
            });
        }

        /// <summary>
        /// Updates an existing product variant with validation.
        /// Handles SKU uniqueness validation when SKU is changed.
        /// </summary>
        /// <param name="variant">ProductVariant entity with updated information.</param>
        /// <exception cref="ConflictException">When the new SKU is already taken by another variant.</exception>
        public void UpdateProductVariant(ProductVariant variant)
        {
            // Business rule: Ensure SKU uniqueness when updating
            var existingVariant = unitOfWork.ProductVariantRepository.GetBySKU(variant.SKU);

            if (existingVariant != null && existingVariant.Id != variant.Id)
            {
                throw new ConflictException(ResourceLogin.DuplicateUserName); // Reusing existing resource string
            }

            // Update audit fields
            variant.DtUpdated = DateTime.Now;

            if (string.IsNullOrEmpty(variant.UpdatedBy))
            {
                variant.UpdatedBy = ApplicationConstants.DefaultCreatedByUser;
            }

            // Execute update within transaction
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductVariantRepository.Update(variant);
            });
        }

        public void DeleteProductVariant(int id)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                var variant = unitOfWork.ProductVariantRepository.GetById(id);
                if (variant != null)
                {
                    unitOfWork.ProductVariantRepository.Remove(variant);
                }
            });
        }

        public void DeleteProductVariant(ProductVariant variant)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductVariantRepository.Remove(variant);
            });
        }

        public void DeleteProductVariants(IEnumerable<ProductVariant> variants)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductVariantRepository.RemoveRange(variants);
            });
        }

        public void DeleteByIdProduct(int idProduct)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.ProductVariantRepository.DeleteByIdProduct(idProduct);
            });
        }
    }
}
