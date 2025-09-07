using Microsoft.Extensions.Configuration;

namespace Authentication.API.Data
{
    public class ApiContextProduction : BaseApiContext
    {
        public ApiContextProduction(IConfiguration configuration) : base(configuration)
        {
        }
    }
}