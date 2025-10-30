using Authentication.Login.Domain.Implementation;

namespace Authentication.Login.Services.Interface
{
    public interface IAccountClaimActionService
    {
        AccountClaimAction? GetById(int id);

        IEnumerable<AccountClaimAction> GetByIdAccount(int idAccount);

        IEnumerable<AccountClaimAction> GetByIdClaimAction(int idClaimAction);

        AccountClaimAction? GetByAccountAndClaimAction(int idAccount, int idClaimAction);

        void AddAccountClaimAction(AccountClaimAction accountClaimAction);

        void UpdateAccountClaimAction(AccountClaimAction accountClaimAction);

        void DeleteAccountClaimAction(int id);
    }
}
