using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using CleanTemplate.Application.Infrastructure.Data;
using CleanTemplate.Application.Domain.Implementation;
using Microsoft.Extensions.Configuration;

namespace CleanTemplate.Tests.Fixtures;

public class CleanTemplateWebApplicationFactory : WebApplicationFactory<Program>
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
            // Remove existing ApplicationContext registrations
            var descriptors = services.Where(d => 
                d.ServiceType == typeof(DbContextOptions<ApplicationContext>) ||
                d.ServiceType == typeof(ApplicationContext) ||
                d.ServiceType == typeof(DbContext)).ToList();
                
            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            // Add the in-memory database context
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
            
            // Re-add the DbContext service registration
            services.AddScoped<DbContext>(provider => provider.GetRequiredService<ApplicationContext>());
        });

        builder.UseEnvironment("Testing");
    }

    public void SeedTestData()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

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

