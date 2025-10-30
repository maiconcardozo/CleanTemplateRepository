public static IServiceCollection AddAuthenticationLoginServices(this IServiceCollection services, string connectionString, string environment = null)
{
    // Se estiver em ambiente de desenvolvimento ou debug, use banco em memória
    if (!string.IsNullOrEmpty(environment) && (environment.Equals("Development", StringComparison.OrdinalIgnoreCase) || environment.Equals("Debug", StringComparison.OrdinalIgnoreCase)))
    {
        services.AddDbContext<LoginContext>(options => options.UseInMemoryDatabase("InMemoryDbForTesting"));
    }
    // Check if we're in test environment to avoid MySQL connection
    else if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("InMemoryDbForTesting"))
    {
        // Use in-memory database for testing
        services.AddDbContext<LoginContext>(options => options.UseInMemoryDatabase("InMemoryDbForTesting"));
    }
    else
    {
        // Use MySQL for production/development
        services.AddDbContext<LoginContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    }
    
    services.AddScoped<DbContext>(provider => provider.GetRequiredService<LoginContext>());

    // Services
    services.AddScoped<IAccountService, AccountService>();
    services.AddScoped<IClaimService, ClaimService>();
    services.AddScoped<IActionService, ActionService>();
    services.AddScoped<IClaimActionService, ClaimActionService>();
    services.AddScoped<IAccountClaimActionService, AccountClaimActionService>();

    // Repositories
    services.AddScoped<IAccountRepository, AccountRepository>();
    services.AddScoped<IClaimRepository, ClaimRepository>();
    services.AddScoped<IActionRepository, ActionRepository>();
    services.AddScoped<IClaimActionRepository, ClaimActionRepository>();
    services.AddScoped<IAccountClaimActionRepository, AccountClaimActionRepository>();

    // Unit of Work
    services.AddScoped<ILoginUnitOfWork, LoginUnitOfWork>();

    return services;
}