using Authentication.Login.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Authentication.API.Data
{
    public abstract class BaseApiContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        protected BaseApiContext(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    if (connectionString.Contains("InMemoryDbForTesting"))
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