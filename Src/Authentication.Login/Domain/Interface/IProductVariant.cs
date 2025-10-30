namespace Authentication.Login.Domain.Interface
{
    /// <summary>
    /// Represents a product variant interface.
    /// Defines the contract for product variant objects with relationships to products.
    /// </summary>
    public interface IProductVariant
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the product identifier this variant belongs to.
        /// </summary>
        int IdProduct { get; set; }

        /// <summary>
        /// Gets or sets the associated product.
        /// </summary>
        IProduct Product { get; set; }

        /// <summary>
        /// Gets or sets the variant SKU (Stock Keeping Unit).
        /// Unique identifier for inventory management.
        /// </summary>
        string SKU { get; set; }

        /// <summary>
        /// Gets or sets the color of the product variant.
        /// </summary>
        string Color { get; set; }

        /// <summary>
        /// Gets or sets the size of the product variant.
        /// </summary>
        string Size { get; set; }

        /// <summary>
        /// Gets or sets the stock quantity available.
        /// </summary>
        int StockQuantity { get; set; }
    }
}
