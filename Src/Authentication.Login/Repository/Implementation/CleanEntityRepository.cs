using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Repository.Implementation
{
    /// <summary>
    /// Repository implementation for CleanEntity management operations.
    /// Provides concrete data access methods for CleanEntity following the repository pattern.
    /// </summary>
    public class CleanEntityRepository : EntityRepository<CleanEntity>, ICleanEntityRepository
    {
        private readonly DbContext _context;

        /// <summary>
        /// Initializes a new instance of the CleanEntityRepository.
        /// </summary>
        /// <param name="context">Database context for data operations</param>
        public CleanEntityRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a clean entity by its name.
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>CleanEntity if found, null otherwise</returns>
        public CleanEntity? GetByName(string name)
        {
            return _context.Set<CleanEntity>().FirstOrDefault(c => c.Name == name);
        }

        /// <summary>
        /// Retrieves all active clean entities.
        /// </summary>
        /// <returns>Collection of active clean entities</returns>
        public IEnumerable<CleanEntity> GetAllActive()
        {
            return _context.Set<CleanEntity>().Where(c => c.IsActive).ToList();
        }

        /// <summary>
        /// Overrides the base GetById method to provide specific CleanEntity retrieval.
        /// </summary>
        /// <param name="id">The entity ID to search for</param>
        /// <returns>CleanEntity if found, null otherwise</returns>
        public new CleanEntity? GetById(int id)
        {
            return _context.Set<CleanEntity>().FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Retrieves clean entities by a list of names.
        /// </summary>
        /// <param name="names">Collection of names to search for</param>
        /// <returns>Collection of matching clean entities</returns>
        public IEnumerable<CleanEntity> GetByNameList(IEnumerable<string> names)
        {
            return _context.Set<CleanEntity>().Where(c => names.Contains(c.Name)).ToList();
        }

        /// <summary>
        /// Deletes clean entities by their names using soft delete.
        /// </summary>
        /// <param name="names">Collection of names to delete</param>
        public void DeleteByNameList(IEnumerable<string> names)
        {
            var entities = GetByNameList(names);
            if (entities != null && entities.Any())
            {
                RemoveRange(entities); // Use base soft delete method
            }
        }

        /// <summary>
        /// Overrides the base Update method to provide specific CleanEntity update logic.
        /// </summary>
        /// <param name="cleanEntity">CleanEntity to update</param>
        public new void Update(CleanEntity cleanEntity)
        {
            var existingEntity = _context.Set<CleanEntity>().FirstOrDefault(c => c.Id == cleanEntity.Id);
           
            if (existingEntity != null)
            {
                existingEntity.Name = cleanEntity.Name;
                existingEntity.Description = cleanEntity.Description;
                existingEntity.IsActive = cleanEntity.IsActive;
                existingEntity.DtUpdated = DateTime.UtcNow;
                existingEntity.UpdatedBy = cleanEntity.UpdatedBy;
                _context.Update(existingEntity);
                _context.SaveChanges();
            }
        }
    }
}