using System.Collections.Generic;

namespace Authentication.Login.Domain.Interface
{
    public interface IAction
    {
        int Id { get; set; }

        string Name { get; set; }

        ICollection<IClaimAction> LstClaimAction { get; set; }
    }
}
