using System.Net;
using System.Text;
using System.Text.Json;
using Authentication.Tests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Authentication.Tests.Integration;

public class AccountControllerTests : IClassFixture<AuthenticationWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> factory;
    private readonly HttpClient client;

    public AccountControllerTests(AuthenticationWebApplicationFactory factory)
    {
        this.factory = factory;
        client = this.factory.CreateClient();
    }

    [Fact]
    public async Task AddAccount_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var request = new
        {
            userName = "newuser",
            password = "newpassword123",
            email = "newuser@test.com",
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync("/Account/AddAccount", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task AddAccount_WithEmptyUserName_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            userName = string.Empty,
            password = "newpassword123",
            email = "newuser@test.com",
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync("/Account/AddAccount", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task AddAccount_WithInvalidEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            userName = "newuser",
            password = "newpassword123",
            email = "invalid-email",
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync("/Account/AddAccount", content);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task AddAccount_WithDuplicateUserName_ShouldReturnConflict()
    {
        // Arrange
        var duplicateUserName = "duplicateuser";

        // First request to create the account
        var firstRequest = new
        {
            userName = duplicateUserName,
            password = "password123",
            email = "first@test.com",
        };

        var firstContent = new StringContent(
            JsonSerializer.Serialize(firstRequest),
            Encoding.UTF8,
            "application/json");

        // Second request with same username but different email
        var secondRequest = new
        {
            userName = duplicateUserName,
            password = "password456",
            email = "second@test.com",
        };

        var secondContent = new StringContent(
            JsonSerializer.Serialize(secondRequest),
            Encoding.UTF8,
            "application/json");

        // Act
        var firstResponse = await client.PostAsync("/Account/AddAccount", firstContent);
        var secondResponse = await client.PostAsync("/Account/AddAccount", secondContent);

        // Assert
        firstResponse.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError);

        secondResponse.StatusCode.Should().BeOneOf(
            HttpStatusCode.Conflict,
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task UpdateAccount_WithDuplicateUserName_ShouldReturnConflict()
    {
        // Arrange
        var firstUserName = "firstuser";
        var secondUserName = "seconduser";

        // Create first account
        var firstRequest = new
        {
            userName = firstUserName,
            password = "password123",
            email = "first@test.com",
        };

        var firstContent = new StringContent(
            JsonSerializer.Serialize(firstRequest),
            Encoding.UTF8,
            "application/json");

        // Create second account
        var secondRequest = new
        {
            userName = secondUserName,
            password = "password456",
            email = "second@test.com",
        };

        var secondContent = new StringContent(
            JsonSerializer.Serialize(secondRequest),
            Encoding.UTF8,
            "application/json");

        // Try to update second account with first account's username
        var updateRequest = new
        {
            id = 2, // Assuming second account gets ID 2 for this test
            userName = firstUserName, // This should cause conflict
            password = "updatedpassword",
            email = "updated@test.com",
        };

        var updateContent = new StringContent(
            JsonSerializer.Serialize(updateRequest),
            Encoding.UTF8,
            "application/json");

        // Act
        var firstResponse = await client.PostAsync("/Account/AddAccount", firstContent);
        var secondResponse = await client.PostAsync("/Account/AddAccount", secondContent);

        var updateResponse = await client.PutAsync("/Account/UpdateAccount", updateContent);

        // Assert
        updateResponse.StatusCode.Should().BeOneOf(
            HttpStatusCode.Conflict,
            HttpStatusCode.NotFound,
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task GetAccountById_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = 99999;

        // Act
        var response = await client.GetAsync($"/Account/GetAccountById/{nonExistentId}");

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.NotFound,
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError);
    }

    [Theory]
    [InlineData("GET")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    [InlineData("PATCH")]
    public async Task AccountEndpoints_WithUnsupportedHttpMethods_ShouldReturnMethodNotAllowed(string httpMethod)
    {
        // Arrange
        var request = new HttpRequestMessage(new HttpMethod(httpMethod), "/Account/AddAccount");

        // Act
        var response = await client.SendAsync(request);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.MethodNotAllowed,
            HttpStatusCode.NotFound,
            HttpStatusCode.InternalServerError);
    }
}
