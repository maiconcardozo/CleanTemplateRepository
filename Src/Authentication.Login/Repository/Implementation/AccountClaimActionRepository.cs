using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Authentication.Login.Repository.Implementation
{
    public class AccountClaimActionRepository : EntityRepository<AccountClaimAction>, IAccountClaimActionRepository
    {
        private readonly DbContext _context;

        public AccountClaimActionRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<AccountClaimAction> GetByIdAccount(int idAccount)
        {
            return _context.Set<AccountClaimAction>()
                .Include(aca => aca.ClaimAction)
                    .ThenInclude(ca => ca.Claim)
                .Include(aca => aca.ClaimAction)
                    .ThenInclude(ca => ca.Action)
                .Where(aca => aca.IdAccount == idAccount)
                .ToList();
        }

        public IEnumerable<AccountClaimAction> GetByIdClaimAction(int idClaimAction)
        {
            return _context.Set<AccountClaimAction>()
                .Include(aca => aca.Account)
                .Where(aca => aca.IdClaimAction == idClaimAction)
                .ToList();
        }

        public AccountClaimAction? GetByAccountAndClaimAction(int idAccount, int idClaimAction)
        {
            return _context.Set<AccountClaimAction>()
                .FirstOrDefault(aca => aca.IdAccount == idAccount && aca.IdClaimAction == idClaimAction);
        }
    }
}