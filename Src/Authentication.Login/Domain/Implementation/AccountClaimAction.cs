using Authentication.Login.Domain.Interface;
using Foundation.Base.Domain.Implementation;

namespace Authentication.Login.Domain.Implementation
{
    public class AccountClaimAction : Entity, IAccountClaimAction
    {
        public int IdAccount { get; set; }
        public Account Account { get; set; } = null!;
        public int IdClaimAction { get; set; }
        public ClaimAction ClaimAction { get; set; } = null!;

        IAccount IAccountClaimAction.Account
        {
            get => Account;
            set => Account = (Account)value;
        }

        IClaimAction IAccountClaimAction.ClaimAction
        {
            get => ClaimAction;
            set => ClaimAction = (ClaimAction)value;
        }
    }
}