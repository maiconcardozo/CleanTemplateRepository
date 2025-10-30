using Authentication.API.Resource;

namespace Authentication.API.Swagger
{
    internal static class ProductVariantRoutes
    {
        public const string GetProductVariants = nameof(ResourceRoutesAPI.GetProductVariants);
        public const string GetProductVariantById = nameof(ResourceRoutesAPI.GetProductVariantById);
        public const string GetProductVariantsByProductId = nameof(ResourceRoutesAPI.GetProductVariantsByProductId);
        public const string AddProductVariant = nameof(ResourceRoutesAPI.AddProductVariant);
        public const string UpdateProductVariant = nameof(ResourceRoutesAPI.UpdateProductVariant);
        public const string DeleteProductVariant = nameof(ResourceRoutesAPI.DeleteProductVariant);
    }
}
