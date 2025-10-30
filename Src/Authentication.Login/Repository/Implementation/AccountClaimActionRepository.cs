using System.Collections.Generic;
using System.Linq;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Repository.Implementation
{
    public class AccountClaimActionRepository : EntityRepository<AccountClaimAction>, IAccountClaimActionRepository
    {
        private readonly DbContext context;

        public AccountClaimActionRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }

        public IEnumerable<AccountClaimAction> GetByIdAccount(int idAccount)
        {
            return context.Set<AccountClaimAction>()
                .Include(aca => aca.ClaimAction)
                    .ThenInclude(ca => ca.Claim)
                .Include(aca => aca.ClaimAction)
                    .ThenInclude(ca => ca.Action)
                .Where(aca => aca.IdAccount == idAccount)
                .ToList();
        }

        public IEnumerable<AccountClaimAction> GetByIdClaimAction(int idClaimAction)
        {
            return context.Set<AccountClaimAction>()
                .Include(aca => aca.Account)
                .Where(aca => aca.IdClaimAction == idClaimAction)
                .ToList();
        }

        public AccountClaimAction? GetByAccountAndClaimAction(int idAccount, int idClaimAction)
        {
            return context.Set<AccountClaimAction>()
                .FirstOrDefault(aca => aca.IdAccount == idAccount && aca.IdClaimAction == idClaimAction);
        }
    }
}
