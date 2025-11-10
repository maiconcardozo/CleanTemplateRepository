using Authentication.Login.Domain.Interface;
using Foundation.Base.Domain.Implementation;
using System.Diagnostics;

namespace Authentication.Login.Domain.Implementation
{
    /// <summary>
    /// Represents a template example entity in the system.
    /// Inherits from Entity base class providing audit fields and implements IEntityTemplateExample interface.
    /// This entity serves as a clean template with various property types for demonstration purposes.
    /// </summary>
    [DebuggerDisplay("Pro1={Pro1}, Id={Id}")]
    public class EntityTemplateExample : Entity, IEntityTemplateExample
    {
        /// <summary>
        /// Gets or sets the first example property (string type).
        /// Used for demonstrating string properties in the template.
        /// </summary>
        public string Pro1 { get; set; } = string.Empty;

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
