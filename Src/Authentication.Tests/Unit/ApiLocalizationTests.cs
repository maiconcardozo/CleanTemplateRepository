using Authentication.API.Resource;
using System.Globalization;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class ApiLocalizationTests
    {
        // TODO: Update these tests once ResourceStartup entries are properly regenerated
        //[Fact]
        //public void CleanEntityApiDisplayName_Returns_EnglishValue_ForEnglishCulture()
        //{
        //    // Arrange
        //    var originalCulture = CultureInfo.CurrentUICulture;
        //    CultureInfo.CurrentUICulture = new CultureInfo("en");

        //    try
        //    {
        //        // Act
        //        var result = ResourceStartup.CleanEntityApiDisplayName;

        //        // Assert
        //        Assert.Equal("CleanEntity API", result);
        //    }
        //    finally
        //    {
        //        CultureInfo.CurrentUICulture = originalCulture;
        //    }
        //}

        [Fact]
        public void AuthenticationApiDisplayName_Returns_PortugueseValue_ForPortugueseCulture()
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentUICulture;
            var ptCulture = new CultureInfo("pt-BR");
            CultureInfo.CurrentUICulture = ptCulture;
            ResourceAPI.Culture = ptCulture;

            try
            {
                // Act
                var result = ResourceAPI.AuthenticationApiTitle;

                // Assert
                Assert.Equal("API de Autenticação", result);
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalCulture;
                ResourceAPI.Culture = originalCulture;
            }
        }

        // TODO: Update this test once ResourceStartup entries are properly regenerated  
        //[Fact]
        //public void CleanEntityController_Returns_EnglishValue_ForEnglishCulture()
        //{
        //    // Arrange
        //    var originalCulture = CultureInfo.CurrentUICulture;
        //    CultureInfo.CurrentUICulture = new CultureInfo("en");

        //    try
        //    {
        //        // Act
        //        var result = ResourceStartup.CleanEntityController;

        //        // Assert
        //        Assert.Equal("CleanEntity", result);
        //    }
        //    finally
        //    {
        //        CultureInfo.CurrentUICulture = originalCulture;
        //    }
        //}



        [Fact]
        public void AuthenticationControllerDescription_Returns_PortugueseValue_ForPortugueseCulture()
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentUICulture;
            var ptCulture = new CultureInfo("pt-BR");
            CultureInfo.CurrentUICulture = ptCulture;
            ResourceAPI.Culture = ptCulture;

            try
            {
                // Act
                var result = ResourceAPI.AuthenticationControllerDescription;

                // Assert
                Assert.Equal("Controller responsável por operações de autenticação. Fornece endpoints para autenticação de usuários.", result);
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalCulture;
                ResourceAPI.Culture = originalCulture;
            }
        }
    }
}