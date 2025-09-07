using Authentication.Login.Domain.Implementation;
using Foundation.Base.Repository.Interface;
using System.Collections.Generic;

namespace Authentication.Login.Repository.Interface
{
    public interface IClaimActionRepository : IEntityRepository<ClaimAction>
    {
        ClaimAction? GetByClaimAndAction(int idClaim, int idAction);
        IEnumerable<ClaimAction> GetByClaimId(int idClaim);
        IEnumerable<ClaimAction> GetByActionId(int idAction);
    }
}