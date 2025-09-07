using System.Globalization;
using Authentication.API.Resource;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class LocalizationTests
    {
        [Theory]
        [InlineData("en", "Account created successfully.")]
        [InlineData("pt-BR", "Conta criada com sucesso.")]
        public void ResourceAPI_AccountCreatedSuccessfully_ReturnsCorrectTranslation(string culture, string expected)
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentUICulture;
            var testCulture = new CultureInfo(culture);
            
            try
            {
                // Act
                CultureInfo.CurrentUICulture = testCulture;
                ResourceAPI.Culture = testCulture;
                var result = ResourceAPI.AccountCreatedSuccessfully;
                
                // Assert
                Assert.Equal(expected, result);
            }
            finally
            {
                // Cleanup
                CultureInfo.CurrentUICulture = originalCulture;
                ResourceAPI.Culture = originalCulture;
            }
        }

        [Theory]
        [InlineData("en", "API endpoints for user authentication and token management")]
        public void ResourceStartup_SwaggerAuthenticationDescription_ReturnsCorrectTranslation(string culture, string expected)
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentUICulture;
            var testCulture = new CultureInfo(culture);
            
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
                // Cleanup
                CultureInfo.CurrentUICulture = originalCulture;
                ResourceAPI.Culture = originalCulture;
            }
        }

        [Fact]
        public void ResourceAPI_FallsBackToEnglishForUnsupportedCulture()
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentUICulture;
            var unsupportedCulture = new CultureInfo("fr-FR"); // French not yet supported
            
            try
            {
                // Act
                CultureInfo.CurrentUICulture = unsupportedCulture;
                ResourceAPI.Culture = unsupportedCulture;
                var result = ResourceAPI.AccountCreatedSuccessfully;
                
                // Assert - Should fallback to English
                Assert.Equal("Account created successfully.", result);
            }
            finally
            {
                // Cleanup
                CultureInfo.CurrentUICulture = originalCulture;
                ResourceAPI.Culture = originalCulture;
            }
        }

        [Fact]
        public void ResourceStartup_HasAllRequiredSwaggerDescriptions()
        {
            // Test that our new resource properties exist and are not empty
            Assert.NotNull(ResourceStartup.SwaggerAuthenticationDescription);
            Assert.NotEmpty(ResourceStartup.SwaggerAuthenticationDescription);
            
            Assert.NotNull(ResourceStartup.SwaggerAccessControlDescription);
            Assert.NotEmpty(ResourceStartup.SwaggerAccessControlDescription);
        }

        [Fact]
        public void ResourceAPI_HasAllControllerDescriptions()
        {
            // Test that our new controller description resource properties exist and are not empty
            Assert.NotNull(ResourceAPI.ActionControllerDescription);
            Assert.NotEmpty(ResourceAPI.ActionControllerDescription);
            
            Assert.NotNull(ResourceAPI.AccountControllerDescription);
            Assert.NotEmpty(ResourceAPI.AccountControllerDescription);
            
            Assert.NotNull(ResourceAPI.AccountClaimActionControllerDescription);
            Assert.NotEmpty(ResourceAPI.AccountClaimActionControllerDescription);
            
            Assert.NotNull(ResourceAPI.ClaimActionControllerDescription);
            Assert.NotEmpty(ResourceAPI.ClaimActionControllerDescription);
            
            Assert.NotNull(ResourceAPI.ClaimControllerDescription);
            Assert.NotEmpty(ResourceAPI.ClaimControllerDescription);
            
            Assert.NotNull(ResourceAPI.AuthenticationControllerDescription);
            Assert.NotEmpty(ResourceAPI.AuthenticationControllerDescription);
        }

        [Fact]
        public void ResourceAPI_HasAllAccountOperationDescriptions()
        {
            // Test that our new Account operation resource properties exist and are not empty
            Assert.NotNull(ResourceAPI.GetAccounts);
            Assert.NotEmpty(ResourceAPI.GetAccounts);
            
            Assert.NotNull(ResourceAPI.DocumentationGetAccounts);
            Assert.NotEmpty(ResourceAPI.DocumentationGetAccounts);
            
            Assert.NotNull(ResourceAPI.GetAccountById);
            Assert.NotEmpty(ResourceAPI.GetAccountById);
            
            Assert.NotNull(ResourceAPI.DocumentationGetAccountById);
            Assert.NotEmpty(ResourceAPI.DocumentationGetAccountById);
            
            Assert.NotNull(ResourceAPI.UpdateAccount);
            Assert.NotEmpty(ResourceAPI.UpdateAccount);
            
            Assert.NotNull(ResourceAPI.DocumentationUpdateAccount);
            Assert.NotEmpty(ResourceAPI.DocumentationUpdateAccount);
            
            Assert.NotNull(ResourceAPI.DeleteAccount);
            Assert.NotEmpty(ResourceAPI.DeleteAccount);
            
            Assert.NotNull(ResourceAPI.DocumentationDeleteAccount);
            Assert.NotEmpty(ResourceAPI.DocumentationDeleteAccount);
        }

        [Theory]
        [InlineData("en", "Account not found.")]
        [InlineData("pt-BR", "Conta não encontrada.")]
        public void ResourceAPI_AccountNotFound_ReturnsCorrectTranslation(string culture, string expected)
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentUICulture;
            var testCulture = new CultureInfo(culture);
            
            try
            {
                // Act
                CultureInfo.CurrentUICulture = testCulture;
                ResourceAPI.Culture = testCulture;
                var result = ResourceAPI.AccountNotFound;
                
                // Assert
                Assert.Equal(expected, result);
            }
            finally
            {
                // Cleanup
                CultureInfo.CurrentUICulture = originalCulture;
                ResourceAPI.Culture = originalCulture;
            }
        }

        [Theory]
        [InlineData("en", "Get all accounts")]
        [InlineData("pt-BR", "Obter todas as contas")]
        public void ResourceAPI_GetAccounts_ReturnsCorrectTranslation(string culture, string expected)
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentUICulture;
            var testCulture = new CultureInfo(culture);
            
            try
            {
                // Act
                CultureInfo.CurrentUICulture = testCulture;
                ResourceAPI.Culture = testCulture;
                var result = ResourceAPI.GetAccounts;
                
                // Assert
                Assert.Equal(expected, result);
            }
            finally
            {
                // Cleanup
                CultureInfo.CurrentUICulture = originalCulture;
                ResourceAPI.Culture = originalCulture;
            }
        }

        [Fact]
        public void ResourceAPI_ValidateAllPortugueseTranslationsExist()
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentUICulture;
            var ptCulture = new CultureInfo("pt-BR");
            
            try
            {
                // Act & Assert - Test all the newly added Portuguese translations
                CultureInfo.CurrentUICulture = ptCulture;
                ResourceAPI.Culture = ptCulture;
                
                // Test all the translations that were recently added
                Assert.Equal("Conta não encontrada.", ResourceAPI.AccountNotFound);
                Assert.Equal("Conta recuperada com sucesso.", ResourceAPI.AccountRetrievedSuccessfully);
                Assert.Equal("Excluir conta", ResourceAPI.DeleteAccount);
                Assert.Equal("Remove uma conta de usuário do sistema pelo seu identificador único.", ResourceAPI.DocumentationDeleteAccount);
                Assert.Equal("Recupera uma conta de usuário específica pelo seu identificador único.", ResourceAPI.DocumentationGetAccountById);
                Assert.Equal("Recupera todas as contas de usuário no sistema.", ResourceAPI.DocumentationGetAccounts);
                Assert.Equal("Atualiza uma conta de usuário existente com as informações fornecidas.", ResourceAPI.DocumentationUpdateAccount);
                Assert.Equal("Obter conta por ID", ResourceAPI.GetAccountById);
                Assert.Equal("Obter todas as contas", ResourceAPI.GetAccounts);
                Assert.Equal("Atualizar conta", ResourceAPI.UpdateAccount);
            }
            finally
            {
                // Cleanup
                CultureInfo.CurrentUICulture = originalCulture;
                ResourceAPI.Culture = originalCulture;
            }
        }
    }
}