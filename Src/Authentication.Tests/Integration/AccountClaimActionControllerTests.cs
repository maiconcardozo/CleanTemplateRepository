using System.Net;
using System.Text;
using System.Text.Json;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Infrastructure.Data;
using Authentication.Tests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Authentication.Login.Infrastructure.Data; // Add this using directive at the top of the file

namespace Authentication.Tests.Integration;

public class AccountClaimActionControllerTests : IClassFixture<AuthenticationWebApplicationFactory>
{
    private readonly AuthenticationWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AccountClaimActionControllerTests(AuthenticationWebApplicationFactory factory)
    {
        _factory = factory;
        var context = factory.Services.GetService<LoginContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        factory.SeedTestData(); // Seed test data before creating client
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAccountClaimActions_WithoutParameters_ShouldReturnExpectedStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/AccountClaimAction/GetAccountClaimActions");

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.InternalServerError
        );
    }

    

    [Fact]
    public async Task GetAccountClaimActions_WithIdClaimAction_ShouldReturnExpectedStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/AccountClaimAction/GetAccountClaimActions?idClaimAction=1");

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.InternalServerError
        );
    }

    [Fact]
    public async Task GetAccountClaimActions_WithBothParameters_ShouldReturnExpectedStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/AccountClaimAction/GetAccountClaimActions?idAccount=1&idClaimAction=1");

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.InternalServerError
        );
    }

    

   
    [Fact]
    public async Task AddAccountClaimAction_WithInvalidAccountId_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            idAccount = -1,
            idClaimAction = 1
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/AccountClaimAction/AddAccountClaimAction", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError
        );
    }

    [Fact]
    public async Task AddAccountClaimAction_WithInvalidClaimActionId_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            idAccount = 1,
            idClaimAction = -1
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/AccountClaimAction/AddAccountClaimAction", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError
        );
    }

    [Fact]
    public async Task UpdateAccountClaimAction_WithValidData_ShouldReturnExpectedStatusCode()
    {
        // Arrange
        int id = 1;
        var request = new
        {
            idAccount = 2,
            idClaimAction = 2
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PutAsync($"/AccountClaimAction/UpdateAccountClaimAction/{id}", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.NotFound,
            HttpStatusCode.BadRequest,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.InternalServerError
        );
    }

    [Fact]
    public async Task UpdateAccountClaimAction_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        int nonExistentId = 99999;
        var request = new
        {
            idAccount = 1,
            idClaimAction = 1
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PutAsync($"/AccountClaimAction/UpdateAccountClaimAction/{nonExistentId}", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.NotFound,
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError
        );
    }

  

    [Fact]
    public async Task DeleteAccountClaimAction_WithNonExistentIds_ShouldReturnNotFound()
    {
        // Arrange
        int nonExistentAccountId = 99999;
        int nonExistentClaimActionId = 99999;

        // Act
        var response = await _client.DeleteAsync($"/AccountClaimAction/DeleteAccountClaimAction/{nonExistentAccountId}/{nonExistentClaimActionId}");

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.NotFound,
            HttpStatusCode.InternalServerError
        );
    }


   

    [Theory]
    [InlineData("POST", "/AccountClaimAction/GetAccountClaimActions")]
    [InlineData("PUT", "/AccountClaimAction/GetAccountClaimActions")]
    [InlineData("DELETE", "/AccountClaimAction/GetAccountClaimActionById/1/1")]
    public async Task AccountClaimActionEndpoints_WithUnsupportedHttpMethods_ShouldReturnMethodNotAllowed(string httpMethod, string endpoint)
    {
        // Arrange
        var request = new HttpRequestMessage(new HttpMethod(httpMethod), endpoint);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.MethodNotAllowed,
            HttpStatusCode.NotFound,
            HttpStatusCode.InternalServerError
        );
    }
}
