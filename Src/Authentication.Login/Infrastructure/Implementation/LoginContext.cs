using Authentication.Login.Domain.Implementation;
using Authentication.Login.Infrastructure.Implementation;
using Authentication.Login.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Action = Authentication.Login.Domain.Implementation.Action;

namespace Authentication.Login.Infrastructure.Data
{
    public class LoginContext : DbContext, ILoginContext
    {
        public LoginContext(DbContextOptions<LoginContext> options)
            : base(options)
        {
        }

        public DbSet<Account> DbAccount { get; set; }

        public DbSet<Claim> DbClaim { get; set; }

        public DbSet<Action> DbAction { get; set; }

        public DbSet<ClaimAction> DbClaimAction { get; set; }

        public DbSet<AccountClaimAction> DbAccountClaimAction { get; set; }

        public DbSet<Application> DbApplication { get; set; }

        public DbSet<ApplicationClaim> DbApplicationClaim { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            LoadModel(modelBuilder);
        }

        public static void LoadModel(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountMap());
            modelBuilder.ApplyConfiguration(new ClaimMap());
            modelBuilder.ApplyConfiguration(new ActionMap());
            modelBuilder.ApplyConfiguration(new ClaimActionMap());
            modelBuilder.ApplyConfiguration(new AccountClaimActionMap());
            modelBuilder.ApplyConfiguration(new ApplicationMap());
            modelBuilder.ApplyConfiguration(new ApplicationClaimMap());
        }
    }
}
