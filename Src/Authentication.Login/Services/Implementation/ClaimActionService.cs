using System.Collections.Generic;
using Authentication.Login.Constants;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Authentication.Login.Services.Interface;

namespace Authentication.Login.Services.Implementation
{
    public class ClaimActionService : IClaimActionService
    {
        private readonly IClaimActionRepository claimActionRepository;

        public ClaimActionService(IClaimActionRepository claimActionRepository)
        {
            this.claimActionRepository = claimActionRepository;
        }

        public IEnumerable<ClaimAction> GetAll() => claimActionRepository.GetAll();

        public ClaimAction? GetById(int id) => claimActionRepository.GetById(id);

        public ClaimAction? GetByClaimAndAction(int idClaim, int idAction) =>
            claimActionRepository.GetByClaimAndAction(idClaim, idAction);

        public IEnumerable<ClaimAction> GetByClaimId(int idClaim) =>
            claimActionRepository.GetByClaimId(idClaim);

        public IEnumerable<ClaimAction> GetByActionId(int idAction) =>
            claimActionRepository.GetByActionId(idAction);

        public void AddClaimAction(ClaimAction claimAction)
        {
            // Set audit fields for tracking when and by whom the claim action was created
            claimAction.DtCreated = DateTime.Now;

            // Use the CreatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(claimAction.CreatedBy))
            {
                claimAction.CreatedBy = ApplicationConstants.DefaultCreatedByUser;
            }

            claimActionRepository.Add(claimAction);
        }

        public void UpdateClaimAction(ClaimAction claimAction)
        {
            // Update audit fields for tracking modifications
            claimAction.DtUpdated = DateTime.Now;

            // Use the UpdatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(claimAction.UpdatedBy))
            {
                claimAction.UpdatedBy = ApplicationConstants.DefaultCreatedByUser;
            }

            claimActionRepository.Update(claimAction);
        }

        public void DeleteClaimAction(int id)
        {
            var entity = claimActionRepository.GetById(id);
            if (entity != null)
            {
                claimActionRepository.Remove(entity);
            }
        }
    }
}
