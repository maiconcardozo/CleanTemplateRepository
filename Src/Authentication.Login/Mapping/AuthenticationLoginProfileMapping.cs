using Authentication.Login.Domain.Implementation;
using Authentication.Login.DTO;
using AutoMapper;

namespace Authentication.Login.Mapping
{
    public class AuthenticationLoginProfileMapping : Profile
    {
        public AuthenticationLoginProfileMapping()
        {
            // Token mappings (keep for JWT functionality)
            CreateMap<Token, TokenResponseDTO>();
            CreateMap<TokenResponseDTO, Token>();

            // CleanEntity mappings
            CreateMap<CleanEntityPayLoadDTO, CleanEntity>();
            CreateMap<CleanEntity, CleanEntityPayLoadDTO>();
            CreateMap<CleanEntity, CleanEntityResponseDTO>();
            CreateMap<CleanEntityResponseDTO, CleanEntity>();
        }
    }
}