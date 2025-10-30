using Authentication.Login.Domain.Interface;
using Foundation.Base.Domain.Implementation;
using System.Diagnostics;

namespace Authentication.Login.Domain.Implementation
{
    [DebuggerDisplay("IdClaim={IdClaim}, IdAction={IdAction}, Id={Id}")]
    public class ClaimAction : Entity, IClaimAction
    {
        public int IdClaim { get; set; }

        public Claim Claim { get; set; } = null!;

        public int IdAction { get; set; }

        public Action Action { get; set; } = null!;

        public ICollection<AccountClaimAction> LstAccountClaimAction { get; set; } = new List<AccountClaimAction>();

        IClaim IClaimAction.Claim
        {
            get => Claim;
            set => Claim = (Claim)value;
        }

        IAction IClaimAction.Action
        {
            get => Action;
            set => Action = (Action)value;
        }

        ICollection<IAccountClaimAction> IClaimAction.LstAccountClaimAction
        {
            get => (ICollection<IAccountClaimAction>)LstAccountClaimAction;
            set => LstAccountClaimAction = (ICollection<AccountClaimAction>)value;
        }
    }
}
