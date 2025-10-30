using Authentication.Login.DTO;
using Authentication.Login.Util;
using FluentValidation.TestHelper;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class ApplicationPayloadValidatorTests
    {
        private readonly ApplicationPayloadValidator validator;

        public ApplicationPayloadValidatorTests()
        {
            validator = new ApplicationPayloadValidator();
        }

        [Fact]
        public void Model_WhenAllFieldsValid_ShouldHaveNoValidationErrors()
        {
            // Arrange
            var model = new ApplicationPayLoadDTO 
            { 
                Name = "ValidApplicationName", 
                Description = "Valid description for application"
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Name_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ApplicationPayLoadDTO 
            { 
                Name = "ValidApplicationName",
                Description = "Valid description"
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_WhenEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ApplicationPayLoadDTO 
            { 
                Name = string.Empty,
                Description = "Valid description"
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Application name is required.");
        }

        [Fact]
        public void Name_WhenNull_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ApplicationPayLoadDTO 
            { 
                Name = null!,
                Description = "Valid description"
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_WhenWhitespace_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ApplicationPayLoadDTO 
            { 
                Name = "   ",
                Description = "Valid description"
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Application name cannot contain spaces or be null/empty.");
        }

        [Fact]
        public void Name_WhenContainsSpaces_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ApplicationPayLoadDTO 
            { 
                Name = "Invalid Name",
                Description = "Valid description"
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Application name cannot contain spaces or be null/empty.");
        }

        [Fact]
        public void Name_WhenTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var longName = new string('a', 101); // 101 characters
            var model = new ApplicationPayLoadDTO 
            { 
                Name = longName,
                Description = "Valid description"
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Application name must be at most 100 characters.");
        }

        [Fact]
        public void Name_WhenExactlyMaximumLength_ShouldNotHaveValidationError()
        {
            // Arrange
            var maxLengthName = new string('a', 100); // 100 characters
            var model = new ApplicationPayLoadDTO 
            { 
                Name = maxLengthName,
                Description = "Valid description"
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_WhenMinimumLength_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ApplicationPayLoadDTO 
            { 
                Name = "a", // 1 character
                Description = "Valid description"
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_WithValidSpecialCharacters_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ApplicationPayLoadDTO 
            { 
                Name = "Application_Name-123",
                Description = "Valid description"
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Description_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ApplicationPayLoadDTO 
            { 
                Name = "ValidApp",
                Description = "This is a valid description for the application"
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Description_WhenEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ApplicationPayLoadDTO 
            { 
                Name = "ValidApp",
                Description = string.Empty
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("Application description is required.");
        }

        [Fact]
        public void Description_WhenTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var longDescription = new string('a', 501); // 501 characters
            var model = new ApplicationPayLoadDTO 
            { 
                Name = "ValidApp",
                Description = longDescription
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("Application description must be at most 500 characters.");
        }

        [Fact]
        public void Description_WhenExactlyMaximumLength_ShouldNotHaveValidationError()
        {
            // Arrange
            var maxLengthDescription = new string('a', 500); // 500 characters
            var model = new ApplicationPayLoadDTO 
            { 
                Name = "ValidApp",
                Description = maxLengthDescription
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void UpdatedBy_WhenNull_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ApplicationPayLoadDTO 
            { 
                Name = "ValidApp",
                Description = "Valid description",
                UpdatedBy = null
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UpdatedBy);
        }

        [Fact]
        public void UpdatedBy_WhenTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var longUpdatedBy = new string('a', 101); // 101 characters
            var model = new ApplicationPayLoadDTO 
            { 
                Name = "ValidApp",
                Description = "Valid description",
                UpdatedBy = longUpdatedBy
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UpdatedBy)
                .WithErrorMessage("UpdatedBy must be at most 100 characters.");
        }
    }
}
