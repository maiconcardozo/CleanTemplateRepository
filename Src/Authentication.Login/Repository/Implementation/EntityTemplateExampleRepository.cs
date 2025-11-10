using Authentication.Login.Domain.Implementation;
using Authentication.Login.Domain.Interface;
using Authentication.Login.Infrastructure.Interface;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Repository.Implementation
{
    /// <summary>
    /// Repository implementation for EntityTemplateExample entity.
    /// Provides data access operations for template example entities.
    /// </summary>
    public class EntityTemplateExampleRepository : EntityRepository<EntityTemplateExample>, IEntityTemplateExampleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTemplateExampleRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for data operations.</param>
        public EntityTemplateExampleRepository(DbContext context)
            : base(context)
        {
        }
    }
}
