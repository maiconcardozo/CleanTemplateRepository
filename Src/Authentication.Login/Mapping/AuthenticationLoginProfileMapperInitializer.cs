using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace Authentication.Login.Mapping
{
    public static class AuthenticationLoginProfileMapperInitializer
    {
        private static IMapper? mapper;
        private static MapperConfiguration? config;

        public static void Initialize()
        {
            var configExpr = new MapperConfigurationExpression();
            configExpr.AddProfile<AuthenticationLoginProfileMapping>();

            config = new MapperConfiguration(configExpr, NullLoggerFactory.Instance);
            mapper = config.CreateMapper();
        }

        public static IMapper Mapper
        {
            get
            {
                if (mapper == null)
                {
                    Initialize();
                }

                return mapper!;
            }
        }
    }
}
