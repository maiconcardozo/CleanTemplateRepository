using System.Reflection;
using Authentication.API.Controllers;
using Authentication.API.Resource;
using Authentication.API.Swagger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class LocalizedSwaggerOperationFilterTests
    {
        [Fact]
        public void LocalizedSwaggerOperationFilter_GetAccountsMethod_SetsCorrectValues()
        {
            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var filter = new LocalizedSwaggerOperationFilter(mockHttpContextAccessor.Object);
            var operation = new OpenApiOperation();
            operation.Responses = new OpenApiResponses();
            operation.Responses.Add("200", new OpenApiResponse { Description = "Success" });
            operation.Responses.Add("400", new OpenApiResponse { Description = "Bad Request" });
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("500", new OpenApiResponse { Description = "Server Error" });

            var methodInfo = typeof(AccountController).GetMethod("GetAccounts");
            var context = new OperationFilterContext(
                null!,
                null!,
                null!,
                methodInfo!);

            // Act
            filter.Apply(operation, context);

            // Assert
            Assert.Equal(ResourceAPI.GetAccounts, operation.Summary);
            Assert.Equal(ResourceAPI.DocumentationGetAccounts, operation.Description);
            Assert.Equal(ResourceAPI.AccountsRetrievedSuccessfully, operation.Responses["200"].Description);
            Assert.Equal(ResourceAPI.InvalidDataValidationError, operation.Responses["400"].Description);
            Assert.Equal(ResourceAPI.UserNotAuthorized, operation.Responses["401"].Description);
            Assert.Equal(ResourceAPI.AnUnexpectedErrorOccurredAccountsCouldNotBeRetrieved, operation.Responses["500"].Description);
        }

        [Fact]
        public void LocalizedSwaggerOperationFilter_GetAccountByIdMethod_SetsCorrectValues()
        {
            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var filter = new LocalizedSwaggerOperationFilter(mockHttpContextAccessor.Object);
            var operation = new OpenApiOperation();
            operation.Responses = new OpenApiResponses();
            operation.Responses.Add("200", new OpenApiResponse { Description = "Success" });
            operation.Responses.Add("400", new OpenApiResponse { Description = "Bad Request" });
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("404", new OpenApiResponse { Description = "Not Found" });
            operation.Responses.Add("500", new OpenApiResponse { Description = "Server Error" });

            var methodInfo = typeof(AccountController).GetMethod("GetAccountById");
            var context = new OperationFilterContext(
                null!,
                null!,
                null!,
                methodInfo!);

            // Act
            filter.Apply(operation, context);

            // Assert
            Assert.Equal(ResourceAPI.GetAccountById, operation.Summary);
            Assert.Equal(ResourceAPI.DocumentationGetAccountById, operation.Description);
            Assert.Equal(ResourceAPI.AccountRetrievedSuccessfully, operation.Responses["200"].Description);
            Assert.Equal(ResourceAPI.InvalidDataValidationError, operation.Responses["400"].Description);
            Assert.Equal(ResourceAPI.UserNotAuthorized, operation.Responses["401"].Description);
            Assert.Equal(ResourceAPI.AccountNotFound, operation.Responses["404"].Description);
            Assert.Equal(ResourceAPI.AnUnexpectedErrorOccurredAccountCouldNotBeRetrieved, operation.Responses["500"].Description);
        }

        [Fact]
        public void LocalizedSwaggerOperationFilter_UpdateAccountMethod_SetsCorrectValues()
        {
            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var filter = new LocalizedSwaggerOperationFilter(mockHttpContextAccessor.Object);
            var operation = new OpenApiOperation();
            operation.Responses = new OpenApiResponses();
            operation.Responses.Add("200", new OpenApiResponse { Description = "Success" });
            operation.Responses.Add("400", new OpenApiResponse { Description = "Bad Request" });
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("404", new OpenApiResponse { Description = "Not Found" });
            operation.Responses.Add("500", new OpenApiResponse { Description = "Server Error" });

            var methodInfo = typeof(AccountController).GetMethod("UpdateAccount");
            var context = new OperationFilterContext(
                null!,
                null!,
                null!,
                methodInfo!);

            // Act
            filter.Apply(operation, context);

            // Assert
            Assert.Equal(ResourceAPI.UpdateAccount, operation.Summary);
            Assert.Equal(ResourceAPI.DocumentationUpdateAccount, operation.Description);
            Assert.Equal(ResourceAPI.AccountUpdatedSuccessfully, operation.Responses["200"].Description);
            Assert.Equal(ResourceAPI.InvalidDataValidationError, operation.Responses["400"].Description);
            Assert.Equal(ResourceAPI.UserNotAuthorized, operation.Responses["401"].Description);
            Assert.Equal(ResourceAPI.AccountNotFound, operation.Responses["404"].Description);
            Assert.Equal(ResourceAPI.AnUnexpectedErrorOccurredAccountCouldNotBeUpdated, operation.Responses["500"].Description);
        }

        [Fact]
        public void LocalizedSwaggerOperationFilter_DeleteAccountMethod_SetsCorrectValues()
        {
            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var filter = new LocalizedSwaggerOperationFilter(mockHttpContextAccessor.Object);
            var operation = new OpenApiOperation();
            operation.Responses = new OpenApiResponses();
            operation.Responses.Add("200", new OpenApiResponse { Description = "Success" });
            operation.Responses.Add("400", new OpenApiResponse { Description = "Bad Request" });
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("404", new OpenApiResponse { Description = "Not Found" });
            operation.Responses.Add("500", new OpenApiResponse { Description = "Server Error" });

            var methodInfo = typeof(AccountController).GetMethod("DeleteAccount");
            var context = new OperationFilterContext(
                null!,
                null!,
                null!,
                methodInfo!);

            // Act
            filter.Apply(operation, context);

            // Assert
            Assert.Equal(ResourceAPI.DeleteAccount, operation.Summary);
            Assert.Equal(ResourceAPI.DocumentationDeleteAccount, operation.Description);
            Assert.Equal(ResourceAPI.AccountDeletedSuccessfully, operation.Responses["200"].Description);
            Assert.Equal(ResourceAPI.InvalidDataValidationError, operation.Responses["400"].Description);
            Assert.Equal(ResourceAPI.UserNotAuthorized, operation.Responses["401"].Description);
            Assert.Equal(ResourceAPI.AccountNotFound, operation.Responses["404"].Description);
            Assert.Equal(ResourceAPI.AnUnexpectedErrorOccurredAccountCouldNotBeDeleted, operation.Responses["500"].Description);
        }

        [Fact]
        public void LocalizedSwaggerOperationFilter_UnknownMethod_DoesNotModifyOperation()
        {
            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var filter = new LocalizedSwaggerOperationFilter(mockHttpContextAccessor.Object);
            var operation = new OpenApiOperation();
            var originalSummary = operation.Summary;
            var originalDescription = operation.Description;

            var methodInfo = typeof(LocalizedSwaggerOperationFilterTests).GetMethod("LocalizedSwaggerOperationFilter_UnknownMethod_DoesNotModifyOperation");
            var context = new OperationFilterContext(
                null!,
                null!,
                null!,
                methodInfo!);

            // Act
            filter.Apply(operation, context);

            // Assert
            Assert.Equal(originalSummary, operation.Summary);
            Assert.Equal(originalDescription, operation.Description);
        }
    }
}
