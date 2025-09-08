using Microsoft.Extensions.Configuration;

namespace CleanTemplate.API.Data
{
    public class ApiContextDevelopment : BaseApiContext
    {
        public ApiContextDevelopment(IConfiguration configuration) : base(configuration)
        {
        }
    }
}