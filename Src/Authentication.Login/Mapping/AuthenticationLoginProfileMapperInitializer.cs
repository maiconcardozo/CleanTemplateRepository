using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace Authentication.Login.Mapping
{
    public static class AuthenticationLoginProfileMapperInitializer
    {
        private static IMapper? _mapper;
        private static MapperConfiguration? _config;

        public static void Initialize()
        {
            var configExpr = new MapperConfigurationExpression();
            configExpr.AddProfile<AuthenticationLoginProfileMapping>();

            _config = new MapperConfiguration(configExpr, NullLoggerFactory.Instance);
            _mapper = _config.CreateMapper();
        }

        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    Initialize();
                }
                return _mapper!;
            }
        }
    }
}
