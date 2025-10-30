using System.Globalization;
using Authentication.API.Resource;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class ResourceStartupTests
    {
        [Fact]
        public void ResourceStartup_AuthenticationApiDisplayName_ReturnsCorrectValue()
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentUICulture;
            CultureInfo.CurrentUICulture = new CultureInfo("en", false);

            try
            {
                // Act
                var result = ResourceStartup.AuthenticationApiDisplayName;

                // Assert
                Assert.Equal("Authentication API", result);
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalCulture;
            }
        }

        [Fact]
        public void ResourceStartup_AllControllerNames_AreNotNull()
        {
            // Act & Assert
            Assert.NotNull(ResourceStartup.AuthenticationController);
            Assert.NotNull(ResourceStartup.AccountController);
            Assert.NotNull(ResourceStartup.ActionController);
            Assert.NotNull(ResourceStartup.ClaimController);
            Assert.NotNull(ResourceStartup.ClaimActionController);
            Assert.NotNull(ResourceStartup.AccountClaimActionController);

            Assert.NotEmpty(ResourceStartup.AuthenticationController);
            Assert.NotEmpty(ResourceStartup.AccountController);
            Assert.NotEmpty(ResourceStartup.ActionController);
            Assert.NotEmpty(ResourceStartup.ClaimController);
            Assert.NotEmpty(ResourceStartup.ClaimActionController);
            Assert.NotEmpty(ResourceStartup.AccountClaimActionController);
        }

        [Fact]
        public void ResourceStartup_AllSwaggerDescriptions_AreNotNull()
        {
            // Act & Assert
            Assert.NotNull(ResourceStartup.SwaggerAuthenticationDescription);
            Assert.NotNull(ResourceStartup.SwaggerAccessControlDescription);
            Assert.NotNull(ResourceStartup.AuthenticationApiDisplayName);
            Assert.NotNull(ResourceStartup.AccessControlApiDisplayName);

            Assert.NotEmpty(ResourceStartup.SwaggerAuthenticationDescription);
            Assert.NotEmpty(ResourceStartup.SwaggerAccessControlDescription);
            Assert.NotEmpty(ResourceStartup.AuthenticationApiDisplayName);
            Assert.NotEmpty(ResourceStartup.AccessControlApiDisplayName);
        }

        [Theory]
        [InlineData("en", "API endpoints for user authentication and token management")]
        public void ResourceStartup_SwaggerAuthenticationDescription_ReturnsCorrectTranslation(string culture, string expected)
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentUICulture;
            var testCulture = new CultureInfo(culture, false);

            try
            {
                // Act
                CultureInfo.CurrentUICulture = testCulture;
                ResourceStartup.Culture = testCulture;
                var result = ResourceStartup.SwaggerAuthenticationDescription;

                // Assert
                Assert.Equal(expected, result);
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalCulture;
                ResourceStartup.Culture = originalCulture;
            }
        }

        [Theory]
        [InlineData("en", "API endpoints for managing access control including accounts, claims, and actions")]
        public void ResourceStartup_SwaggerAccessControlDescription_ReturnsCorrectTranslation(string culture, string expected)
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentUICulture;
            var testCulture = new CultureInfo(culture, false);

            try
            {
                // Act
                CultureInfo.CurrentUICulture = testCulture;
                ResourceStartup.Culture = testCulture;
                var result = ResourceStartup.SwaggerAccessControlDescription;

                // Assert
                Assert.Equal(expected, result);
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalCulture;
                ResourceStartup.Culture = originalCulture;
            }
        }
    }
}
