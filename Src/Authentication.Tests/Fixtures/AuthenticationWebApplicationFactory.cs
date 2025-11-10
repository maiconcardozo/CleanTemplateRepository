using Authentication.Login.Domain.Implementation;
using Authentication.Login.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", "InMemoryDbForTesting"),
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

        // Add test data for EntityTemplateExample
        context.DbEntityTemplateExample.Add(new EntityTemplateExample
        {
            Id = 1,
            Pro1 = "Test Example",
            Pro2 = 100,
            Pro3 = 99.99m,
            Pro4 = DateTime.Now,
            Pro5 = true,
        });

        context.SaveChanges();
    }
}
