using Authentication.Login.Repository.Interface;
using Authentication.Login.UnitOfWork.Interface;
using Foundation.Base.UnitOfWork.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.UnitOfWork.Implementation
{
    public class LoginUnitOfWork : BaseUnitOfWork, ILoginUnitOfWork
    {
        public IAccountRepository AccountRepository { get; }
        public IClaimRepository ClaimRepository { get; }
        public IActionRepository ActionRepository { get; }
        public IClaimActionRepository ClaimActionRepository { get; }
        public IAccountClaimActionRepository AccountClaimActionRepository { get; }

        public LoginUnitOfWork(
            DbContext context,
            IAccountRepository accountRepository,
            IClaimRepository claimRepository,
            IActionRepository actionRepository,
            IClaimActionRepository claimActionRepository,
            IAccountClaimActionRepository accountClaimActionRepository
        ) : base(context)
        {
            AccountRepository = accountRepository;
            ClaimRepository = claimRepository;
            ActionRepository = actionRepository;
            ClaimActionRepository = claimActionRepository;
            AccountClaimActionRepository = accountClaimActionRepository;
        }
    }
}