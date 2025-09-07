using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Authentication.Login.Infrastructure.Data;
using Authentication.Login.Domain.Implementation;
using Microsoft.Extensions.Configuration;

namespace Authentication.Tests.Fixtures;

public class AuthenticationWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Override connection string for testing
            config.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", "InMemoryDbForTesting")
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove existing LoginContext registrations
            var descriptors = services.Where(d => 
                d.ServiceType == typeof(DbContextOptions<LoginContext>) ||
                d.ServiceType == typeof(LoginContext) ||
                d.ServiceType == typeof(DbContext)).ToList();
                
            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            // Add the in-memory database context
            services.AddDbContext<LoginContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
            
            // Re-add the DbContext service registration
            services.AddScoped<DbContext>(provider => provider.GetRequiredService<LoginContext>());
        });

        builder.UseEnvironment("Testing");
    }

    public void SeedTestData()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LoginContext>();

        // Always reset the database to avoid duplicate key errors
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Add test data for CleanEntity
        context.dbCleanEntity.Add(new CleanEntity
        {
            Id = 1,
            Name = "Test CleanEntity",
            Description = "A test clean entity for demonstration",
            IsActive = true,
            DtCreated = DateTime.UtcNow,
            CreatedBy = "System"
        });

        context.SaveChanges();
    }
}

