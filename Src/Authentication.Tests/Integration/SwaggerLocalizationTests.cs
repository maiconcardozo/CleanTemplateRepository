using System.Net;
using System.Text.Json;
using Authentication.API.Resource;
using Authentication.Tests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Authentication.Tests.Integration;

public class SwaggerLocalizationTests : IClassFixture<AuthenticationWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> factory;
    private readonly HttpClient client;

    public SwaggerLocalizationTests(AuthenticationWebApplicationFactory factory)
    {
        this.factory = factory;
        client = this.factory.CreateClient();
    }

    [Fact]
    public async Task SwaggerJson_AccountEndpoints_ShouldHaveLocalizedDescriptions()
    {
        // Arrange
        // Add basic auth header for Swagger access
        var credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin:senha123"));
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

        // Act
        var response = await client.GetAsync("/swagger/AccessControl/swagger.json");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonContent = await response.Content.ReadAsStringAsync();
        var swaggerDoc = JsonDocument.Parse(jsonContent);

        // Verify GetAccounts endpoint has localized summary and description
        var getAccountsPath = swaggerDoc.RootElement
            .GetProperty("paths")
            .GetProperty("/Account/GetAccounts")
            .GetProperty("get");

        var summary = getAccountsPath.GetProperty("summary").GetString();
        var description = getAccountsPath.GetProperty("description").GetString();

        summary.Should().Be(ResourceAPI.GetAccounts);
        description.Should().Be(ResourceAPI.DocumentationGetAccounts);

        // Verify that responses also have localized descriptions
        var successResponse = getAccountsPath
            .GetProperty("responses")
            .GetProperty("200")
            .GetProperty("description").GetString();

        successResponse.Should().Be(ResourceAPI.AccountsRetrievedSuccessfully);
    }

    [Fact]
    public async Task SwaggerJson_GetAccountByIdEndpoint_ShouldHaveLocalizedDescriptions()
    {
        // Arrange
        var credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin:senha123"));
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

        // Act
        var response = await client.GetAsync("/swagger/AccessControl/swagger.json");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonContent = await response.Content.ReadAsStringAsync();
        var swaggerDoc = JsonDocument.Parse(jsonContent);

        // Verify GetAccountById endpoint has localized summary and description
        var getAccountByIdPath = swaggerDoc.RootElement
            .GetProperty("paths")
            .GetProperty("/Account/GetAccountById")
            .GetProperty("get");

        var summary = getAccountByIdPath.GetProperty("summary").GetString();
        var description = getAccountByIdPath.GetProperty("description").GetString();

        summary.Should().Be(ResourceAPI.GetAccountById);
        description.Should().Be(ResourceAPI.DocumentationGetAccountById);
    }

    [Fact]
    public async Task SwaggerJson_ShouldNotContainResourceKeyPlaceholders()
    {
        // Arrange
        var credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin:senha123"));
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

        // Act
        var response = await client.GetAsync("/swagger/AccessControl/swagger.json");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonContent = await response.Content.ReadAsStringAsync();

        // The JSON should not contain the original curly brace placeholders like {ResourceAPI.DocumentationGetAccounts}
        jsonContent.Should().NotContain("{ResourceAPI.DocumentationGetAccounts}");
        jsonContent.Should().NotContain("{ResourceAPI.DocumentationGetAccountById}");
        jsonContent.Should().NotContain("{ResourceAPI.DocumentationUpdateAccount}");
        jsonContent.Should().NotContain("{ResourceAPI.DocumentationDeleteAccount}");

        // It should contain the actual translated text
        jsonContent.Should().Contain(ResourceAPI.DocumentationGetAccounts);
        jsonContent.Should().Contain(ResourceAPI.DocumentationGetAccountById);
    }
}
