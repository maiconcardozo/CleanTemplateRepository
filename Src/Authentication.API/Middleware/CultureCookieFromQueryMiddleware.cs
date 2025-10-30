using Microsoft.AspNetCore.Localization;

namespace Authentication.API.Middleware
{
    /// <summary>
    /// Middleware that persists culture query string parameters as localization cookies.
    /// This ensures that when Swagger UI makes subsequent requests for swagger.json,
    /// the correct culture is maintained via the cookie rather than falling back to Accept-Language.
    /// </summary>
    internal class CultureCookieFromQueryMiddleware
    {
        private readonly RequestDelegate next;

        public CultureCookieFromQueryMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if culture or ui-culture is present in query string
            var cultureFromQuery = context.Request.Query["culture"].FirstOrDefault();
            var uiCultureFromQuery = context.Request.Query["ui-culture"].FirstOrDefault();

            if (!string.IsNullOrEmpty(cultureFromQuery) || !string.IsNullOrEmpty(uiCultureFromQuery))
            {
                // Use the provided culture, fallback to the same value if ui-culture is not specified
                var culture = cultureFromQuery ?? uiCultureFromQuery;
                var uiCulture = uiCultureFromQuery ?? cultureFromQuery;

                // Create RequestCulture and generate cookie value
                var requestCulture = new RequestCulture(culture!, uiCulture!);
                var cookieValue = CookieRequestCultureProvider.MakeCookieValue(requestCulture);

                // Set the localization cookie
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1), // 1 year expiration
                    IsEssential = true, // Essential for functionality
                    Path = "/", // Available for all paths
                    SameSite = SameSiteMode.Lax, // Standard security setting
                };

                context.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    cookieValue,
                    cookieOptions);
            }

            await next(context);
        }
    }
}
