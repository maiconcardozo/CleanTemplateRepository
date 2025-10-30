using Foundation.Base.Domain.Interface;

namespace Authentication.Login.Domain.Interface
{
    /// <summary>
    /// Represents a product interface that extends the base entity functionality.
    /// Defines the contract for product objects with standard properties.
    /// </summary>
    public interface IProduct : IEntity
    {
        /// <summary>
        /// Gets or sets the unique product name.
        /// Used for product identification.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// Provides detailed information about the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// Represents the monetary value of the product.
        /// </summary>
        public decimal Price { get; set; }
    }
}
