using System.Collections.Generic;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Enum;

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
