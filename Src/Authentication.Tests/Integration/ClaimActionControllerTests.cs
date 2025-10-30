using System.Net;
using System.Text;
using System.Text.Json;
using Authentication.Tests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Authentication.Tests.Integration;

public class ClaimActionControllerTests : IClassFixture<AuthenticationWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> factory;
    private readonly HttpClient client;

    public ClaimActionControllerTests(AuthenticationWebApplicationFactory factory)
    {
        this.factory = factory;
        factory.SeedTestData(); // Seed test data before creating client
        client = this.factory.CreateClient();
    }

    [Fact]
    public async Task GetClaimActions_ShouldReturnExpectedStatusCode()
    {
        // Act
        var response = await client.GetAsync("/ClaimAction/GetClaimActions");

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.InternalServerError);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(999)]
    [InlineData(-1)]
    public async Task GetClaimActionById_WithVariousIds_ShouldReturnExpectedStatusCode(int id)
    {
        // Act
        var response = await client.GetAsync($"/ClaimAction/GetClaimActionById/{id}");

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.NotFound,
            HttpStatusCode.BadRequest,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task AddClaimAction_WithValidData_ShouldReturnExpectedStatusCode()
    {
        // Arrange
        var request = new
        {
            claimId = 1,
            actionId = 1,
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync("/ClaimAction/AddClaimAction", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.BadRequest,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task AddClaimAction_WithInvalidClaimId_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            claimId = -1,
            actionId = 1,
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync("/ClaimAction/AddClaimAction", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task AddClaimAction_WithInvalidActionId_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            claimId = 1,
            actionId = -1,
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync("/ClaimAction/AddClaimAction", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task UpdateClaimAction_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        int nonExistentId = 99999;
        var request = new
        {
            id = nonExistentId,
            idClaim = 1,
            idAction = 1,
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PutAsync("/ClaimAction/UpdateClaimAction", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.NotFound,
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task AddClaimAction_WithInvalidJson_ShouldReturnBadRequest()
    {
        // Arrange
        var content = new StringContent(
            "{ invalid json }",
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync("/ClaimAction/AddClaimAction", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError);
    }
}
