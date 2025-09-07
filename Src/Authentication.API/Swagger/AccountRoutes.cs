using Authentication.API.Resource;

namespace Authentication.API.Swagger
{
    public static class AccountRoutes
    {
        public const string GetAccounts = nameof(ResourceRoutesAPI.GetAccounts);
        public const string GetAccountById = nameof(ResourceRoutesAPI.GetAccountById);
        public const string AddAccount = nameof(ResourceRoutesAPI.AddAccount);
        public const string UpdateAccount = nameof(ResourceRoutesAPI.UpdateAccount);
        public const string DeleteAccount = nameof(ResourceRoutesAPI.DeleteAccount);
    }
}