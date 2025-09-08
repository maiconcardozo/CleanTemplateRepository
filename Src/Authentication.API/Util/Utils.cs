using Authentication.Login.Constants;

namespace Authentication.API.Helper
{
    public static class Utils
    {
        public static string GetConnectionString(IConfigurationBuilder appsettings, string connectionName = ApplicationConstants.DefaultConnectionStringName)
        {
            // Se estiver rodando em ambiente de teste, sempre retorna InMemoryDbForTesting
            var isTest = AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.FullName.StartsWith("xunit", StringComparison.OrdinalIgnoreCase) ||
                          a.FullName.StartsWith("nunit", StringComparison.OrdinalIgnoreCase) ||
                          a.FullName.StartsWith("Microsoft.VisualStudio.TestPlatform", StringComparison.OrdinalIgnoreCase));
            if (isTest)
                return "InMemoryDbForTesting";

            var configuration = appsettings.Build();
            return configuration.GetConnectionString(connectionName) ?? string.Empty;
        }


    }
}
