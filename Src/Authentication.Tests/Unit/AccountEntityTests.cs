using Authentication.Login.Domain.Implementation;
using FluentAssertions;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class AccountEntityTests
    {
        [Fact]
        public void Account_WhenCreated_ShouldHaveDefaultValues()
        {
            // Act
            var account = new Account();

            // Assert
            account.UserName.Should().Be(string.Empty);
            account.Password.Should().Be(string.Empty);
            account.Id.Should().Be(0);
        }

        [Fact]
        public void Account_SetUserName_ShouldUpdateUserNameProperty()
        {
            // Arrange
            var account = new Account();
            var expectedUserName = "testuser";

            // Act
            account.UserName = expectedUserName;

            // Assert
            account.UserName.Should().Be(expectedUserName);
        }

        [Fact]
        public void Account_SetPassword_ShouldUpdatePasswordProperty()
        {
            // Arrange
            var account = new Account();
            var expectedPassword = "testpassword";

            // Act
            account.Password = expectedPassword;

            // Assert
            account.Password.Should().Be(expectedPassword);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Account_SetUserNameToNullOrEmpty_ShouldAllowValue(string userName)
        {
            // Arrange
            var account = new Account();

            // Act
            account.UserName = userName ?? string.Empty;

            // Assert
            account.UserName.Should().Be(userName ?? string.Empty);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Account_SetPasswordToNullOrEmpty_ShouldAllowValue(string password)
        {
            // Arrange
            var account = new Account();

            // Act
            account.Password = password ?? string.Empty;

            // Assert
            account.Password.Should().Be(password ?? string.Empty);
        }

        [Fact]
        public void Account_WithValidUserNameAndPassword_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var expectedUserName = "validuser";
            var expectedPassword = "validpassword123";

            // Act
            var account = new Account
            {
                UserName = expectedUserName,
                Password = expectedPassword
            };

            // Assert
            account.UserName.Should().Be(expectedUserName);
            account.Password.Should().Be(expectedPassword);
        }

        [Fact]
        public void Account_WithLongUserName_ShouldAllowValue()
        {
            // Arrange
            var account = new Account();
            var longUserName = new string('a', 100);

            // Act
            account.UserName = longUserName;

            // Assert
            account.UserName.Should().Be(longUserName);
        }

        [Fact]
        public void Account_WithLongPassword_ShouldAllowValue()
        {
            // Arrange
            var account = new Account();
            var longPassword = new string('a', 100);

            // Act
            account.Password = longPassword;

            // Assert
            account.Password.Should().Be(longPassword);
        }

        [Fact]
        public void Account_WithSpecialCharactersInUserName_ShouldAllowValue()
        {
            // Arrange
            var account = new Account();
            var userNameWithSpecialChars = "user@domain.com";

            // Act
            account.UserName = userNameWithSpecialChars;

            // Assert
            account.UserName.Should().Be(userNameWithSpecialChars);
        }

        [Fact]
        public void Account_WithSpecialCharactersInPassword_ShouldAllowValue()
        {
            // Arrange
            var account = new Account();
            var passwordWithSpecialChars = "P@ssw0rd!#$";

            // Act
            account.Password = passwordWithSpecialChars;

            // Assert
            account.Password.Should().Be(passwordWithSpecialChars);
        }

        [Fact]
        public void Account_Implementation_ShouldImplementIAccountInterface()
        {
            // Arrange & Act
            var account = new Account();

            // Assert
            account.Should().BeAssignableTo<Authentication.Login.Domain.Interface.IAccount>();
        }

        [Fact]
        public void Account_TwoInstancesWithSameData_ShouldBeEqual()
        {
            // Arrange
            var account1 = new Account
            {
                UserName = "testuser",
                Password = "testpassword"
            };

            var account2 = new Account
            {
                UserName = "testuser",
                Password = "testpassword"
            };

            // Act & Assert
            account1.UserName.Should().Be(account2.UserName);
            account1.Password.Should().Be(account2.Password);
        }

        [Fact]
        public void Account_UpdateUserNameAfterCreation_ShouldReflectChanges()
        {
            // Arrange
            var account = new Account { UserName = "initialuser" };
            var newUserName = "updateduser";

            // Act
            account.UserName = newUserName;

            // Assert
            account.UserName.Should().Be(newUserName);
        }

        [Fact]
        public void Account_UpdatePasswordAfterCreation_ShouldReflectChanges()
        {
            // Arrange
            var account = new Account { Password = "initialpassword" };
            var newPassword = "updatedpassword";

            // Act
            account.Password = newPassword;

            // Assert
            account.Password.Should().Be(newPassword);
        }
    }
}