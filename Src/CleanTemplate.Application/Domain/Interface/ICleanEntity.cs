using Foundation.Base.Domain.Interface;

namespace CleanTemplate.Application.Domain.Interface
{
    /// <summary>
    /// Represents a clean entity interface that extends the base entity functionality.
    /// This is a template entity demonstrating the standard patterns used in this project.
    /// </summary>
    public interface ICleanEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the name of the clean entity.
        /// This is a simple string property to demonstrate basic entity properties.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the description of the clean entity.
        /// Used to provide additional details about the entity instance.
        /// </summary>
        public string Description { get; set; }
    }
}