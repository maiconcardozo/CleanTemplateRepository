using CleanTemplate.Application.Domain.Implementation;
using Foundation.Base.Repository.Interface;

namespace CleanTemplate.Application.Repository.Interface
{
    /// <summary>
    /// Repository interface for CleanEntity management operations.
    /// Provides access to CleanEntity data layer operations following the repository pattern.
    /// </summary>
    public interface ICleanEntityRepository : IEntityRepository<CleanEntity>
    {
        /// <summary>
        /// Retrieves a clean entity by its name.
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>CleanEntity if found, null otherwise</returns>
        CleanEntity? GetByName(string name);
        
        /// <summary>
        /// Retrieves all active clean entities.
        /// </summary>
        /// <returns>Collection of active clean entities</returns>
        IEnumerable<CleanEntity> GetAllActive();
        
        /// <summary>
        /// Retrieves clean entities by a list of names.
        /// </summary>
        /// <param name="names">Collection of names to search for</param>
        /// <returns>Collection of matching clean entities</returns>
        IEnumerable<CleanEntity> GetByNameList(IEnumerable<string> names);
        
        /// <summary>
        /// Deletes clean entities by their names.
        /// </summary>
        /// <param name="names">Collection of names to delete</param>
        void DeleteByNameList(IEnumerable<string> names);
    }
}