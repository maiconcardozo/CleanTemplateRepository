namespace CleanTemplate.Application.Constants
{
    public static class ApplicationConstants
    {
        public const string DefaultCreatedByUser = "ADMINISTRATOR";
        public const string DefaultConnectionStringName = "DefaultConnection";
        
        public static class ClaimTypes
        {
            public const string Permission = "permission";
        }
        
        public static class Environment
        {
            public const string Production = "Production";
            public const string Development = "Development";
        }
        
        public static class Cors
        {
            public const string AllowAllPolicy = "AllowAll";
        }
        
        public static class Api
        {
            public const string Title = "CleanTemplate API";
            public const string Version = "v1";
            public const string SwaggerEndpoint = "/swagger/v1/swagger.json";
            public const string SwaggerDisplayName = "CleanTemplate API V1";
            public const string CustomStylePath = "/Style/custom-swagger.css";
            public const string EmptyRoutePrefix = "";

            // Swagger definitions
            public static class SwaggerDefinitions
            {
                public const string CleanTemplate = "CleanTemplate";
                public const string AccessControl = "AccessControl";
                
                public const string CleanTemplateEndpoint = "/swagger/CleanTemplate/swagger.json";
                public const string AccessControlEndpoint = "/swagger/AccessControl/swagger.json";
                
                public const string CleanTemplateDisplayName = "CleanTemplate API";
                public const string AccessControlDisplayName = "Access Control API";
            }
        }
    }
}