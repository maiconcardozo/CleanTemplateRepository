using Authentication.Login.DTO;
using Authentication.Login.Util;
using FluentValidation.TestHelper;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class ActionPayloadValidatorTests
    {
        private readonly ActionPayloadValidator validator;

        public ActionPayloadValidatorTests()
        {
            validator = new ActionPayloadValidator();
        }

        [Fact]
        public void Model_WhenNameValid_ShouldHaveNoValidationErrors()
        {
            // Arrange
            var model = new ActionPayLoadDTO { Name = "ValidActionName" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Name_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ActionPayLoadDTO { Name = "ValidActionName" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_WhenEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ActionPayLoadDTO { Name = string.Empty };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Action name is required.");
        }

        [Fact]
        public void Name_WhenNull_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ActionPayLoadDTO { Name = null! };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_WhenWhitespace_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ActionPayLoadDTO { Name = "   " };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Action name cannot contain spaces or be null/empty.");
        }

        [Fact]
        public void Name_WhenContainsSpaces_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ActionPayLoadDTO { Name = "Invalid Name" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Action name cannot contain spaces or be null/empty.");
        }

        [Fact]
        public void Name_WhenTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var longName = new string('a', 101); // 101 characters
            var model = new ActionPayLoadDTO { Name = longName };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Action name must be at most 100 characters.");
        }

        [Fact]
        public void Name_WhenExactlyMaximumLength_ShouldNotHaveValidationError()
        {
            // Arrange
            var maxLengthName = new string('a', 100); // 100 characters
            var model = new ActionPayLoadDTO { Name = maxLengthName };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_WhenMinimumLength_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ActionPayLoadDTO { Name = "a" }; // 1 character

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_WithValidSpecialCharacters_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ActionPayLoadDTO { Name = "Action_Name-123" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }
    }
}
