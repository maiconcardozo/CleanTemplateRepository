using Authentication.Login.Constants;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Authentication.API.Services
{
    internal interface IConfigurationCache
    {
        string GetConnectionString(string connectionName = ApplicationConstants.DefaultConnectionStringName);

        IConfigurationSection GetJwtSettings();
    }

    internal class ConfigurationCache : IConfigurationCache
    {
        private readonly IConfiguration configuration;
        private readonly IMemoryCache cache;
        private readonly TimeSpan cacheExpirationTime = TimeSpan.FromMinutes(30);

        public ConfigurationCache(IConfiguration configuration, IMemoryCache cache)
        {
            this.configuration = configuration;
            this.cache = cache;
        }

        public string GetConnectionString(string connectionName = ApplicationConstants.DefaultConnectionStringName)
        {
            string cacheKey = $"ConnectionString_{connectionName}";

            if (!cache.TryGetValue(cacheKey, out string? connectionString))
            {
                connectionString = configuration.GetConnectionString(connectionName) ?? string.Empty;
                cache.Set(cacheKey, connectionString, cacheExpirationTime);
            }

            return connectionString ?? string.Empty;
        }

        public IConfigurationSection GetJwtSettings()
        {
            string cacheKey = "JwtSettings";

            if (!cache.TryGetValue(cacheKey, out IConfigurationSection? jwtSettings))
            {
                jwtSettings = configuration.GetSection(ApplicationConstants.JwtSettingsSection);
                cache.Set(cacheKey, jwtSettings, cacheExpirationTime);
            }

            return jwtSettings!;
        }
    }
}
