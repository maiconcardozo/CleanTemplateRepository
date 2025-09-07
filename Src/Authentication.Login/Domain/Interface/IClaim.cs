using Authentication.Login.Domain.Implementation;
using Authentication.Login.Enum;
using System.Collections.Generic;

namespace Authentication.Login.Domain.Interface
{
    public interface IClaim
    {
        int Id { get; set; }
        ClaimType Type { get; set; }
        string Value { get; set; }
        string Description { get; set; }
        ICollection<IClaimAction> LstClaimAction { get; set; }
    }
}