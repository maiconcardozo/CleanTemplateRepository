using Authentication.Login.DTO;
using Authentication.Login.Util;
using FluentValidation.TestHelper;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class ClaimActionPayloadValidatorTests
    {
        private readonly ClaimActionPayloadValidator _validator;

        public ClaimActionPayloadValidatorTests()
        {
            _validator = new ClaimActionPayloadValidator();
        }

        [Fact]
        public void Model_WhenBothFieldsValid_ShouldHaveNoValidationErrors()
        {
            // Arrange
            var model = new ClaimActionPayLoadDTO { IdClaim = 1, IdAction = 1, CreatedBy = "testuser" };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Model_WhenBothFieldsInvalid_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var model = new ClaimActionPayLoadDTO { IdClaim = 0, IdAction = 0 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdClaim);
            result.ShouldHaveValidationErrorFor(x => x.IdAction);
        }

        [Fact]
        public void IdClaim_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ClaimActionPayLoadDTO { IdClaim = 1, IdAction = 1 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IdClaim);
        }

        [Fact]
        public void IdClaim_WhenZero_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ClaimActionPayLoadDTO { IdClaim = 0, IdAction = 1 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdClaim)
                .WithErrorMessage("Claim ID must be a positive number.");
        }

        [Fact]
        public void IdClaim_WhenNegative_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ClaimActionPayLoadDTO { IdClaim = -1, IdAction = 1 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdClaim)
                .WithErrorMessage("Claim ID must be a positive number.");
        }

        [Fact]
        public void IdAction_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ClaimActionPayLoadDTO { IdClaim = 1, IdAction = 1 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IdAction);
        }

        [Fact]
        public void IdAction_WhenZero_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ClaimActionPayLoadDTO { IdClaim = 1, IdAction = 0 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdAction)
                .WithErrorMessage("Action ID must be a positive number.");
        }

        [Fact]
        public void IdAction_WhenNegative_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ClaimActionPayLoadDTO { IdClaim = 1, IdAction = -1 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdAction)
                .WithErrorMessage("Action ID must be a positive number.");
        }

        [Fact]
        public void Model_WithLargeValidValues_ShouldValidateCorrectly()
        {
            // Arrange
            var model = new ClaimActionPayLoadDTO { IdClaim = 999999, IdAction = 999999, CreatedBy = "testuser" };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
