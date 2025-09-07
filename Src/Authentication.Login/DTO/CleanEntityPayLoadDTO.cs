namespace Authentication.Login.DTO
{
    /// <summary>
    /// Data Transfer Object for CleanEntity payload operations.
    /// Used for creating and updating CleanEntity instances.
    /// </summary>
    public class CleanEntityPayLoadDTO
    {
        /// <summary>
        /// Gets or sets the name of the clean entity.
        /// This is a required field and must be unique across the system.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the description of the clean entity.
        /// Optional field providing additional details about the entity.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the user who created this entity.
        /// Used for audit trail purposes.
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the user who last updated this entity.
        /// Used for audit trail purposes during updates.
        /// </summary>
        public string? UpdatedBy { get; set; }
    }
}