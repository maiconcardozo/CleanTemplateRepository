using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Authentication.Tests.Fixtures;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<DbContext>(options =>
            options.UseInMemoryDatabase("TestDatabase"));
        services.AddLogging();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            // Mock endpoints that simulate the real API behavior
            endpoints.MapPost("/Authentication/GenerateToken", async context =>
            {
                // Simulate authentication endpoint behavior
                var response = new { message = "Mock response" };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapPost("/Account/AddAccount", async context =>
            {
                // Simulate account creation endpoint behavior
                var response = new { message = "Mock response" };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapGet("/Claim/GetClaims", async context =>
            {
                var response = new[] { new { id = 1, type = "Permission", value = "test" } };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapGet("/Claim/GetClaimById/{id}", async context =>
            {
                var id = context.Request.RouteValues["id"];
                var response = new { id = id, type = "Permission", value = "test" };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapPost("/Claim/AddClaim", async context =>
            {
                var response = new { id = 1, message = "Created" };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapPut("/Claim/UpdateClaim/{id}", async context =>
            {
                var id = context.Request.RouteValues["id"];
                var response = new { id = id, message = "Updated" };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapDelete("/Claim/DeleteClaim/{id}", async context =>
            {
                var id = context.Request.RouteValues["id"];
                var response = new { id = id, message = "Deleted" };
                await context.Response.WriteAsJsonAsync(response);
            });

            // Similar patterns for other controllers
            endpoints.MapGet("/Action/GetActions", async context =>
            {
                var response = new[] { new { id = 1, name = "TestAction" } };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapGet("/Action/GetActionById/{id}", async context =>
            {
                var id = context.Request.RouteValues["id"];
                var response = new { id = id, name = "TestAction" };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapPost("/Action/AddAction", async context =>
            {
                var response = new { id = 1, message = "Created" };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapPut("/Action/UpdateAction/{id}", async context =>
            {
                try
                {
                    // Try to read and parse the request body as JSON
                    using var reader = new StreamReader(context.Request.Body);
                    var body = await reader.ReadToEndAsync();
                    
                    if (string.IsNullOrEmpty(body))
                    {
                        context.Response.StatusCode = 400;
                        var errorResponse = new { error = "Request body is required" };
                        await context.Response.WriteAsJsonAsync(errorResponse);
                        return;
                    }

                    // Try to parse JSON
                    try
                    {
                        JsonDocument.Parse(body);
                    }
                    catch (JsonException)
                    {
                        // Invalid JSON should return 400 Bad Request
                        context.Response.StatusCode = 400;
                        var errorResponse = new { error = "Invalid JSON format" };
                        await context.Response.WriteAsJsonAsync(errorResponse);
                        return;
                    }

                    var id = context.Request.RouteValues["id"];
                    var response = new { id = id, message = "Updated" };
                    await context.Response.WriteAsJsonAsync(response);
                }
                catch (Exception)
                {
                    // Any other exception should return 500 Internal Server Error
                    context.Response.StatusCode = 500;
                    var errorResponse = new { error = "Internal server error" };
                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
            });

            endpoints.MapDelete("/Action/DeleteAction/{id}", async context =>
            {
                var id = context.Request.RouteValues["id"];
                var response = new { id = id, message = "Deleted" };
                await context.Response.WriteAsJsonAsync(response);
            });

            // ClaimAction endpoints
            endpoints.MapGet("/ClaimAction/GetClaimActions", async context =>
            {
                var response = new[] { new { id = 1, claimId = 1, actionId = 1 } };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapGet("/ClaimAction/GetClaimActionById/{id}", async context =>
            {
                var id = context.Request.RouteValues["id"];
                var response = new { id = id, claimId = 1, actionId = 1 };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapPost("/ClaimAction/AddClaimAction", async context =>
            {
                var response = new { id = 1, message = "Created" };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapPut("/ClaimAction/UpdateClaimAction/{id}", async context =>
            {
                try
                {
                    // Try to read and parse the request body as JSON
                    using var reader = new StreamReader(context.Request.Body);
                    var body = await reader.ReadToEndAsync();
                    
                    if (string.IsNullOrEmpty(body))
                    {
                        context.Response.StatusCode = 400;
                        var errorResponse = new { error = "Request body is required" };
                        await context.Response.WriteAsJsonAsync(errorResponse);
                        return;
                    }

                    // Try to parse JSON
                    try
                    {
                        JsonDocument.Parse(body);
                    }
                    catch (JsonException)
                    {
                        // Invalid JSON should return 400 Bad Request
                        context.Response.StatusCode = 400;
                        var errorResponse = new { error = "Invalid JSON format" };
                        await context.Response.WriteAsJsonAsync(errorResponse);
                        return;
                    }

                    var id = context.Request.RouteValues["id"];
                    var response = new { id = id, message = "Updated" };
                    await context.Response.WriteAsJsonAsync(response);
                }
                catch (Exception)
                {
                    // Any other exception should return 500 Internal Server Error
                    context.Response.StatusCode = 500;
                    var errorResponse = new { error = "Internal server error" };
                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
            });

            endpoints.MapDelete("/ClaimAction/DeleteClaimAction/{id}", async context =>
            {
                var id = context.Request.RouteValues["id"];
                var response = new { id = id, message = "Deleted" };
                await context.Response.WriteAsJsonAsync(response);
            });

            // AccountClaimAction endpoints
            endpoints.MapGet("/AccountClaimAction/GetAccountClaimActions", async context =>
            {
                var response = new[] { new { id = 1, idAccount = 1, idClaimAction = 1 } };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapGet("/AccountClaimAction/GetAccountClaimActionById/{idAccount}/{idClaimAction}", async context =>
            {
                var idAccount = context.Request.RouteValues["idAccount"];
                var idClaimAction = context.Request.RouteValues["idClaimAction"];
                var response = new { id = 1, idAccount = idAccount, idClaimAction = idClaimAction };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapPost("/AccountClaimAction/AddAccountClaimAction", async context =>
            {
                var response = new { id = 1, message = "Created" };
                await context.Response.WriteAsJsonAsync(response);
            });

            endpoints.MapPut("/AccountClaimAction/UpdateAccountClaimAction/{id}", async context =>
            {
                try
                {
                    // Try to read and parse the request body as JSON
                    using var reader = new StreamReader(context.Request.Body);
                    var body = await reader.ReadToEndAsync();
                    
                    if (string.IsNullOrEmpty(body))
                    {
                        context.Response.StatusCode = 400;
                        var errorResponse = new { error = "Request body is required" };
                        await context.Response.WriteAsJsonAsync(errorResponse);
                        return;
                    }

                    // Try to parse JSON
                    try
                    {
                        JsonDocument.Parse(body);
                    }
                    catch (JsonException)
                    {
                        // Invalid JSON should return 400 Bad Request
                        context.Response.StatusCode = 400;
                        var errorResponse = new { error = "Invalid JSON format" };
                        await context.Response.WriteAsJsonAsync(errorResponse);
                        return;
                    }

                    var id = context.Request.RouteValues["id"];
                    var response = new { id = id, message = "Updated" };
                    await context.Response.WriteAsJsonAsync(response);
                }
                catch (Exception)
                {
                    // Any other exception should return 500 Internal Server Error
                    context.Response.StatusCode = 500;
                    var errorResponse = new { error = "Internal server error" };
                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
            });

            endpoints.MapDelete("/AccountClaimAction/DeleteAccountClaimAction/{idAccount}/{idClaimAction}", async context =>
            {
                var idAccount = context.Request.RouteValues["idAccount"];
                var idClaimAction = context.Request.RouteValues["idClaimAction"];
                var response = new { idAccount = idAccount, idClaimAction = idClaimAction, message = "Deleted" };
                await context.Response.WriteAsJsonAsync(response);
            });
        });
    }
}