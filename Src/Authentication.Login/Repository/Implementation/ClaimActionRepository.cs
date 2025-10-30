using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Repository.Implementation
{
    public class ClaimActionRepository : EntityRepository<ClaimAction>, IClaimActionRepository
    {
        private readonly DbContext context;

        public ClaimActionRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }

        public ClaimAction? GetByClaimAndAction(int idClaim, int idAction)
        {
            return context.Set<ClaimAction>().FirstOrDefault(ca => ca.IdClaim == idClaim && ca.IdAction == idAction);
        }

        public IEnumerable<ClaimAction> GetByClaimId(int idClaim)
        {
            return context.Set<ClaimAction>().Where(ca => ca.IdClaim == idClaim).ToList();
        }

        public IEnumerable<ClaimAction> GetByActionId(int idAction)
        {
            return context.Set<ClaimAction>().Where(ca => ca.IdAction == idAction).ToList();
        }
    }
}
