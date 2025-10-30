using Microsoft.Extensions.Configuration;

namespace Authentication.API.Data
{
    internal class ApiContextProduction : BaseApiContext
    {
        public ApiContextProduction(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}
