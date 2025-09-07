using Authentication.Login.Repository.Interface;
using Foundation.Base.UnitOfWork.Interface;

namespace Authentication.Login.UnitOfWork.Interface
{
    public interface ILoginUnitOfWork : IBaseUnitOfWork
    {
        ICleanEntityRepository CleanEntityRepository { get; }
    }
}