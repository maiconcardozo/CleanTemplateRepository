using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Interface;
using Foundation.Base.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Repository.Implementation
{
    public class ActionRepository : EntityRepository<Domain.Implementation.Action>, IActionRepository
    {
        private readonly DbContext context;

        public ActionRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }

        public Domain.Implementation.Action? GetByName(string name)
        {
            return context.Set<Domain.Implementation.Action>().FirstOrDefault(a => a.Name == name);
        }

        public IEnumerable<Domain.Implementation.Action> GetAllActive()
        {
            return context.Set<Domain.Implementation.Action>().Where(a => a.IsActive).ToList();
        }
    }
}
