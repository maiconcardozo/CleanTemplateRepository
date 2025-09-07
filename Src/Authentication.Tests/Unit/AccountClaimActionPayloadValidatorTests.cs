using Authentication.Login.DTO;
using Authentication.Login.Util;
using FluentValidation.TestHelper;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class AccountClaimActionPayloadValidatorTests
    {
        private readonly AccountClaimActionPayloadValidator _validator;

        public AccountClaimActionPayloadValidatorTests()
        {
            _validator = new AccountClaimActionPayloadValidator();
        }

        [Fact]
        public void Model_WhenBothFieldsValid_ShouldHaveNoValidationErrors()
        {
            // Arrange
            var model = new AccountClaimActionPayLoadDTO { IdAccount = 1, IdClaimAction = 1, CreatedBy = "testuser" };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Model_WhenBothFieldsInvalid_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var model = new AccountClaimActionPayLoadDTO { IdAccount = 0, IdClaimAction = 0 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdAccount);
            result.ShouldHaveValidationErrorFor(x => x.IdClaimAction);
        }

        [Fact]
        public void IdAccount_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new AccountClaimActionPayLoadDTO { IdAccount = 1, IdClaimAction = 1 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IdAccount);
        }

        [Fact]
        public void IdAccount_WhenZero_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountClaimActionPayLoadDTO { IdAccount = 0, IdClaimAction = 1 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdAccount)
                .WithErrorMessage("Account ID must be a positive number.");
        }

        [Fact]
        public void IdAccount_WhenNegative_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountClaimActionPayLoadDTO { IdAccount = -1, IdClaimAction = 1 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdAccount)
                .WithErrorMessage("Account ID must be a positive number.");
        }

        [Fact]
        public void IdClaimAction_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new AccountClaimActionPayLoadDTO { IdAccount = 1, IdClaimAction = 1 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IdClaimAction);
        }

        [Fact]
        public void IdClaimAction_WhenZero_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountClaimActionPayLoadDTO { IdAccount = 1, IdClaimAction = 0 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdClaimAction)
                .WithErrorMessage("Claim Action ID must be a positive number.");
        }

        [Fact]
        public void IdClaimAction_WhenNegative_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountClaimActionPayLoadDTO { IdAccount = 1, IdClaimAction = -1 };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdClaimAction)
                .WithErrorMessage("Claim Action ID must be a positive number.");
        }

        [Fact]
        public void Model_WithLargeValidValues_ShouldValidateCorrectly()
        {
            // Arrange
            var model = new AccountClaimActionPayLoadDTO { IdAccount = 999999, IdClaimAction = 999999, CreatedBy = "testuser" };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
