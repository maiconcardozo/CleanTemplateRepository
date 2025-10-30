using Authentication.Login.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Authentication.API.Data
{
    internal abstract class BaseApiContext : DbContext
    {
        private readonly IConfiguration configuration;

        protected BaseApiContext(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    if (connectionString.Contains("InMemoryDbForTesting", StringComparison.OrdinalIgnoreCase))
                    {
                        optionsBuilder.UseInMemoryDatabase("InMemoryDbForTesting");
                    }
                    else
                    {
                        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            LoginContext.LoadModel(modelBuilder);
        }
    }
}
