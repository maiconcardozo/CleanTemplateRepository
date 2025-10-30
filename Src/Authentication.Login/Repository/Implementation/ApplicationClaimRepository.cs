using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Repository.Implementation
{
    public class ApplicationClaimRepository : EntityRepository<ApplicationClaim>, IApplicationClaimRepository
    {
        private readonly DbContext context;

        public ApplicationClaimRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }

        public IEnumerable<ApplicationClaim> GetByApplicationId(int applicationId)
        {
            return context.Set<ApplicationClaim>()
                .Where(ac => ac.IdApplication == applicationId && ac.IsActive)
                .ToList();
        }

        public IEnumerable<ApplicationClaim> GetByClaimId(int claimId)
        {
            return context.Set<ApplicationClaim>()
                .Where(ac => ac.IdClaim == claimId && ac.IsActive)
                .ToList();
        }

        public ApplicationClaim? GetByApplicationAndClaim(int applicationId, int claimId)
        {
            return context.Set<ApplicationClaim>()
                .FirstOrDefault(ac => ac.IdApplication == applicationId && ac.IdClaim == claimId);
        }
    }
}
