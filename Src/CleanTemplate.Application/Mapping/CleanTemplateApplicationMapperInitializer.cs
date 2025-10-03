using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace CleanTemplate.Application.Mapping
{
    public static class CleanTemplateApplicationMapperInitializer
    {
        private static IMapper? _mapper;
        private static MapperConfiguration? _config;

        public static void Initialize()
        {
            _config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CleanTemplateApplicationMapping>();
            });
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
