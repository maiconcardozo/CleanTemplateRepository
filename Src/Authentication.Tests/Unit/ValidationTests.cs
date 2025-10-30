using System.Runtime.CompilerServices;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Authentication.Tests.Unit;

public class ValidationTests
{
    [Fact]
    public async Task ValidationHelper_WithValidEntity_ShouldReturnNull()
    {
        // Arrange
        var entity = new TestEntity { Name = "ValidName", Email = "valid@email.com" };
        var validator = new Mock<IValidator<TestEntity>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IValidator<TestEntity>)))
                      .Returns(validator.Object);

        var controller = new TestController();

        // Act
        var result = await ValidationHelper.ValidateEntityAsync(entity, serviceProvider.Object, controller);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ValidationHelper_WithInvalidEntity_ShouldReturnBadRequest()
    {
        // Arrange
        var entity = new TestEntity { Name = string.Empty, Email = "invalid-email" };
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required"),
            new ValidationFailure("Email", "Email is invalid"),
        };
        var validationResult = new ValidationResult(validationFailures);

        var validator = new Mock<IValidator<TestEntity>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IValidator<TestEntity>)))
                      .Returns(validator.Object);

        var controller = new TestController();

        // Act
        var result = await ValidationHelper.ValidateEntityAsync(entity, serviceProvider.Object, controller);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestObjectResult>();

        var badRequestResult = result as BadRequestObjectResult;
        controller.ModelState.Should().ContainKey("Name");
        controller.ModelState.Should().ContainKey("Email");
    }

    [Fact]
    public async Task ValidationHelper_WithNoValidator_ShouldReturnNull()
    {
        // Arrange
        var entity = new TestEntity { Name = "TestName", Email = "test@email.com" };
        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IValidator<TestEntity>)))
                      .Returns((IValidator<TestEntity>?)null);

        var controller = new TestController();

        // Act
        var result = await ValidationHelper.ValidateEntityAsync(entity, serviceProvider.Object, controller);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void AccountPayloadValidation_WithValidData_ShouldPass()
    {
        // Arrange
        var validator = new AccountPayloadValidator();
        var payload = new AccountPayloadDTO
        {
            UserName = "testuser",
            Password = "StrongPass123!",
            Email = "test@example.com",
        };

        // Act
        var result = validator.Validate(payload);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("", "Password123!", "test@example.com", "UserName")]
    [InlineData("testuser", "", "test@example.com", "Password")]
    [InlineData("testuser", "Password123!", "", "Email")]
    [InlineData("testuser", "Password123!", "invalid-email", "Email")]
    [InlineData("testuser", "weak", "test@example.com", "Password")]
    public void AccountPayloadValidation_WithInvalidData_ShouldFail(
        string userName, string password, string email, string expectedErrorProperty)
    {
        // Arrange
        var validator = new AccountPayloadValidator();
        var payload = new AccountPayloadDTO
        {
            UserName = userName,
            Password = password,
            Email = email,
        };

        // Act
        var result = validator.Validate(payload);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == expectedErrorProperty);
    }

    [Fact]
    public void ClaimPayloadValidation_WithValidData_ShouldPass()
    {
        // Arrange
        var validator = new ClaimPayloadValidator();
        var payload = new ClaimPayloadDTO
        {
            Type = "Permission",
            Value = "user:read",
            Description = "Read user permissions",
        };

        // Act
        var result = validator.Validate(payload);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("", "user:read", "Read permissions", "Type")]
    [InlineData("Permission", "", "Read permissions", "Value")]
    public void ClaimPayloadValidation_WithInvalidData_ShouldFail(
        string type, string value, string description, string expectedErrorProperty)
    {
        // Arrange
        var validator = new ClaimPayloadValidator();
        var payload = new ClaimPayloadDTO
        {
            Type = type,
            Value = value,
            Description = description,
        };

        // Act
        var result = validator.Validate(payload);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == expectedErrorProperty);
    }

    // Test classes and DTOs for validation testing
    internal class TestEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }

    internal class TestController : ControllerBase
    {
        // Test controller for validation testing
    }

    internal class AccountPayloadDTO
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }

    internal class ClaimPayloadDTO
    {
        public string Type { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    internal class AccountPayloadValidator : AbstractValidator<AccountPayloadDTO>
    {
        public AccountPayloadValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("Password must be at least 8 characters");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required");
        }
    }

    internal class ClaimPayloadValidator : AbstractValidator<ClaimPayloadDTO>
    {
        public ClaimPayloadValidator()
        {
            RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
            RuleFor(x => x.Value).NotEmpty().WithMessage("Value is required");
        }
    }

    // Static ValidationHelper class for testing
    internal static class ValidationHelper
    {
        public static async Task<IActionResult?> ValidateEntityAsync<T>(T entity, IServiceProvider serviceProvider, ControllerBase controller)
        {
            var validator = serviceProvider.GetService<IValidator<T>>();
            if (validator != null)
            {
                var validationResult = await validator.ValidateAsync(entity, CancellationToken.None);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        controller.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }

                    return controller.BadRequest(controller.ModelState);
                }
            }

            return null;
        }
    }
}
