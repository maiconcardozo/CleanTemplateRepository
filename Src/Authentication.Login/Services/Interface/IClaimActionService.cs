using Authentication.Login.Domain.Implementation;
using System.Collections.Generic;

namespace Authentication.Login.Services.Interface
{
    public interface IClaimActionService
    {
        IEnumerable<ClaimAction> GetAll();
        ClaimAction? GetById(int id);
        ClaimAction? GetByClaimAndAction(int idClaim, int idAction);
        IEnumerable<ClaimAction> GetByClaimId(int idClaim);
        IEnumerable<ClaimAction> GetByActionId(int idAction);
        void AddClaimAction(ClaimAction claimAction);
        void UpdateClaimAction(ClaimAction claimAction);
        void DeleteClaimAction(int id);
    }
}