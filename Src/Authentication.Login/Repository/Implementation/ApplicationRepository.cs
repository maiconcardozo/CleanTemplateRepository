using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Repository.Implementation
{
    public class ApplicationRepository : EntityRepository<Application>, IApplicationRepository
    {
        private readonly DbContext context;

        public ApplicationRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }

        public Application? GetByName(string name)
        {
            return context.Set<Application>().FirstOrDefault(a => a.Name == name);
        }

        public IEnumerable<Application> GetAllActive()
        {
            return context.Set<Application>().Where(a => a.IsActive).ToList();
        }
    }
}
