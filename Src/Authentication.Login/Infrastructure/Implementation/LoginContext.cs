using Authentication.Login.Domain.Implementation;
using Authentication.Login.Infrastructure.Implementation;
using Authentication.Login.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Action = Authentication.Login.Domain.Implementation.Action;

namespace Authentication.Login.Infrastructure.Data
{
    public class LoginContext : DbContext, ILoginContext
    {
        public LoginContext(DbContextOptions<LoginContext> options) : base(options)
        {
        }

        public DbSet<Account> dbAccount { get; set; }
        public DbSet<Claim> dbClaim { get; set; }
        public DbSet<Action> dbAction { get; set; }
        public DbSet<ClaimAction> dbClaimAction { get; set; }
        public DbSet<AccountClaimAction> dbAccountClaimAction { get; set; }

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
        }
    }
}