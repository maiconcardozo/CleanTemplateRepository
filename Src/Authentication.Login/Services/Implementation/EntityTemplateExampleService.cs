using Authentication.Login.Domain.Implementation;
using Authentication.Login.Services.Interface;
using Authentication.Login.UnitOfWork.Interface;

namespace Authentication.Login.Services.Implementation
{
    /// <summary>
    /// Service implementation for EntityTemplateExample business logic.
    /// Provides business operations for template example entities.
    /// </summary>
    public class EntityTemplateExampleService : IEntityTemplateExampleService
    {
        private readonly ILoginUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTemplateExampleService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for transaction management.</param>
        public EntityTemplateExampleService(ILoginUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EntityTemplateExample> GetAll()
        {
            return unitOfWork.EntityTemplateExampleRepository.GetAll();
        }

        public EntityTemplateExample? GetById(int id)
        {
            return unitOfWork.EntityTemplateExampleRepository.GetById(id);
        }

        public void Add(EntityTemplateExample entity)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.EntityTemplateExampleRepository.Add(entity);
            });
        }

        public void Update(EntityTemplateExample entity)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                unitOfWork.EntityTemplateExampleRepository.Update(entity);
            });
        }

        public void Delete(int id)
        {
            unitOfWork.ExecuteInTransaction(() =>
            {
                var entity = unitOfWork.EntityTemplateExampleRepository.GetById(id);
                if (entity != null)
                {
                    unitOfWork.EntityTemplateExampleRepository.Remove(entity);
                }
            });
        }
    }
}
