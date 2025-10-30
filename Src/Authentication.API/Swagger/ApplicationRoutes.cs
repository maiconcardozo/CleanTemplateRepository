using Authentication.API.Resource;

namespace Authentication.API.Swagger
{
    internal static class ApplicationRoutes
    {
        public const string GetApplications = nameof(ResourceRoutesAPI.GetApplications);
        public const string GetApplicationById = nameof(ResourceRoutesAPI.GetApplicationById);
        public const string AddApplication = nameof(ResourceRoutesAPI.AddApplication);
        public const string UpdateApplication = nameof(ResourceRoutesAPI.UpdateApplication);
        public const string DeleteApplication = nameof(ResourceRoutesAPI.DeleteApplication);
    }
}
