using Authentication.Login.Repository.Interface;
using Authentication.Login.UnitOfWork.Interface;
using Foundation.Base.UnitOfWork.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.UnitOfWork.Implementation
{
    public class LoginUnitOfWork : BaseUnitOfWork, ILoginUnitOfWork
    {
        public IEntityTemplateExampleRepository EntityTemplateExampleRepository { get; }

        public LoginUnitOfWork(
            DbContext context,
            IEntityTemplateExampleRepository entityTemplateExampleRepository)
            : base(context)
        {
            EntityTemplateExampleRepository = entityTemplateExampleRepository;
        }
    }
}
