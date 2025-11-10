using Foundation.Base.Domain.Interface;

namespace Authentication.Login.Domain.Interface
{
    /// <summary>
    /// Represents a template example entity interface that extends the base entity functionality.
    /// Defines the contract for example entity objects with various property types for demonstration.
    /// </summary>
    public interface IEntityTemplateExample : IEntity
    {
        /// <summary>
        /// Gets or sets the first example property (string type).
        /// Used for demonstrating string properties in the template.
        /// </summary>
        public string Pro1 { get; set; }

        /// <summary>
        /// Gets or sets the second example property (integer type).
        /// Used for demonstrating integer properties in the template.
        /// </summary>
        public int Pro2 { get; set; }

        /// <summary>
        /// Gets or sets the third example property (decimal type).
        /// Used for demonstrating decimal properties in the template.
        /// </summary>
        public decimal Pro3 { get; set; }

        /// <summary>
        /// Gets or sets the fourth example property (DateTime type).
        /// Used for demonstrating DateTime properties in the template.
        /// </summary>
        public DateTime Pro4 { get; set; }

        /// <summary>
        /// Gets or sets the fifth example property (boolean type).
        /// Used for demonstrating boolean properties in the template.
        /// </summary>
        public bool Pro5 { get; set; }
    }
}
