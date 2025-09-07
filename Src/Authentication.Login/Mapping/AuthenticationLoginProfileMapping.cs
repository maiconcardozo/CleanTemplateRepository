using Authentication.Login.Domain.Implementation;
using Authentication.Login.DTO;
using AutoMapper;

namespace Authentication.Login.Mapping
{
    public class AuthenticationLoginProfileMapping : Profile
    {
        public AuthenticationLoginProfileMapping()
        {
            CreateMap<AccountPayLoadDTO, Account>();
            CreateMap<Account, AccountPayLoadDTO>();

            CreateMap<Token, TokenResponseDTO>();
            CreateMap<TokenResponseDTO, Token>();

            CreateMap<Account, AccountResponseDTO>();
            CreateMap<AccountResponseDTO, Account>();

            // Claim mappings
            CreateMap<ClaimPayLoadDTO, Claim>();
            CreateMap<Claim, ClaimPayLoadDTO>();
            CreateMap<Claim, ClaimResponseDTO>();
            CreateMap<ClaimResponseDTO, Claim>();

            // Action mappings
            CreateMap<ActionPayLoadDTO, Authentication.Login.Domain.Implementation.Action>();
            CreateMap<Authentication.Login.Domain.Implementation.Action, ActionPayLoadDTO>();
            CreateMap<Authentication.Login.Domain.Implementation.Action, ActionResponseDTO>();
            CreateMap<ActionResponseDTO, Authentication.Login.Domain.Implementation.Action>();

            // ClaimAction mappings
            CreateMap<ClaimActionPayLoadDTO, ClaimAction>();
            CreateMap<ClaimAction, ClaimActionPayLoadDTO>();
            CreateMap<ClaimAction, ClaimActionResponseDTO>();
            CreateMap<ClaimActionResponseDTO, ClaimAction>();

            // AccountClaimAction mappings
            CreateMap<AccountClaimActionPayLoadDTO, AccountClaimAction>();
            CreateMap<AccountClaimAction, AccountClaimActionPayLoadDTO>();
            CreateMap<AccountClaimAction, AccountClaimActionResponseDTO>();
            CreateMap<AccountClaimActionResponseDTO, AccountClaimAction>();
        }
    }
}