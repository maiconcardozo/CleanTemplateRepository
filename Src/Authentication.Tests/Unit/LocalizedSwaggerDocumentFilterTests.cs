using Authentication.API.Resource;
using Authentication.API.Swagger;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;

namespace Authentication.Tests.Unit
{
    public class LocalizedSwaggerDocumentFilterTests
    {
        [Fact]
        public void Apply_ReplacesResourceKeysWithLocalizedDescriptions()
        {
            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var filter = new LocalizedSwaggerDocumentFilter(mockHttpContextAccessor.Object);
            var swaggerDoc = new OpenApiDocument
            {
                Tags = new List<OpenApiTag>
                {
                    new OpenApiTag 
                    { 
                        Name = "Authentication", 
                        Description = "ResourceAPI.AuthenticationControllerDescription" 
                    },
                    new OpenApiTag 
                    { 
                        Name = "Action", 
                        Description = "ResourceAPI.ActionControllerDescription" 
                    },
                    new OpenApiTag 
                    { 
                        Name = "Account", 
                        Description = "ResourceAPI.AccountControllerDescription" 
                    },
                    new OpenApiTag 
                    { 
                        Name = "Regular", 
                        Description = "This is a regular description" 
                    }
                }
            };
            var context = new DocumentFilterContext(null, null, null);

            // Act
            filter.Apply(swaggerDoc, context);

            // Assert
            Assert.Equal(ResourceAPI.AuthenticationControllerDescription, swaggerDoc.Tags[0].Description);
            Assert.Equal(ResourceAPI.ActionControllerDescription, swaggerDoc.Tags[1].Description);
            Assert.Equal(ResourceAPI.AccountControllerDescription, swaggerDoc.Tags[2].Description);
            Assert.Equal("This is a regular description", swaggerDoc.Tags[3].Description); // Should remain unchanged
        }

        [Fact]
        public void Apply_HandlesNullTags()
        {
            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var filter = new LocalizedSwaggerDocumentFilter(mockHttpContextAccessor.Object);
            var swaggerDoc = new OpenApiDocument
            {
                Tags = null
            };
            var context = new DocumentFilterContext(null, null, null);

            // Act & Assert - Should not throw
            filter.Apply(swaggerDoc, context);
        }

        [Fact]
        public void Apply_HandlesInvalidResourceKeys()
        {
            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var filter = new LocalizedSwaggerDocumentFilter(mockHttpContextAccessor.Object);
            var swaggerDoc = new OpenApiDocument
            {
                Tags = new List<OpenApiTag>
                {
                    new OpenApiTag 
                    { 
                        Name = "Test", 
                        Description = "ResourceAPI.NonExistentKey" 
                    }
                }
            };
            var context = new DocumentFilterContext(null, null, null);

            // Act
            filter.Apply(swaggerDoc, context);

            // Assert - Should remain unchanged when resource key doesn't exist
            Assert.Equal("ResourceAPI.NonExistentKey", swaggerDoc.Tags[0].Description);
        }
    }
}