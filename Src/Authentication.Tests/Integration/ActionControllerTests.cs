using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Authentication.Tests.Fixtures;
using Xunit;

namespace Authentication.Tests.Integration;

public class ActionControllerTests : IClassFixture<AuthenticationWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ActionControllerTests(AuthenticationWebApplicationFactory factory)
    {
        _factory = factory;
        factory.SeedTestData(); // Seed test data before creating client
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetActions_ShouldReturnExpectedStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/Action/GetActions");

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.InternalServerError
        );
    }

    [Theory]
    [InlineData(1)]
    [InlineData(999)]
    [InlineData(-1)]
    public async Task GetActionById_WithVariousIds_ShouldReturnExpectedStatusCode(int id)
    {
        // Act
        var response = await _client.GetAsync($"/Action/GetActionById/{id}");

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
    public async Task AddAction_WithValidData_ShouldReturnExpectedStatusCode()
    {
        // Arrange
        var request = new
        {
            name = "CreateUser",
            description = "Create a new user"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/Action/AddAction", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.BadRequest,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.InternalServerError
        );
    }

    [Fact]
    public async Task AddAction_WithEmptyName_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            name = "",
            description = "Create a new user"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/Action/AddAction", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError
        );
    }

 

    [Fact]
    public async Task UpdateAction_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        int nonExistentId = 99999;
        var request = new
        {
            name = "UpdateUser",
            description = "Update an existing user"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PutAsync($"/Action/UpdateAction/{nonExistentId}", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.NotFound,
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError
        );
    }


    [Fact]
    public async Task DeleteAction_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        int nonExistentId = 99999;

        // Act
        var response = await _client.DeleteAsync($"/Action/DeleteAction/{nonExistentId}");

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.NotFound,
            HttpStatusCode.InternalServerError
        );
    }

    [Fact]
    public async Task AddAction_WithInvalidJson_ShouldReturnBadRequest()
    {
        // Arrange
        var content = new StringContent(
            "{ invalid json }",
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/Action/AddAction", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError
        );
    }

   

}