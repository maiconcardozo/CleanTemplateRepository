using System.Collections.Generic;
using Authentication.Login.Constants;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Authentication.Login.Services.Interface;

namespace Authentication.Login.Services.Implementation
{
    public class ApplicationClaimService : IApplicationClaimService
    {
        private readonly IApplicationClaimRepository applicationClaimRepository;

        public ApplicationClaimService(IApplicationClaimRepository applicationClaimRepository)
        {
            this.applicationClaimRepository = applicationClaimRepository;
        }

        public IEnumerable<ApplicationClaim> GetAll() => applicationClaimRepository.GetAll();

        public ApplicationClaim? GetById(int id) => applicationClaimRepository.GetById(id);

        public IEnumerable<ApplicationClaim> GetByApplicationId(int applicationId) =>
            applicationClaimRepository.GetByApplicationId(applicationId);

        public IEnumerable<ApplicationClaim> GetByClaimId(int claimId) =>
            applicationClaimRepository.GetByClaimId(claimId);

        public void AddApplicationClaim(ApplicationClaim applicationClaim)
        {
            // Verify if the mapping already exists
            var existing = applicationClaimRepository.GetByApplicationAndClaim(
                applicationClaim.IdApplication,
                applicationClaim.IdClaim);

            if (existing != null)
            {
                throw new InvalidOperationException("Application-Claim mapping already exists.");
            }

            // Set audit fields for tracking when and by whom the mapping was created
            applicationClaim.DtCreated = DateTime.Now;

            // Use the CreatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(applicationClaim.CreatedBy))
            {
                applicationClaim.CreatedBy = ApplicationConstants.DefaultCreatedByUser;
            }

            applicationClaimRepository.Add(applicationClaim);
        }

        public void UpdateApplicationClaim(ApplicationClaim applicationClaim)
        {
            // Update audit fields for tracking modifications
            applicationClaim.DtUpdated = DateTime.Now;

            // Use the UpdatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(applicationClaim.UpdatedBy))
            {
                applicationClaim.UpdatedBy = ApplicationConstants.DefaultCreatedByUser;
            }

            applicationClaimRepository.Update(applicationClaim);
        }

        public void DeleteApplicationClaim(int id)
        {
            var applicationClaim = applicationClaimRepository.GetById(id);
            if (applicationClaim != null)
            {
                applicationClaimRepository.Remove(applicationClaim);
            }
        }
    }
}
