using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using System.Globalization;
using CleanTemplate.API.Resource;
using Microsoft.AspNetCore.Localization;

namespace CleanTemplate.API.Swagger
{
    public class LocalizedSwaggerDocumentFilter : IDocumentFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalizedSwaggerDocumentFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var culture = GetCurrentCulture();

            if (swaggerDoc.Info != null)
            {
                if (!string.IsNullOrEmpty(swaggerDoc.Info.Title) && swaggerDoc.Info.Title.StartsWith("ResourceAPI."))
                {
                    var key = swaggerDoc.Info.Title.Substring("ResourceAPI.".Length);
                    var text = ResourceAPI.ResourceManager.GetString(key, culture);
                    if (!string.IsNullOrEmpty(text)) swaggerDoc.Info.Title = text;
                }
                else if (!string.IsNullOrEmpty(swaggerDoc.Info.Title) && swaggerDoc.Info.Title.StartsWith("ResourceStartup."))
                {
                    var key = swaggerDoc.Info.Title.Substring("ResourceStartup.".Length);
                    var text = ResourceStartup.ResourceManager.GetString(key, culture);
                    if (!string.IsNullOrEmpty(text)) swaggerDoc.Info.Title = text;
                }

                if (!string.IsNullOrEmpty(swaggerDoc.Info.Description) && swaggerDoc.Info.Description.StartsWith("ResourceAPI."))
                {
                    var key = swaggerDoc.Info.Description.Substring("ResourceAPI.".Length);
                    var text = ResourceAPI.ResourceManager.GetString(key, culture);
                    if (!string.IsNullOrEmpty(text)) swaggerDoc.Info.Description = text;
                }
                else if (!string.IsNullOrEmpty(swaggerDoc.Info.Description) && swaggerDoc.Info.Description.StartsWith("ResourceStartup."))
                {
                    var key = swaggerDoc.Info.Description.Substring("ResourceStartup.".Length);
                    var text = ResourceStartup.ResourceManager.GetString(key, culture);
                    if (!string.IsNullOrEmpty(text)) swaggerDoc.Info.Description = text;
                }
            }

            if (swaggerDoc.Tags != null)
            {
                foreach (var tag in swaggerDoc.Tags)
                {
                    if (!string.IsNullOrEmpty(tag.Description) && tag.Description.StartsWith("ResourceAPI."))
                    {
                        var key = tag.Description.Substring("ResourceAPI.".Length);
                        var text = ResourceAPI.ResourceManager.GetString(key, culture);
                        if (!string.IsNullOrEmpty(text)) tag.Description = text;
                    }
                    else if (!string.IsNullOrEmpty(tag.Description) && tag.Description.StartsWith("ResourceStartup."))
                    {
                        var key = tag.Description.Substring("ResourceStartup.".Length);
                        var text = ResourceStartup.ResourceManager.GetString(key, culture);
                        if (!string.IsNullOrEmpty(text)) tag.Description = text;
                    }
                }
            }
        }

        private CultureInfo GetCurrentCulture()
        {
            // Try to get culture from current HTTP request context first
            if (_httpContextAccessor.HttpContext != null)
            {
                var requestCultureFeature = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
                if (requestCultureFeature?.RequestCulture?.Culture != null)
                {
                    return requestCultureFeature.RequestCulture.Culture;
                }
            }

            // Fall back to CurrentUICulture if no HTTP context or request culture available
            return CultureInfo.CurrentUICulture;
        }
    }
}