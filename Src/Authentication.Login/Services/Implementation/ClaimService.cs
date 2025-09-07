using Authentication.Login.Constants;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Authentication.Login.Services.Interface;
using System.Collections.Generic;

namespace Authentication.Login.Services.Implementation
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepository;

        public ClaimService(IClaimRepository claimRepository)
        {
            _claimRepository = claimRepository;
        }

        public IEnumerable<Claim> GetAll() => _claimRepository.GetAll();

        public Claim? GetById(int id) => _claimRepository.GetById(id);

        public Claim? GetByValue(string value) => _claimRepository.GetByValue(value);

        public void AddClaim(Claim claim)
        {
            // Set audit fields for tracking when and by whom the claim was created
            claim.DtCreated = DateTime.Now;
            // Use the CreatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(claim.CreatedBy))
            {
                claim.CreatedBy = ApplicationConstants.DefaultCreatedByUser;
            }
            
            _claimRepository.Add(claim);
        }

        public void UpdateClaim(Claim claim)
        {
            // Update audit fields for tracking modifications
            claim.DtUpdated = DateTime.Now;
            // Use the UpdatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(claim.UpdatedBy))
            {
                claim.UpdatedBy = ApplicationConstants.DefaultCreatedByUser;
            }
            
            _claimRepository.Update(claim);
        }

        public void DeleteClaim(int id)
        {
            var claim = _claimRepository.GetById(id);
            if (claim != null)
                _claimRepository.Remove(claim);
        }
    }
}