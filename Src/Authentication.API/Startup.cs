using System.Reflection;
using Authentication.API.Data;
using Authentication.API.Resource;
using Authentication.API.Services;
using Authentication.API.Swagger;
using Authentication.Login.Constants;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Domain.Interface;
using Authentication.Login.DTO;
using Authentication.Login.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;

namespace Authentication.API
{
    /// <summary>
    /// Classe responsável pela configuração inicial da aplicação ASP.NET Core.
    /// Organiza a configuração de serviços e do pipeline de middlewares de forma estruturada.
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // ==============================
            // CONFIGURAÇÃO DO AMBIENTE
            // ==============================
            // Detect if running under test (xUnit, NUnit, MSTest, etc.)
            var isTest = AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.FullName.StartsWith("xunit", StringComparison.OrdinalIgnoreCase) ||
                          a.FullName.StartsWith("nunit", StringComparison.OrdinalIgnoreCase) ||
                          a.FullName.StartsWith("Microsoft.VisualStudio.TestPlatform", StringComparison.OrdinalIgnoreCase));

            var environment = isTest
                ? ApplicationConstants.Environment.Development
                : Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? ApplicationConstants.Environment.Production;

            var appsettings = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);


            // ==============================
            // CACHE
            // ==============================
            services.AddMemoryCache();
            services.AddSingleton<IConfigurationCache, ConfigurationCache>();

            // ==============================
            // HTTP CONTEXT ACCESSOR (for Swagger localization)
            // ==============================
            services.AddHttpContextAccessor();

            // ==============================
            // LOCALIZAÇÃO
            // ==============================
            services.AddLocalization(options => options.ResourcesPath = "Resource");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en", "pt-BR" };
                options.SetDefaultCulture(supportedCultures[0])
                       .AddSupportedCultures(supportedCultures)
                       .AddSupportedUICultures(supportedCultures);

                // Adicione os providers manualmente!
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider()
                };
            });

            // ==============================
            // AUTENTICAÇÃO & DOMÍNIO
            // ==============================
            services.AddAuthenticationLoginServices(Authentication.API.Helper.Utils.GetConnectionString(appsettings));

            // ==============================
            // CONTROLLERS & VALIDAÇÃO
            // ==============================
            services.AddControllers();
            // TODO: Add FluentValidation for CleanEntity when needed
            // services.AddTransient<FluentValidation.IValidator<CleanEntityPayLoadDTO>, CleanEntityPayloadValidator>();

            // ==============================
            // SWAGGER
            // ==============================
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                options.EnableAnnotations();
                options.ExampleFilters();

                // Filtros para internacionalização
                options.OperationFilter<LocalizedSwaggerOperationFilter>();
                options.DocumentFilter<LocalizedSwaggerDocumentFilter>();

                // Use CHAVES do resource nas definições de SwaggerDoc!
                options.SwaggerDoc(ApplicationConstants.Api.SwaggerDefinitions.Authentication, new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ResourceStartup.AuthenticationApiDisplayName",
                    Version = ApplicationConstants.Api.Version
                });

                options.SwaggerDoc(ApplicationConstants.Api.SwaggerDefinitions.AccessControl, new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ResourceStartup.AccessControlApiDisplayName",
                    Version = ApplicationConstants.Api.Version
                });

                options.DocInclusionPredicate((docName, apiDescription) =>
                {
                    var controllerName = apiDescription.ActionDescriptor.RouteValues["controller"];
                    return docName switch
                    {
                        ApplicationConstants.Api.SwaggerDefinitions.Authentication =>
                            controllerName?.Equals(ResourceStartup.AuthenticationController, StringComparison.OrdinalIgnoreCase) == true,
                        ApplicationConstants.Api.SwaggerDefinitions.AccessControl =>
                            controllerName?.Equals(ResourceStartup.AccountController, StringComparison.OrdinalIgnoreCase) == true ||
                            controllerName?.Equals(ResourceStartup.AccountClaimActionController, StringComparison.OrdinalIgnoreCase) == true ||
                            controllerName?.Equals(ResourceStartup.ActionController, StringComparison.OrdinalIgnoreCase) == true ||
                            controllerName?.Equals(ResourceStartup.ClaimActionController, StringComparison.OrdinalIgnoreCase) == true ||
                            controllerName?.Equals(ResourceStartup.ClaimController, StringComparison.OrdinalIgnoreCase) == true,
                        _ => false
                    };
                });
            });

            services.AddSwaggerExamplesFromAssemblyOf<SucessDetailsExample>();
            services.AddSwaggerExamplesFromAssemblyOf<ProblemDetailsBadRequestExample>();

            // ==============================
            // JWT
            // ==============================
            services.Configure<JwtSettings>(Authentication.API.Helper.Utils.GetJwtSettings(appsettings));
            services.AddSingleton<IJwtSettings>(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

            // ==============================
            // CORS
            // ==============================
            services.AddCors(options =>
            {
                options.AddPolicy(ApplicationConstants.Cors.AllowAllPolicy, policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ==============================
            // DEV/DEBUG
            // ==============================
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ==============================
            // SEGURANÇA & STATIC FILES
            // ==============================
            app.UseMiddleware<Authentication.API.Middleware.SwaggerAuthMiddleware>();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            // ==============================
            // CORS
            // ==============================
            app.UseCors(ApplicationConstants.Cors.AllowAllPolicy);

            // ==============================
            // CULTURE COOKIE FROM QUERY (deve vir ANTES da localização!)
            // ==============================
            app.UseMiddleware<Authentication.API.Middleware.CultureCookieFromQueryMiddleware>();

            // ==============================
            // LOCALIZAÇÃO (deve vir ANTES do Swagger!)
            // ==============================
            app.UseRequestLocalization();

            // ==============================
            // SWAGGER
            // ==============================
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    ApplicationConstants.Api.SwaggerDefinitions.AuthenticationEndpoint,
                    ResourceStartup.AuthenticationApiDisplayName
                );
                options.SwaggerEndpoint(
                    ApplicationConstants.Api.SwaggerDefinitions.AccessControlEndpoint,
                    ResourceStartup.AccessControlApiDisplayName
                );
                options.RoutePrefix = ApplicationConstants.Api.EmptyRoutePrefix;
                options.InjectStylesheet(ApplicationConstants.Api.CustomStylePath);
            });

            // ==============================
            // EXCEÇÕES
            // ==============================
            app.UseMiddleware<Authentication.API.Middleware.ExceptionHandlingMiddleware>();

            // ==============================
            // ROTEAMENTO & AUTORIZAÇÃO
            // ==============================
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}