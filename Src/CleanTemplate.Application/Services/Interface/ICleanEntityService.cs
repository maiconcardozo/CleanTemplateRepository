using System.Linq.Expressions;
using CleanTemplate.Application.Domain.Implementation;

namespace CleanTemplate.Application.Services.Interface
{
    /// <summary>
    /// Service interface for CleanEntity management operations.
    /// Provides comprehensive CleanEntity CRUD operations following service layer patterns.
    /// </summary>
    public interface ICleanEntityService
    {
        #region Query Operations
        
        /// <summary>
        /// Retrieves all clean entities from the system.
        /// </summary>
        /// <returns>Collection of all clean entity entities</returns>
        IEnumerable<CleanEntity> GetAllCleanEntities();
        
        /// <summary>
        /// Finds a clean entity by name.
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>CleanEntity if found, null otherwise</returns>
        CleanEntity? GetCleanEntityByName(string name);
        
        /// <summary>
        /// Retrieves a clean entity by its unique identifier.
        /// </summary>
        /// <param name="id">CleanEntity ID</param>
        /// <returns>CleanEntity if found, null otherwise</returns>
        CleanEntity? GetById(int id);
        
        /// <summary>
        /// Retrieves multiple clean entities by their IDs.
        /// </summary>
        /// <param name="cleanEntityIds">Collection of clean entity IDs</param>
        /// <returns>Collection of matching clean entity entities</returns>
        IEnumerable<CleanEntity> GetCleanEntitiesByIds(IEnumerable<int> cleanEntityIds);
        
        /// <summary>
        /// Retrieves all clean entity entities (alias for GetAllCleanEntities).
        /// </summary>
        /// <returns>Collection of all clean entity entities</returns>
        IEnumerable<CleanEntity> GetAllCleanEntityEntities();
        
        /// <summary>
        /// Retrieves clean entities that match the specified predicate condition.
        /// </summary>
        /// <param name="predicate">LINQ expression to filter clean entities</param>
        /// <returns>Collection of matching clean entity entities</returns>
        IEnumerable<CleanEntity> GetCleanEntities(Expression<Func<CleanEntity, bool>> predicate);
        
        /// <summary>
        /// Retrieves a single clean entity that matches the predicate, or null if none found.
        /// </summary>
        /// <param name="predicate">LINQ expression to filter clean entities</param>
        /// <returns>Single matching clean entity or null</returns>
        /// <exception cref="InvalidOperationException">Thrown when multiple clean entities match the predicate</exception>
        CleanEntity? GetSingleOrDefaultCleanEntity(Expression<Func<CleanEntity, bool>> predicate);
        
        /// <summary>
        /// Retrieves all active clean entities.
        /// </summary>
        /// <returns>Collection of active clean entities</returns>
        IEnumerable<CleanEntity> GetAllActiveCleanEntities();
        
        #endregion
        
        #region Modification Operations
        
        /// <summary>
        /// Creates a new clean entity in the system.
        /// Validates name uniqueness and sets audit fields.
        /// </summary>
        /// <param name="cleanEntity">CleanEntity to create</param>
        /// <exception cref="ConflictException">Thrown when name already exists</exception>
        void AddCleanEntity(CleanEntity cleanEntity);
        
        /// <summary>
        /// Creates multiple clean entities in a single transaction.
        /// </summary>
        /// <param name="cleanEntities">Collection of clean entity entities to create</param>
        void AddCleanEntities(IEnumerable<CleanEntity> cleanEntities);
        
        /// <summary>
        /// Updates an existing clean entity.
        /// </summary>
        /// <param name="cleanEntity">CleanEntity with updated information</param>
        void UpdateCleanEntity(CleanEntity cleanEntity);
        
        /// <summary>
        /// Deletes a clean entity.
        /// </summary>
        /// <param name="cleanEntity">CleanEntity to delete</param>
        void DeleteCleanEntity(CleanEntity cleanEntity);
        
        /// <summary>
        /// Deletes a clean entity by its ID.
        /// </summary>
        /// <param name="id">CleanEntity ID to delete</param>
        void DeleteCleanEntity(int id);
        
        /// <summary>
        /// Deletes multiple clean entity entities.
        /// </summary>
        /// <param name="cleanEntities">Collection of clean entity entities to delete</param>
        void DeleteCleanEntities(IEnumerable<CleanEntity> cleanEntities);
        
        /// <summary>
        /// Deletes clean entities by their names.
        /// </summary>
        /// <param name="names">Collection of names to delete</param>
        void DeleteCleanEntitiesByNames(IEnumerable<string> names);
        
        #endregion
    }
}