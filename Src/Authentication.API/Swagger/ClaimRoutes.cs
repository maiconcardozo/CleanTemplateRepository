using Authentication.API.Resource;

namespace Authentication.API.Swagger
{
    internal static class ClaimRoutes
    {
        public const string GetClaims = nameof(ResourceRoutesAPI.GetClaims);
        public const string GetClaimById = nameof(ResourceRoutesAPI.GetClaimById);
        public const string AddClaim = nameof(ResourceRoutesAPI.AddClaim);
        public const string UpdateClaim = nameof(ResourceRoutesAPI.UpdateClaim);
        public const string DeleteClaim = nameof(ResourceRoutesAPI.DeleteClaim);
    }
}
