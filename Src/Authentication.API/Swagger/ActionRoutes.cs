using Authentication.API.Resource;

namespace Authentication.API.Swagger
{
    internal static class ActionRoutes
    {
        public const string GetActions = nameof(ResourceRoutesAPI.GetActions);
        public const string GetActionById = nameof(ResourceRoutesAPI.GetActionById);
        public const string AddAction = nameof(ResourceRoutesAPI.AddAction);
        public const string UpdateAction = nameof(ResourceRoutesAPI.UpdateAction);
        public const string DeleteAction = nameof(ResourceRoutesAPI.DeleteAction);
    }
}
