using CleanTemplate.Application.Domain.Implementation;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Application.Infrastructure.Interface
{
    public interface IApplicationContext
    {
        public DbSet<CleanEntity> dbCleanEntity { get; set; }
    }
}
