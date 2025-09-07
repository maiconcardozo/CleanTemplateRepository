using Authentication.API.Resource;

namespace Authentication.API.Swagger
{
    public static class ClaimActionRoutes
    {
        public const string GetClaimActions = nameof(ResourceRoutesAPI.GetClaimActions);
        public const string GetClaimActionById = nameof(ResourceRoutesAPI.GetClaimActionById);
        public const string AddClaimAction = nameof(ResourceRoutesAPI.AddClaimAction);
        public const string UpdateClaimAction = nameof(ResourceRoutesAPI.UpdateClaimAction);
        public const string DeleteClaimAction = nameof(ResourceRoutesAPI.DeleteClaimAction);
    }
}