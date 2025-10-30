using Authentication.API.Resource;

namespace Authentication.API.Swagger
{
    internal static class ProductRoutes
    {
        public const string GetProducts = nameof(ResourceRoutesAPI.GetProducts);
        public const string GetProductById = nameof(ResourceRoutesAPI.GetProductById);
        public const string AddProduct = nameof(ResourceRoutesAPI.AddProduct);
        public const string UpdateProduct = nameof(ResourceRoutesAPI.UpdateProduct);
        public const string DeleteProduct = nameof(ResourceRoutesAPI.DeleteProduct);
    }
}
