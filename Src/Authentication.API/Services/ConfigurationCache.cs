using Authentication.Login.Constants;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Authentication.API.Services
{
    public interface IConfigurationCache
    {
        string GetConnectionString(string connectionName = ApplicationConstants.DefaultConnectionStringName);
        IConfigurationSection GetJwtSettings();
    }

    public class ConfigurationCache : IConfigurationCache
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheExpirationTime = TimeSpan.FromMinutes(30);

        public ConfigurationCache(IConfiguration configuration, IMemoryCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        public string GetConnectionString(string connectionName = ApplicationConstants.DefaultConnectionStringName)
        {
            string cacheKey = $"ConnectionString_{connectionName}";
            
            if (!_cache.TryGetValue(cacheKey, out string? connectionString))
            {
                connectionString = _configuration.GetConnectionString(connectionName) ?? string.Empty;
                _cache.Set(cacheKey, connectionString, _cacheExpirationTime);
            }

            return connectionString ?? string.Empty;
        }

        public IConfigurationSection GetJwtSettings()
        {
            string cacheKey = "JwtSettings";
            
            if (!_cache.TryGetValue(cacheKey, out IConfigurationSection? jwtSettings))
            {
                jwtSettings = _configuration.GetSection(ApplicationConstants.JwtSettingsSection);
                _cache.Set(cacheKey, jwtSettings, _cacheExpirationTime);
            }

            return jwtSettings!;
        }
    }
}