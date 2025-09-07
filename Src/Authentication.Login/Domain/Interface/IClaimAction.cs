using Authentication.Login.Domain.Implementation;
using System.Collections.Generic;

namespace Authentication.Login.Domain.Interface
{
    public interface IClaimAction
    {
        int Id { get; set; }
        int IdClaim { get; set; }
        IClaim Claim { get; set; }
        int IdAction { get; set; }
        IAction Action { get; set; }
        ICollection<IAccountClaimAction> LstAccountClaimAction { get; set; }
    }
}