using CleanTemplate.API.Resource;

namespace CleanTemplate.API.Swagger
{
    /// <summary>
    /// Route constants for CleanEntity API endpoints.
    /// Uses resource files for localization and consistency.
    /// </summary>
    public static class CleanEntityRoutes
    {
        /// <summary>
        /// Route for getting all clean entities.
        /// </summary>
        public const string GetCleanEntities = "GetCleanEntities";
        
        /// <summary>
        /// Route for getting a clean entity by ID.
        /// </summary>
        public const string GetCleanEntityById = "GetCleanEntityById/{id}";
        
        /// <summary>
        /// Route for adding a new clean entity.
        /// </summary>
        public const string AddCleanEntity = "AddCleanEntity";
        
        /// <summary>
        /// Route for updating an existing clean entity.
        /// </summary>
        public const string UpdateCleanEntity = "UpdateCleanEntity/{id}";
        
        /// <summary>
        /// Route for deleting a clean entity.
        /// </summary>
        public const string DeleteCleanEntity = "DeleteCleanEntity/{id}";
    }
}