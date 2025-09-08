using Microsoft.Extensions.Configuration;

namespace CleanTemplate.API.Data
{
    public class ApiContextProduction : BaseApiContext
    {
        public ApiContextProduction(IConfiguration configuration) : base(configuration)
        {
        }
    }
}