using Authentication.Login.Infrastructure.Data;
using Authentication.Login.Repository.Implementation;
using Authentication.Login.Repository.Interface;
using Authentication.Login.Services.Implementation;
using Authentication.Login.Services.Interface;
using Authentication.Login.UnitOfWork.Implementation;
using Authentication.Login.UnitOfWork.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql;

namespace Authentication.Login.Extensions
{
    public static class AuthenticationLoginServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationLoginServices(this IServiceCollection services, string connectionString)
        {
            // Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IClaimActionService, ClaimActionService>();
            services.AddScoped<IAccountClaimActionService, AccountClaimActionService>();
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<IApplicationClaimService, ApplicationClaimService>();

            // Repositories
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IClaimRepository, ClaimRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IClaimActionRepository, ClaimActionRepository>();
            services.AddScoped<IAccountClaimActionRepository, AccountClaimActionRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IApplicationClaimRepository, ApplicationClaimRepository>();

            // Unit of Work
            services.AddScoped<ILoginUnitOfWork, LoginUnitOfWork>();

            services.AddScoped<DbContext>(provider => provider.GetRequiredService<LoginContext>());

            if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("InMemoryDbForTesting", StringComparison.OrdinalIgnoreCase))
            {
                services.AddDbContext<LoginContext>(options => options.UseInMemoryDatabase("InMemoryDbForTesting"));
                return services;
            }

            services.AddDbContext<LoginContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            return services;
        }
    }
}
