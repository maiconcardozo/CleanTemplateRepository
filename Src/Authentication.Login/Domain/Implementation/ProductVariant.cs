using Authentication.Login.Domain.Interface;
using Foundation.Base.Domain.Implementation;
using System.Diagnostics;

namespace Authentication.Login.Domain.Implementation
{
    /// <summary>
    /// Represents a product variant entity that extends the base entity functionality.
    /// Provides variant-specific properties like SKU, color, size, and stock quantity.
    /// </summary>
    [DebuggerDisplay("SKU={SKU}, IdProduct={IdProduct}, Id={Id}")]
    public class ProductVariant : Entity, IProductVariant
    {
        /// <summary>
        /// Gets or sets the product identifier this variant belongs to.
        /// Foreign key to the Product entity.
        /// </summary>
        public int IdProduct { get; set; }

        /// <summary>
        /// Gets or sets the associated product entity.
        /// Navigation property for Entity Framework.
        /// </summary>
        public Product Product { get; set; } = null!;

        /// <summary>
        /// Gets or sets the variant SKU (Stock Keeping Unit).
        /// Unique identifier for inventory management.
        /// </summary>
        public string SKU { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the color of the product variant.
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the size of the product variant.
        /// </summary>
        public string Size { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the stock quantity available.
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Explicit interface implementation for IProduct to allow covariance.
        /// </summary>
        IProduct IProductVariant.Product
        {
            get => Product;
            set => Product = (Product)value;
        }
    }
}
