using Authentication.Login.Constants;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Authentication.Login.Services.Interface;
using System.Linq.Expressions;

namespace Authentication.Login.Services.Implementation
{
    /// <summary>
    /// Service implementation for CleanEntity management operations.
    /// Provides business logic and data access coordination for CleanEntity operations.
    /// </summary>
    public class CleanEntityService : ICleanEntityService
    {
        private readonly ICleanEntityRepository _cleanEntityRepository;

        /// <summary>
        /// Initializes a new instance of the CleanEntityService.
        /// </summary>
        /// <param name="cleanEntityRepository">Repository for clean entity data operations</param>
        public CleanEntityService(ICleanEntityRepository cleanEntityRepository)
        {
            _cleanEntityRepository = cleanEntityRepository;
        }

        #region Query Operations

        /// <summary>
        /// Retrieves all clean entities from the system.
        /// </summary>
        /// <returns>Collection of all clean entity entities</returns>
        public IEnumerable<CleanEntity> GetAllCleanEntities() => _cleanEntityRepository.GetAll();

        /// <summary>
        /// Finds a clean entity by name.
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>CleanEntity if found, null otherwise</returns>
        public CleanEntity? GetCleanEntityByName(string name) => _cleanEntityRepository.GetByName(name);

        /// <summary>
        /// Retrieves a clean entity by its unique identifier.
        /// </summary>
        /// <param name="id">CleanEntity ID</param>
        /// <returns>CleanEntity if found, null otherwise</returns>
        public CleanEntity? GetById(int id) => _cleanEntityRepository.GetById(id);

        /// <summary>
        /// Retrieves multiple clean entities by their IDs.
        /// </summary>
        /// <param name="cleanEntityIds">Collection of clean entity IDs</param>
        /// <returns>Collection of matching clean entity entities</returns>
        public IEnumerable<CleanEntity> GetCleanEntitiesByIds(IEnumerable<int> cleanEntityIds) 
        {
            var result = new List<CleanEntity>();
            foreach (var id in cleanEntityIds)
            {
                var entity = _cleanEntityRepository.GetById(id);
                if (entity != null)
                {
                    result.Add(entity);
                }
            }
            return result;
        }

        /// <summary>
        /// Retrieves all clean entity entities (alias for GetAllCleanEntities).
        /// </summary>
        /// <returns>Collection of all clean entity entities</returns>
        public IEnumerable<CleanEntity> GetAllCleanEntityEntities() => GetAllCleanEntities();

        /// <summary>
        /// Retrieves clean entities that match the specified predicate condition.
        /// </summary>
        /// <param name="predicate">LINQ expression to filter clean entities</param>
        /// <returns>Collection of matching clean entity entities</returns>
        public IEnumerable<CleanEntity> GetCleanEntities(Expression<Func<CleanEntity, bool>> predicate) 
            => _cleanEntityRepository.Find(predicate);

        /// <summary>
        /// Retrieves a single clean entity that matches the predicate, or null if none found.
        /// </summary>
        /// <param name="predicate">LINQ expression to filter clean entities</param>
        /// <returns>Single matching clean entity or null</returns>
        /// <exception cref="InvalidOperationException">Thrown when multiple clean entities match the predicate</exception>
        public CleanEntity? GetSingleOrDefaultCleanEntity(Expression<Func<CleanEntity, bool>> predicate) 
            => _cleanEntityRepository.SingleOrDefault(predicate);

        /// <summary>
        /// Retrieves all active clean entities.
        /// </summary>
        /// <returns>Collection of active clean entities</returns>
        public IEnumerable<CleanEntity> GetAllActiveCleanEntities() => _cleanEntityRepository.GetAllActive();

        #endregion

        #region Modification Operations

        /// <summary>
        /// Creates a new clean entity in the system.
        /// Validates name uniqueness and sets audit fields.
        /// </summary>
        /// <param name="cleanEntity">CleanEntity to create</param>
        /// <exception cref="ConflictException">Thrown when name already exists</exception>
        public void AddCleanEntity(CleanEntity cleanEntity)
        {
            // Check for duplicate names
            var existingEntity = _cleanEntityRepository.GetByName(cleanEntity.Name);
            if (existingEntity != null)
            {
                throw new InvalidOperationException($"CleanEntity with name '{cleanEntity.Name}' already exists.");
            }

            // Set audit fields for tracking when and by whom the clean entity was created
            cleanEntity.DtCreated = DateTime.Now;
            // Use the CreatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(cleanEntity.CreatedBy))
            {
                cleanEntity.CreatedBy = ApplicationConstants.DefaultCreatedByUser;
            }
            
            _cleanEntityRepository.Add(cleanEntity);
        }

        /// <summary>
        /// Creates multiple clean entities in a single transaction.
        /// </summary>
        /// <param name="cleanEntities">Collection of clean entity entities to create</param>
        public void AddCleanEntities(IEnumerable<CleanEntity> cleanEntities)
        {
            foreach (var cleanEntity in cleanEntities)
            {
                // Set audit fields for each entity
                cleanEntity.DtCreated = DateTime.Now;
                if (string.IsNullOrEmpty(cleanEntity.CreatedBy))
                {
                    cleanEntity.CreatedBy = ApplicationConstants.DefaultCreatedByUser;
                }
            }
            
            _cleanEntityRepository.AddRange(cleanEntities);
        }

        /// <summary>
        /// Updates an existing clean entity.
        /// </summary>
        /// <param name="cleanEntity">CleanEntity with updated information</param>
        public void UpdateCleanEntity(CleanEntity cleanEntity)
        {
            // Update audit fields for tracking modifications
            cleanEntity.DtUpdated = DateTime.Now;
            // Use the UpdatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(cleanEntity.UpdatedBy))
            {
                cleanEntity.UpdatedBy = ApplicationConstants.DefaultCreatedByUser;
            }
            
            _cleanEntityRepository.Update(cleanEntity);
        }

        /// <summary>
        /// Deletes a clean entity.
        /// </summary>
        /// <param name="cleanEntity">CleanEntity to delete</param>
        public void DeleteCleanEntity(CleanEntity cleanEntity)
        {
            _cleanEntityRepository.Remove(cleanEntity);
        }

        /// <summary>
        /// Deletes a clean entity by its ID.
        /// </summary>
        /// <param name="id">CleanEntity ID to delete</param>
        public void DeleteCleanEntity(int id)
        {
            var cleanEntity = _cleanEntityRepository.GetById(id);
            if (cleanEntity != null)
            {
                _cleanEntityRepository.Remove(cleanEntity);
            }
        }

        /// <summary>
        /// Deletes multiple clean entity entities.
        /// </summary>
        /// <param name="cleanEntities">Collection of clean entity entities to delete</param>
        public void DeleteCleanEntities(IEnumerable<CleanEntity> cleanEntities)
        {
            _cleanEntityRepository.RemoveRange(cleanEntities);
        }

        /// <summary>
        /// Deletes clean entities by their names.
        /// </summary>
        /// <param name="names">Collection of names to delete</param>
        public void DeleteCleanEntitiesByNames(IEnumerable<string> names)
        {
            _cleanEntityRepository.DeleteByNameList(names);
        }

        #endregion
    }
}