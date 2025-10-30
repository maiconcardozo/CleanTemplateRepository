using Authentication.Login.Domain.Interface;
using Foundation.Base.Domain.Implementation;
using System.Diagnostics;

namespace Authentication.Login.Domain.Implementation
{
    /// <summary>
    /// Represents a product entity in the system.
    /// Inherits from Entity base class providing audit fields and implements IProduct interface.
    /// </summary>
    [DebuggerDisplay("Name={Name}, Id={Id}")]
    public class Product : Entity, IProduct
    {
        /// <summary>
        /// Gets or sets the unique product name.
        /// Must be unique across the system and is used for identification.
        /// Cannot be null/empty.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the product.
        /// Provides detailed information about the product.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the price of the product.
        /// Represents the monetary value of the product.
        /// </summary>
        public decimal Price { get; set; }
    }
}
