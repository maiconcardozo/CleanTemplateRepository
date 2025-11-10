using Authentication.Login.Domain.Implementation;

namespace Authentication.Login.Services.Interface
{
    /// <summary>
    /// Service interface for EntityTemplateExample business logic.
    /// Defines the contract for business operations on template example entities.
    /// </summary>
    public interface IEntityTemplateExampleService
    {
        /// <summary>
        /// Gets all EntityTemplateExample entities.
        /// </summary>
        /// <returns>Collection of all entities.</returns>
        IEnumerable<EntityTemplateExample> GetAll();

        /// <summary>
        /// Gets an EntityTemplateExample by ID.
        /// </summary>
        /// <param name="id">Entity ID.</param>
        /// <returns>The entity or null if not found.</returns>
        EntityTemplateExample? GetById(int id);

        /// <summary>
        /// Adds a new EntityTemplateExample entity.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        void Add(EntityTemplateExample entity);

        /// <summary>
        /// Updates an existing EntityTemplateExample entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        void Update(EntityTemplateExample entity);

        /// <summary>
        /// Deletes an EntityTemplateExample entity by ID.
        /// </summary>
        /// <param name="id">Entity ID to delete.</param>
        void Delete(int id);
    }
}
