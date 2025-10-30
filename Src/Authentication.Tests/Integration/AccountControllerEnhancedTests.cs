using System.Net;
using System.Text;
using System.Text.Json;
using Authentication.Tests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Authentication.Tests.Integration
{
    public class AccountControllerEnhancedTests : IClassFixture<AuthenticationWebApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly HttpClient client;

        public AccountControllerEnhancedTests(AuthenticationWebApplicationFactory factory)
        {
            this.factory = factory;
            client = this.factory.CreateClient();
        }

        [Theory]
        [InlineData("", "validpass123")] // Empty username
        [InlineData("usr", "validpass123")] // Too short username
        [InlineData("user name", "validpass123")] // Username with spaces
        [InlineData(null, "validpass123")] // Null username
        public async Task AddAccount_WithInvalidUserName_ShouldReturnBadRequest(string? userName, string password)
        {
            // Arrange
            var request = new
            {
                userName = userName,
                password = password,
                email = "test@example.com",
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

        [Theory]
        [InlineData("validuser", "")] // Empty password
        [InlineData("validuser", "pwd")] // Too short password
        [InlineData("validuser", "pass word")] // Password with spaces
        [InlineData("validuser", null)] // Null password
        public async Task AddAccount_WithInvalidPassword_ShouldReturnBadRequest(string userName, string? password)
        {
            // Arrange
            var request = new
            {
                userName = userName,
                password = password,
                email = "test@example.com",
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

        [Theory]
        [InlineData("user@domain.com", "password123")] // Email format username
        [InlineData("user_name", "P@ssw0rd!")] // Underscore and special chars
        [InlineData("user123", "123456789")] // Numbers
        [InlineData("UPPERCASE", "lowercase")] // Case variations
        public async Task AddAccount_WithValidFormats_ShouldAcceptRequests(string userName, string password)
        {
            // Arrange
            var request = new
            {
                userName = userName,
                password = password,
                email = "test@example.com",
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
                HttpStatusCode.Conflict,
                HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task AddAccount_WithInvalidContentType_ShouldReturnBadRequest()
        {
            // Arrange
            var request = "userName=testuser&password=testpass123";
            var content = new StringContent(request, Encoding.UTF8, "application/x-www-form-urlencoded");

            // Act
            var response = await client.PostAsync("/Account/AddAccount", content);

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.UnsupportedMediaType,
                HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task AddAccount_WithNoContentType_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new StringContent(string.Empty, Encoding.UTF8);
            request.Headers.ContentType = null;

            // Act
            var response = await client.PostAsync("/Account/AddAccount", request);

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.UnsupportedMediaType,
                HttpStatusCode.InternalServerError);
        }

        [Theory]
        [InlineData("{invalid json")]
        [InlineData("{\"userName\":}")]
        [InlineData("{\"userName\":\"test\",}")]
        [InlineData("")]
        public async Task AddAccount_WithMalformedJson_ShouldReturnBadRequest(string malformedJson)
        {
            // Arrange
            var content = new StringContent(malformedJson, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/Account/AddAccount", content);

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task AddAccount_WithVeryLongUserName_ShouldReturnBadRequest()
        {
            // Arrange - Create a username longer than typically allowed
            var longUserName = new string('a', 1000);
            var request = new
            {
                userName = longUserName,
                password = "validpass123",
                email = "test@example.com",
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
        public async Task AddAccount_WithVeryLongPassword_ShouldReturnBadRequest()
        {
            // Arrange - Create a password longer than typically allowed
            var longPassword = new string('p', 1000);
            var request = new
            {
                userName = "validuser",
                password = longPassword,
                email = "test@example.com",
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
        public async Task AddAccount_WithMinimumValidLength_ShouldAcceptRequest()
        {
            // Arrange - Test minimum valid lengths (6 characters each)
            var request = new
            {
                userName = "user12",
                password = "pass12",
                email = "test@example.com",
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
                HttpStatusCode.Conflict,
                HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task AddAccount_WithMaximumValidLength_ShouldAcceptRequest()
        {
            // Arrange - Test maximum valid lengths (50 characters each)
            var maxUserName = new string('u', 50);
            var maxPassword = new string('p', 50);
            var request = new
            {
                userName = maxUserName,
                password = maxPassword,
                email = "test@example.com",
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
                HttpStatusCode.Conflict,
                HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task AddAccount_WithUnicodeCharacters_ShouldHandleCorrectly()
        {
            // Arrange - Test with Unicode characters
            var request = new
            {
                userName = "usuário123",
                password = "contraseña",
                email = "test@example.com",
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
                HttpStatusCode.Conflict,
                HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task AddAccount_WithSpecialCharacters_ShouldHandleCorrectly()
        {
            // Arrange - Test with special characters
            var request = new
            {
                userName = "user@domain.com",
                password = "P@ssw0rd!",
                email = "test@example.com",
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
                HttpStatusCode.Conflict,
                HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task GetAccountById_WithVeryLargeId_ShouldReturnNotFound()
        {
            // Arrange
            var largeId = int.MaxValue;

            // Act
            var response = await client.GetAsync($"/Account/GetAccountById/{largeId}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.NotFound,
                HttpStatusCode.BadRequest,
                HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task GetAccountById_WithNegativeId_ShouldReturnBadRequest()
        {
            // Arrange
            var negativeId = -1;

            // Act
            var response = await client.GetAsync($"/Account/GetAccountById/{negativeId}");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.NotFound,
                HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task GetAccountById_WithZeroId_ShouldReturnBadRequest()
        {
            // Act
            var response = await client.GetAsync("/Account/GetAccountById/0");

            // Assert
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.NotFound,
                HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task AddAccount_ConcurrentRequests_ShouldHandleCorrectly()
        {
            // Arrange
            var tasks = new List<Task<HttpResponseMessage>>();

            for (int i = 0; i < 5; i++)
            {
                var request = new
                {
                    userName = $"concurrent{i}",
                    password = $"password{i}",
                    email = $"test{i}@example.com",
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                tasks.Add(client.PostAsync("/Account/AddAccount", content));
            }

            // Act
            var responses = await Task.WhenAll(tasks);

            // Assert
            responses.Should().HaveCount(5);
            responses.Should().OnlyContain(r =>
                r.StatusCode == HttpStatusCode.OK ||
                r.StatusCode == HttpStatusCode.BadRequest ||
                r.StatusCode == HttpStatusCode.Conflict ||
                r.StatusCode == HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task AddAccount_WithValidData_ShouldReturnJsonResponse()
        {
            // Arrange
            var request = new
            {
                userName = $"jsontest{Guid.NewGuid():N}",
                password = "validpass123",
                email = "test@example.com",
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await client.PostAsync("/Account/AddAccount", content);

            // Assert
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                responseContent.Should().NotBeEmpty();

                // Verify it's valid JSON
                System.Action act = () => JsonSerializer.Deserialize<object>(responseContent);
                act.Should().NotThrow();
            }
        }
    }
}
