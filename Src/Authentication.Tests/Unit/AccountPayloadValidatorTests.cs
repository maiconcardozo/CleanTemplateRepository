using Authentication.Login.DTO;
using Authentication.Login.Resource;
using Authentication.Login.Util;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class AccountPayloadValidatorTests
    {
        private readonly AccountPayloadValidator validator;

        public AccountPayloadValidatorTests()
        {
            validator = new AccountPayloadValidator();
        }

        [Fact]
        public void UserName_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = "validpass123" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }

        [Fact]
        public void UserName_WhenEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = string.Empty, Password = "validpass123" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage(ResourceLogin.UserNameRequired);
        }

        [Fact]
        public void UserName_WhenNull_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = null!, Password = "validpass123" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage(ResourceLogin.UserNameRequired);
        }

        [Fact]
        public void UserName_WhenWhitespace_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "   ", Password = "validpass123" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage(ResourceLogin.UserNameCannotContainSpacesNullEmpty);
        }

        [Fact]
        public void UserName_WhenContainsSpaces_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "user name", Password = "validpass123" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage(ResourceLogin.UserNameCannotContainSpacesNullEmpty);
        }

        [Fact]
        public void UserName_WhenTooShort_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "user", Password = "validpass123" }; // 4 characters, minimum is 6

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage(ResourceLogin.UserNameMustLeast6Characters);
        }

        [Fact]
        public void UserName_WhenExactlyMinimumLength_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "user12", Password = "validpass123" }; // 6 characters

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }

        [Fact]
        public void UserName_WhenTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var longUserName = new string('a', 51); // 51 characters, maximum is 50
            var model = new AccountPayLoadDTO { UserName = longUserName, Password = "validpass123" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage(ResourceLogin.UserNameMustMost50Characters);
        }

        [Fact]
        public void UserName_WhenExactlyMaximumLength_ShouldNotHaveValidationError()
        {
            // Arrange
            var maxLengthUserName = new string('a', 50); // 50 characters
            var model = new AccountPayLoadDTO { UserName = maxLengthUserName, Password = "validpass123" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }

        [Theory]
        [InlineData("user@domain.com")]
        [InlineData("user_name")]
        [InlineData("user-name")]
        [InlineData("user123")]
        [InlineData("123user")]
        [InlineData("UPPERCASE")]
        [InlineData("MixedCase")]
        public void UserName_WithValidSpecialCharacters_ShouldNotHaveValidationError(string userName)
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = userName, Password = "validpass123" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }

        [Fact]
        public void Password_WhenValid_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = "validpass123" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Password_WhenEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = string.Empty };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage(ResourceLogin.PasswordRequired);
        }

        [Fact]
        public void Password_WhenNull_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = null! };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage(ResourceLogin.PasswordRequired);
        }

        [Fact]
        public void Password_WhenWhitespace_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = "   " };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage(ResourceLogin.PasswordCannotContainSpacesNullEmpty);
        }

        [Fact]
        public void Password_WhenContainsSpaces_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = "pass word" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage(ResourceLogin.PasswordCannotContainSpacesNullEmpty);
        }

        [Fact]
        public void Password_WhenTooShort_ShouldHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = "pass" }; // 4 characters, minimum is 6

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage(ResourceLogin.PasswordMustLeast6Characters);
        }

        [Fact]
        public void Password_WhenExactlyMinimumLength_ShouldNotHaveValidationError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = "pass12" }; // 6 characters

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Password_WhenTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var longPassword = new string('a', 51); // 51 characters, maximum is 50
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = longPassword };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage(ResourceLogin.PasswordMustMost50Characters);
        }

        [Fact]
        public void Password_WhenExactlyMaximumLength_ShouldNotHaveValidationError()
        {
            // Arrange
            var maxLengthPassword = new string('a', 50); // 50 characters
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = maxLengthPassword };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Theory]
        [InlineData("password@123")]
        [InlineData("password#123")]
        [InlineData("password$123")]
        [InlineData("password%123")]
        [InlineData("password&123")]
        [InlineData("password*123")]
        [InlineData("password!123")]
        [InlineData("password_123")]
        [InlineData("password-123")]
        [InlineData("password+123")]
        [InlineData("password=123")]
        [InlineData("UPPERCASEPASS")]
        [InlineData("MixedCasePass")]
        [InlineData("123456789")]
        public void Password_WithValidSpecialCharacters_ShouldNotHaveValidationError(string password)
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = password };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Model_WhenBothFieldsValid_ShouldHaveNoValidationErrors()
        {
            // Arrange
            var model = new AccountPayLoadDTO
            {
                UserName = "validuser123",
                Password = "validpass123",
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Model_WhenBothFieldsInvalid_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = string.Empty, Password = string.Empty };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Model_WhenUserNameInvalidAndPasswordValid_ShouldHaveUserNameError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "usr", Password = "validpass123" }; // UserName too short

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Model_WhenUserNameValidAndPasswordInvalid_ShouldHavePasswordError()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = "pwd" }; // Password too short

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void UserName_WithOnlyNumbers_ShouldBeValid()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "123456", Password = "validpass123" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }

        [Fact]
        public void Password_WithOnlyNumbers_ShouldBeValid()
        {
            // Arrange
            var model = new AccountPayLoadDTO { UserName = "validuser", Password = "123456" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void UserName_WithUnicodeCharacters_ShouldBeValid()
        {
            // Arrange - Testing with accented characters
            var model = new AccountPayLoadDTO { UserName = "usuário", Password = "validpass123" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }

        [Fact]
        public void Model_WithBoundaryLengthValues_ShouldValidateCorrectly()
        {
            // Arrange - Both at minimum length (6 characters)
            var model = new AccountPayLoadDTO { UserName = "user12", Password = "pass12" };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Model_WithMaximumLengthValues_ShouldValidateCorrectly()
        {
            // Arrange - Both at maximum length (50 characters)
            var userName = new string('u', 50);
            var password = new string('p', 50);
            var model = new AccountPayLoadDTO { UserName = userName, Password = password };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }


    }
}
