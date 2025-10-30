using Authentication.Login.Domain.Implementation;
using Authentication.Login.DTO;
using AutoMapper;

namespace Authentication.Login.Mapping
{
    public class AuthenticationLoginProfileMapping : Profile
    {
        public AuthenticationLoginProfileMapping()
        {
            // Account mappings - CreatedBy can now be provided in PayLoadDTO for create operations
            CreateMap<AccountPayLoadDTO, Account>();
            CreateMap<Account, AccountPayLoadDTO>();

            CreateMap<Token, TokenResponseDTO>();
            CreateMap<TokenResponseDTO, Token>();

            CreateMap<Account, AccountResponseDTO>();
            CreateMap<AccountResponseDTO, Account>();

            // Claim mappings - CreatedBy can now be provided in PayLoadDTO for create operations
            CreateMap<ClaimPayLoadDTO, Claim>();
            CreateMap<Claim, ClaimPayLoadDTO>();
            CreateMap<Claim, ClaimResponseDTO>();
            CreateMap<ClaimResponseDTO, Claim>();

            // Action mappings - CreatedBy can now be provided in PayLoadDTO for create operations
            CreateMap<ActionPayLoadDTO, Authentication.Login.Domain.Implementation.Action>();
            CreateMap<Authentication.Login.Domain.Implementation.Action, ActionPayLoadDTO>();
            CreateMap<Authentication.Login.Domain.Implementation.Action, ActionResponseDTO>();
            CreateMap<ActionResponseDTO, Authentication.Login.Domain.Implementation.Action>();

            // ClaimAction mappings - CreatedBy can now be provided in PayLoadDTO for create operations
            CreateMap<ClaimActionPayLoadDTO, ClaimAction>();
            CreateMap<ClaimAction, ClaimActionPayLoadDTO>();
            CreateMap<ClaimAction, ClaimActionResponseDTO>();
            CreateMap<ClaimActionResponseDTO, ClaimAction>();

            // AccountClaimAction mappings - CreatedBy can now be provided in PayLoadDTO for create operations
            CreateMap<AccountClaimActionPayLoadDTO, AccountClaimAction>();
            CreateMap<AccountClaimAction, AccountClaimActionPayLoadDTO>();
            CreateMap<AccountClaimAction, AccountClaimActionResponseDTO>();
            CreateMap<AccountClaimActionResponseDTO, AccountClaimAction>();

            // Application mappings - CreatedBy can now be provided in PayLoadDTO for create operations
            CreateMap<ApplicationPayLoadDTO, Application>();
            CreateMap<Application, ApplicationPayLoadDTO>();
            CreateMap<Application, ApplicationResponseDTO>();
            CreateMap<ApplicationResponseDTO, Application>();

            // ApplicationClaim mappings - CreatedBy can now be provided in PayLoadDTO for create operations
            CreateMap<ApplicationClaimPayLoadDTO, ApplicationClaim>();
            CreateMap<ApplicationClaim, ApplicationClaimPayLoadDTO>();
            CreateMap<ApplicationClaim, ApplicationClaimResponseDTO>();
            CreateMap<ApplicationClaimResponseDTO, ApplicationClaim>();

            // Product mappings - CreatedBy can now be provided in PayLoadDTO for create operations
            CreateMap<ProductPayLoadDTO, Product>();
            CreateMap<Product, ProductPayLoadDTO>();
            CreateMap<Product, ProductResponseDTO>();
            CreateMap<ProductResponseDTO, Product>();

            // ProductVariant mappings - CreatedBy can now be provided in PayLoadDTO for create operations
            CreateMap<ProductVariantPayLoadDTO, ProductVariant>();
            CreateMap<ProductVariant, ProductVariantPayLoadDTO>();
            CreateMap<ProductVariant, ProductVariantResponseDTO>();
            CreateMap<ProductVariantResponseDTO, ProductVariant>();
        }
    }
}
