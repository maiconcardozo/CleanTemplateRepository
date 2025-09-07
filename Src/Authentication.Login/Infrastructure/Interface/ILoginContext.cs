using Authentication.Login.Domain.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Infrastructure.Interface
{
    public interface ILoginContext
    {
        public DbSet<Account> dbAccount { get; set; }
    }
}
