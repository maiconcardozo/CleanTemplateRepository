using Authentication.Login.Repository.Interface;
using Foundation.Base.UnitOfWork.Interface;

namespace Authentication.Login.UnitOfWork.Interface
{
    public interface ILoginUnitOfWork : IBaseUnitOfWork
    {
        IAccountRepository AccountRepository { get; }

        IClaimRepository ClaimRepository { get; }

        IActionRepository ActionRepository { get; }

        IClaimActionRepository ClaimActionRepository { get; }

        IAccountClaimActionRepository AccountClaimActionRepository { get; }

        IProductRepository ProductRepository { get; }

        IProductVariantRepository ProductVariantRepository { get; }
    }
}
