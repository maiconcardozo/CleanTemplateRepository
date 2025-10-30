using Authentication.API.Resource;

namespace Authentication.API.Swagger
{
    internal static class ApplicationClaimRoutes
    {
        public const string GetApplicationClaims = nameof(ResourceRoutesAPI.GetApplicationClaims);
        public const string GetApplicationClaimById = nameof(ResourceRoutesAPI.GetApplicationClaimById);
        public const string GetApplicationClaimsByApplicationId = nameof(ResourceRoutesAPI.GetApplicationClaimsByApplicationId);
        public const string AddApplicationClaim = nameof(ResourceRoutesAPI.AddApplicationClaim);
        public const string UpdateApplicationClaim = nameof(ResourceRoutesAPI.UpdateApplicationClaim);
        public const string DeleteApplicationClaim = nameof(ResourceRoutesAPI.DeleteApplicationClaim);
    }
}
