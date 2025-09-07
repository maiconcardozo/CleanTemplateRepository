using Microsoft.Extensions.Configuration;

namespace Authentication.API.Data
{
    public class ApiContextDevelopment : BaseApiContext
    {
        public ApiContextDevelopment(IConfiguration configuration) : base(configuration)
        {
        }
    }
}