using Authentication.Login.Constants;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Authentication.Login.Services.Interface;
using System.Collections.Generic;

namespace Authentication.Login.Services.Implementation
{
    public class AccountClaimActionService : IAccountClaimActionService
    {
        private readonly IAccountClaimActionRepository _repo;

        public AccountClaimActionService(IAccountClaimActionRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<AccountClaimAction> GetByIdAccount(int idAccount) =>
            _repo.GetByIdAccount(idAccount);

        public IEnumerable<AccountClaimAction> GetByIdClaimAction(int idClaimAction) =>
            _repo.GetByIdClaimAction(idClaimAction);

        public AccountClaimAction? GetByAccountAndClaimAction(int idAccount, int idClaimAction) =>
            _repo.GetByAccountAndClaimAction(idAccount, idClaimAction);

        public void AddAccountClaimAction(AccountClaimAction accountClaimAction)
        {
            // Set audit fields for tracking when and by whom the account claim action was created
            accountClaimAction.DtCreated = DateTime.Now;
            // Use the CreatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(accountClaimAction.CreatedBy))
            {
                accountClaimAction.CreatedBy = ApplicationConstants.DefaultCreatedByUser;
            }
            
            _repo.Add(accountClaimAction);
        }

        public void UpdateAccountClaimAction(AccountClaimAction accountClaimAction)
        {
            // Update audit fields for tracking modifications
            accountClaimAction.DtUpdated = DateTime.Now;
            // Use the UpdatedBy value from the entity/DTO instead of a default value
            if (string.IsNullOrEmpty(accountClaimAction.UpdatedBy))
            {
                accountClaimAction.UpdatedBy = ApplicationConstants.DefaultCreatedByUser;
            }
            
            _repo.Update(accountClaimAction);
        }

        public void DeleteAccountClaimAction(int id)
        {
            var entity = _repo.GetById(id);
            if (entity != null)
                _repo.Remove(entity);
        }
    }
}