using Microsoft.Extensions.Configuration;

namespace Authentication.API.Data
{
    internal class ApiContextDevelopment : BaseApiContext
    {
        public ApiContextDevelopment(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}
