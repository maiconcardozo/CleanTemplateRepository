using CleanTemplate.Application.Domain.Interface;
using Foundation.Base.Domain.Implementation;

namespace CleanTemplate.Application.Domain.Implementation
{
    /// <summary>
    /// Represents a clean entity implementation in the authentication system.
    /// This entity serves as a template demonstrating the standard patterns used in this project.
    /// Inherits from Entity base class providing audit fields and implements ICleanEntity interface.
    /// </summary>
    public class CleanEntity : Entity, ICleanEntity
    {
        /// <summary>
        /// Gets or sets the name of the clean entity.
        /// This property demonstrates basic string property handling with validation support.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the description of the clean entity.
        /// Used to provide additional details about the entity instance.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}