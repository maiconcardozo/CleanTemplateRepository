using System.Collections.Generic;
using Authentication.Login.Domain.Implementation;
using Foundation.Base.Repository.Interface;

namespace Authentication.Login.Repository.Interface
{
    public interface IApplicationClaimRepository : IEntityRepository<ApplicationClaim>
    {
        IEnumerable<ApplicationClaim> GetByApplicationId(int applicationId);

        IEnumerable<ApplicationClaim> GetByClaimId(int claimId);

        ApplicationClaim? GetByApplicationAndClaim(int applicationId, int claimId);
    }
}
