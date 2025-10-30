using Authentication.Login.DTO;
using FluentAssertions;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class AccountPayLoadDTOTests
    {
        [Fact]
        public void AccountPayLoadDTO_WhenCreated_ShouldHaveDefaultValues()
        {
            // Act
            var dto = new AccountPayLoadDTO();

            // Assert
            dto.UserName.Should().Be(string.Empty);
            dto.Password.Should().Be(string.Empty);
        }

        [Fact]
        public void AccountPayLoadDTO_SetUserName_ShouldUpdateProperty()
        {
            // Arrange
            var dto = new AccountPayLoadDTO();
            var expectedUserName = "testuser";

            // Act
            dto.UserName = expectedUserName;

            // Assert
            dto.UserName.Should().Be(expectedUserName);
        }

        [Fact]
        public void AccountPayLoadDTO_SetPassword_ShouldUpdateProperty()
        {
            // Arrange
            var dto = new AccountPayLoadDTO();
            var expectedPassword = "testpassword";

            // Act
            dto.Password = expectedPassword;

            // Assert
            dto.Password.Should().Be(expectedPassword);
        }

        [Fact]
        public void AccountPayLoadDTO_WithValidData_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var expectedUserName = "validuser";
            var expectedPassword = "validpassword123";

            // Act
            var dto = new AccountPayLoadDTO
            {
                UserName = expectedUserName,
                Password = expectedPassword,
            };

            // Assert
            dto.UserName.Should().Be(expectedUserName);
            dto.Password.Should().Be(expectedPassword);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("user", "")]
        [InlineData("", "pass")]
        [InlineData("user", "pass")]
        public void AccountPayLoadDTO_WithVariousValues_ShouldAcceptAllInputs(string userName, string password)
        {
            // Act
            var dto = new AccountPayLoadDTO
            {
                UserName = userName,
                Password = password,
            };

            // Assert
            dto.UserName.Should().Be(userName);
            dto.Password.Should().Be(password);
        }

        [Fact]
        public void AccountPayLoadDTO_WithLongValues_ShouldAcceptValues()
        {
            // Arrange
            var longUserName = new string('u', 100);
            var longPassword = new string('p', 100);

            // Act
            var dto = new AccountPayLoadDTO
            {
                UserName = longUserName,
                Password = longPassword,
            };

            // Assert
            dto.UserName.Should().Be(longUserName);
            dto.Password.Should().Be(longPassword);
            dto.UserName.Length.Should().Be(100);
            dto.Password.Length.Should().Be(100);
        }

        [Fact]
        public void AccountPayLoadDTO_WithSpecialCharacters_ShouldAcceptValues()
        {
            // Arrange
            var specialUserName = "user@domain.com!#$%";
            var specialPassword = "pass@123!#$%&*()";

            // Act
            var dto = new AccountPayLoadDTO
            {
                UserName = specialUserName,
                Password = specialPassword,
            };

            // Assert
            dto.UserName.Should().Be(specialUserName);
            dto.Password.Should().Be(specialPassword);
        }

        [Fact]
        public void AccountPayLoadDTO_UpdateAfterCreation_ShouldReflectChanges()
        {
            // Arrange
            var dto = new AccountPayLoadDTO
            {
                UserName = "initialuser",
                Password = "initialpass",
            };

            // Act
            dto.UserName = "updateduser";
            dto.Password = "updatedpass";

            // Assert
            dto.UserName.Should().Be("updateduser");
            dto.Password.Should().Be("updatedpass");
        }

        [Fact]
        public void AccountPayLoadDTO_TwoInstancesWithSameData_ShouldHaveEqualProperties()
        {
            // Arrange
            var dto1 = new AccountPayLoadDTO { UserName = "testuser", Password = "testpass" };
            var dto2 = new AccountPayLoadDTO { UserName = "testuser", Password = "testpass" };

            // Assert
            dto1.UserName.Should().Be(dto2.UserName);
            dto1.Password.Should().Be(dto2.Password);
        }

        [Fact]
        public void AccountPayLoadDTO_WithUnicodeCharacters_ShouldAcceptValues()
        {
            // Arrange
            var unicodeUserName = "usuário";
            var unicodePassword = "contraseña";

            // Act
            var dto = new AccountPayLoadDTO
            {
                UserName = unicodeUserName,
                Password = unicodePassword,
            };

            // Assert
            dto.UserName.Should().Be(unicodeUserName);
            dto.Password.Should().Be(unicodePassword);
        }
    }
}
