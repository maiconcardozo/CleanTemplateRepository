using CleanTemplate.Application.Infrastructure.Data;
using CleanTemplate.Application.Repository.Implementation;
using CleanTemplate.Application.Repository.Interface;
using CleanTemplate.Application.Services.Implementation;
using CleanTemplate.Application.Services.Interface;
using CleanTemplate.Application.UnitOfWork.Implementation;
using CleanTemplate.Application.UnitOfWork.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql;

namespace CleanTemplate.Application.Extensions
{
    public static class CleanTemplateServiceCollectionExtensions
    {
        public static IServiceCollection AddCleanTemplateServices(this IServiceCollection services, string connectionString)
        {
            // Check if we're in test environment to avoid MySQL connection
            if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("InMemoryDbForTesting"))
            {
                // Use in-memory database for testing
                services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("InMemoryDbForTesting"));
            }
            else
            {
                // Use MySQL for production/development
                services.AddDbContext<ApplicationContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            }
            
            services.AddScoped<DbContext>(provider => provider.GetRequiredService<ApplicationContext>());

            // Services
            services.AddScoped<ICleanEntityService, CleanEntityService>();

            // Repositories
            services.AddScoped<ICleanEntityRepository, CleanEntityRepository>();

            // Unit of Work
            services.AddScoped<IApplicationUnitOfWork, ApplicationUnitOfWork>();

            return services;
        }
    }
}