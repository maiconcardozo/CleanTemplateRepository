using CleanTemplate.Application.Repository.Interface;
using Foundation.Base.UnitOfWork.Interface;

namespace CleanTemplate.Application.UnitOfWork.Interface
{
    public interface IApplicationUnitOfWork : IBaseUnitOfWork
    {
        ICleanEntityRepository CleanEntityRepository { get; }
    }
}