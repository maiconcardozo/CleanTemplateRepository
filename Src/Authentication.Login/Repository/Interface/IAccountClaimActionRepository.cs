using System.Collections.Generic;
using Authentication.Login.Domain.Implementation;
using Foundation.Base.Repository.Interface;

namespace Authentication.Login.Repository.Interface
{
    public interface IAccountClaimActionRepository : IEntityRepository<AccountClaimAction>
    {
        IEnumerable<AccountClaimAction> GetByIdAccount(int idAccount);

        IEnumerable<AccountClaimAction> GetByIdClaimAction(int idClaimAction);

        AccountClaimAction? GetByAccountAndClaimAction(int idAccount, int idClaimAction);
    }
}
