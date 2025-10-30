using System.Collections.Generic;
using Authentication.Login.Constants;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Authentication.Login.Services.Interface;

namespace Authentication.Login.Services.Implementation
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository applicationRepository;

        public ApplicationService(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public IEnumerable<Application> GetAll() => applicationRepository.GetAll();

        public Application? GetById(int id) => applicationRepository.GetById(id);

        public Application? GetByName(string name) => applicationRepository.GetByName(name);

        public void AddApplication(Application application)
        {
            // Set audit fields for tracking when and by whom the application was created
            application.DtCreated = DateTime.Now;

            // Use the CreatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(application.CreatedBy))
            {
                application.CreatedBy = ApplicationConstants.DefaultCreatedByUser;
            }

            applicationRepository.Add(application);
        }

        public void UpdateApplication(Application application)
        {
            // Update audit fields for tracking modifications
            application.DtUpdated = DateTime.Now;

            // Use the UpdatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(application.UpdatedBy))
            {
                application.UpdatedBy = ApplicationConstants.DefaultCreatedByUser;
            }

            applicationRepository.Update(application);
        }

        public void DeleteApplication(int id)
        {
            var application = applicationRepository.GetById(id);
            if (application != null)
            {
                applicationRepository.Remove(application);
            }
        }
    }
}
