using Authentication.Login.DTO;
using Authentication.Login.Util;
using FluentValidation.TestHelper;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class ApplicationClaimPayloadValidatorTests
    {
        private readonly ApplicationClaimPayloadValidator validator;

        public ApplicationClaimPayloadValidatorTests()
        {
            validator = new ApplicationClaimPayloadValidator();
        }

        [Fact]
        public void Model_WhenAllFieldsValid_ShouldHaveNoValidationErrors()
        {
            // Arrange
            var model = new ApplicationClaimPayLoadDTO
            {
                IdApplication = 1,
                IdClaim = 1
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void IdApplication_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ApplicationClaimPayLoadDTO
            {
                IdApplication = 1,
                IdClaim = 1
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IdApplication);
        }

        [Fact]
        public void IdApplication_WhenZero_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ApplicationClaimPayLoadDTO
            {
                IdApplication = 0,
                IdClaim = 1
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdApplication)
                .WithErrorMessage("Application ID must be greater than 0.");
        }

        [Fact]
        public void IdApplication_WhenNegative_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ApplicationClaimPayLoadDTO
            {
                IdApplication = -1,
                IdClaim = 1
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdApplication)
                .WithErrorMessage("Application ID must be greater than 0.");
        }

        [Fact]
        public void IdClaim_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ApplicationClaimPayLoadDTO
            {
                IdApplication = 1,
                IdClaim = 1
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IdClaim);
        }

        [Fact]
        public void IdClaim_WhenZero_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ApplicationClaimPayLoadDTO
            {
                IdApplication = 1,
                IdClaim = 0
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdClaim)
                .WithErrorMessage("Claim ID must be greater than 0.");
        }

        [Fact]
        public void IdClaim_WhenNegative_ShouldHaveValidationError()
        {
            // Arrange
            var model = new ApplicationClaimPayLoadDTO
            {
                IdApplication = 1,
                IdClaim = -1
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IdClaim)
                .WithErrorMessage("Claim ID must be greater than 0.");
        }

        [Fact]
        public void UpdatedBy_WhenNull_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new ApplicationClaimPayLoadDTO
            {
                IdApplication = 1,
                IdClaim = 1,
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
            var model = new ApplicationClaimPayLoadDTO
            {
                IdApplication = 1,
                IdClaim = 1,
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
