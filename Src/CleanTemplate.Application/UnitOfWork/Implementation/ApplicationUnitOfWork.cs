using CleanTemplate.Application.Repository.Interface;
using CleanTemplate.Application.UnitOfWork.Interface;
using Foundation.Base.UnitOfWork.Implementation;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.UnitOfWork.Implementation
{
    public class ApplicationUnitOfWork : BaseUnitOfWork, IApplicationUnitOfWork
    {
        public ICleanEntityRepository CleanEntityRepository { get; }

        public ApplicationUnitOfWork(
            DbContext context,
            ICleanEntityRepository cleanEntityRepository
        ) : base(context)
        {
            CleanEntityRepository = cleanEntityRepository;
        }
    }
}