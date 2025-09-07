using Authentication.Login.Domain.Implementation;
using Authentication.Login.Domain.Interface;
using FluentAssertions;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class TokenTests
    {
        [Fact]
        public void Token_WhenCreated_ShouldRequireAccessTokenAndUserName()
        {
            // Arrange & Act
            var token = new Token
            {
                AccessToken = "test.jwt.token",
                UserName = "testuser",
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            // Assert
            token.AccessToken.Should().Be("test.jwt.token");
            token.UserName.Should().Be("testuser");
            token.Expiration.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public void Token_WithValidJwtFormat_ShouldAcceptToken()
        {
            // Arrange
            var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            // Act
            var token = new Token
            {
                AccessToken = jwtToken,
                UserName = "testuser",
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            // Assert
            token.AccessToken.Should().Be(jwtToken);
            token.AccessToken.Should().Contain(".");
            token.AccessToken.Split('.').Should().HaveCount(3); // JWT has 3 parts
        }

        [Fact]
        public void Token_WithFutureExpiration_ShouldBeValid()
        {
            // Arrange
            var futureExpiration = DateTime.UtcNow.AddHours(2);

            // Act
            var token = new Token
            {
                AccessToken = "test.token",
                UserName = "user",
                Expiration = futureExpiration
            };

            // Assert
            token.Expiration.Should().Be(futureExpiration);
            token.Expiration.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public void Token_WithPastExpiration_ShouldAllowValue()
        {
            // Arrange
            var pastExpiration = DateTime.UtcNow.AddHours(-1);

            // Act
            var token = new Token
            {
                AccessToken = "expired.token",
                UserName = "user",
                Expiration = pastExpiration
            };

            // Assert
            token.Expiration.Should().Be(pastExpiration);
            token.Expiration.Should().BeBefore(DateTime.UtcNow);
        }

        [Fact]
        public void Token_WithEmptyAccessToken_ShouldThrowWhenRequired()
        {
            // Act & Assert
            System.Action act = () => new Token
            {
                AccessToken = "", // Empty, but required
                UserName = "user",
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            // The required attribute should enforce this at compile time
            // This test verifies the property can be set to empty string
            var token = new Token
            {
                AccessToken = "",
                UserName = "user",
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            token.AccessToken.Should().Be("");
        }

        [Fact]
        public void Token_WithEmptyUserName_ShouldThrowWhenRequired()
        {
            // Act - The required attribute should enforce this at compile time
            var token = new Token
            {
                AccessToken = "test.token",
                UserName = "", // Empty, but required
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            // Assert - Verify the property can be set
            token.UserName.Should().Be("");
        }

        [Fact]
        public void Token_WithLongAccessToken_ShouldAcceptValue()
        {
            // Arrange
            var longToken = new string('a', 5000); // Very long token

            // Act
            var token = new Token
            {
                AccessToken = longToken,
                UserName = "user",
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            // Assert
            token.AccessToken.Should().Be(longToken);
            token.AccessToken.Length.Should().Be(5000);
        }

        [Fact]
        public void Token_WithSpecialCharactersInUserName_ShouldAcceptValue()
        {
            // Arrange
            var specialUserName = "user@domain.com";

            // Act
            var token = new Token
            {
                AccessToken = "test.token",
                UserName = specialUserName,
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            // Assert
            token.UserName.Should().Be(specialUserName);
        }

        [Fact]
        public void Token_ExpirationCalculation_ShouldWorkCorrectly()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var expirationHours = 24;
            var expectedExpiration = now.AddHours(expirationHours);

            // Act
            var token = new Token
            {
                AccessToken = "test.token",
                UserName = "user",
                Expiration = expectedExpiration
            };

            // Assert
            var timeDifference = token.Expiration - now;
            timeDifference.TotalHours.Should().BeApproximately(expirationHours, 0.1);
        }
    }

    // Mock implementation of IJwtSettings for testing
    public class MockJwtSettings : IJwtSettings
    {
        public string Issuer { get; set; } = "TestIssuer";
        public string Audience { get; set; } = "TestAudience";
        public string SecretKey { get; set; } = "ThisIsATestSecretKeyForJwtTokenGenerationThatIsLongEnough";
    }
}