using Authentication.Login.Domain.Implementation;
using Authentication.Login.Infrastructure.Implementation;
using Authentication.Login.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Login.Infrastructure.Data
{
    public class LoginContext : DbContext, ILoginContext
    {
        public LoginContext(DbContextOptions<LoginContext> options) : base(options)
        {
        }

        public DbSet<CleanEntity> dbCleanEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            LoadModel(modelBuilder);
        }

        public static void LoadModel(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CleanEntityMap());
        }
    }
}