using Authentication.Login.DTO;
using Authentication.Login.Enum;
using Authentication.Login.Util;
using FluentValidation.TestHelper;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class ClaimPayloadValidatorTests
    {
        private readonly ClaimPayloadValidator validator;

        public ClaimPayloadValidatorTests()
        {
            validator = new ClaimPayloadValidator();
        }

        [Fact]
        public void Model_WhenAllFieldsValid_ShouldHaveNoValidationErrors()
        {
            // Arrange
            var model = new ClaimPayLoadDTO
            {
                Type = ClaimType.Permission,
                Value = "ValidValue",
                Description = "Valid description"
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Type_WhenValidEnum_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ClaimPayLoadDTO
            {
                Type = ClaimType.Role,
                Value = "ValidValue",
                Description = "Valid description",
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Type);
        }

        [Fact]
        public void Value_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ClaimPayLoadDTO
            {
                Type = ClaimType.Permission,
                Value = "ValidValue",
                Description = "Valid description",
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Value);
        }

        [Fact]
        public void Value_WhenEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ClaimPayLoadDTO
            {
                Type = ClaimType.Permission,
                Value = string.Empty,
                Description = "Valid description",
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Value)
                .WithErrorMessage("Claim value is required.");
        }

        [Fact]
        public void Value_WhenNull_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ClaimPayLoadDTO
            {
                Type = ClaimType.Permission,
                Value = null!,
                Description = "Valid description",
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Value);
        }

        [Fact]
        public void Value_WhenTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var longValue = new string('a', 201); // 201 characters
            var model = new ClaimPayLoadDTO
            {
                Type = ClaimType.Permission,
                Value = longValue,
                Description = "Valid description",
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Value)
                .WithErrorMessage("Claim value must be at most 200 characters.");
        }

        [Fact]
        public void Value_WhenExactlyMaximumLength_ShouldNotHaveValidationError()
        {
            // Arrange
            var maxLengthValue = new string('a', 200); // 200 characters
            var model = new ClaimPayLoadDTO
            {
                Type = ClaimType.Permission,
                Value = maxLengthValue,
                Description = "Valid description",
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Value);
        }

        [Fact]
        public void Value_WhenMinimumLength_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ClaimPayLoadDTO
            {
                Type = ClaimType.Permission,
                Value = "a", // 1 character
                Description = "Valid description",
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Value);
        }

        [Fact]
        public void Description_WhenEmpty_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ClaimPayLoadDTO
            {
                Type = ClaimType.Permission,
                Value = "ValidValue",
                Description = string.Empty,
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Description_WhenNull_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ClaimPayLoadDTO
            {
                Type = ClaimType.Permission,
                Value = "ValidValue",
                Description = null!,
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Description_WhenTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var longDescription = new string('a', 501); // 501 characters
            var model = new ClaimPayLoadDTO
            {
                Type = ClaimType.Permission,
                Value = "ValidValue",
                Description = longDescription,
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("Claim description must be at most 500 characters.");
        }

        [Fact]
        public void Description_WhenExactlyMaximumLength_ShouldNotHaveValidationError()
        {
            // Arrange
            var maxLengthDescription = new string('a', 500); // 500 characters
            var model = new ClaimPayLoadDTO
            {
                Type = ClaimType.Permission,
                Value = "ValidValue",
                Description = maxLengthDescription,
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Model_WithDifferentEnumValues_ShouldValidateCorrectly()
        {
            // Test Permission
            var permissionModel = new ClaimPayLoadDTO
            {
                Type = ClaimType.Permission,
                Value = "CanRead",
                Description = "Read permission"
            };
            var permissionResult = validator.TestValidate(permissionModel);
            permissionResult.ShouldNotHaveAnyValidationErrors();

            // Test Role
            var roleModel = new ClaimPayLoadDTO
            {
                Type = ClaimType.Role,
                Value = "Administrator",
                Description = "Admin role"
            };
            var roleResult = validator.TestValidate(roleModel);
            roleResult.ShouldNotHaveAnyValidationErrors();

            // Test Custom
            var customModel = new ClaimPayLoadDTO
            {
                Type = ClaimType.Custom,
                Value = "Department",
                Description = "Custom department claim"
            };
            var customResult = validator.TestValidate(customModel);
            customResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}
