using System.Net;
using System.Text;

namespace Authentication.API.Middleware
{
    internal class SwaggerAuthMiddleware
    {
        private readonly RequestDelegate next;
        private const string SwaggerPath = "/swagger";
        private const string IndexPath = "/index.html";
        private const string RootPath = "/";
        private const string Username = "admin";
        private const string Password = "senha123";

        public SwaggerAuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(SwaggerPath, StringComparison.OrdinalIgnoreCase) ||
                context.Request.Path.StartsWithSegments(IndexPath, StringComparison.OrdinalIgnoreCase) ||
                context.Request.Path == RootPath)
            {
                string? authHeader = context.Request.Headers["Authorization"];

                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                    var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                    var parts = decodedUsernamePassword.Split(':', 2);
                    if (parts.Length == 2 && parts[0] == Username && parts[1] == Password)
                    {
                        await next(context);
                        return;
                    }
                }

                context.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await next(context);
        }
    }
}
