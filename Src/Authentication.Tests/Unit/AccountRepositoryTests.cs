using Authentication.Login.Domain.Implementation;
using Authentication.Login.Infrastructure.Data;
using Authentication.Login.Repository.Implementation;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class AccountRepositoryTests : IDisposable
    {
        private readonly LoginContext context;
        private readonly AccountRepository repository;

        public AccountRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new LoginContext(options);
            repository = new AccountRepository(context);
        }

        public void Dispose()
        {
            context?.Dispose();
        }

        [Fact]
        public void GetByUserName_WithExistingUserName_ShouldReturnAccount()
        {
            // Arrange
            var account = new Account { UserName = "testuser", Password = "hashedpassword" };
            context.Set<Account>().Add(account);
            context.SaveChanges();

            // Act
            var result = repository.GetByUserName("testuser");

            // Assert
            result.Should().NotBeNull();
            result!.UserName.Should().Be("testuser");
            result.Password.Should().Be("hashedpassword");
        }

        [Fact]
        public void GetByUserName_WithNonExistingUserName_ShouldReturnNull()
        {
            // Act
            var result = repository.GetByUserName("nonexistentuser");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetByUserName_WithEmptyString_ShouldReturnNull()
        {
            // Act
            var result = repository.GetByUserName(string.Empty);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetByUserName_WithCaseSensitiveUserName_ShouldRespectCase()
        {
            // Arrange
            var account = new Account { UserName = "TestUser", Password = "hashedpassword" };
            context.Set<Account>().Add(account);
            context.SaveChanges();

            // Act
            var resultExact = repository.GetByUserName("TestUser");
            var resultLower = repository.GetByUserName("testuser");

            // Assert
            resultExact.Should().NotBeNull();
            resultLower.Should().BeNull(); // Case sensitive
        }

        [Fact]
        public async Task GetByUserNameAsync_WithExistingUserName_ShouldReturnAccount()
        {
            // Arrange
            var account = new Account { UserName = "asyncuser", Password = "hashedpassword" };
            context.Set<Account>().Add(account);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByUserNameAsync("asyncuser");

            // Assert
            result.Should().NotBeNull();
            result!.UserName.Should().Be("asyncuser");
            result.Password.Should().Be("hashedpassword");
        }

        [Fact]
        public async Task GetByUserNameAsync_WithNonExistingUserName_ShouldReturnNull()
        {
            // Act
            var result = await repository.GetByUserNameAsync("nonexistentuser");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetByUserNameList_WithExistingUserNames_ShouldReturnMatchingAccounts()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { UserName = "user1", Password = "pass1" },
                new Account { UserName = "user2", Password = "pass2" },
                new Account { UserName = "user3", Password = "pass3" },
            };
            context.Set<Account>().AddRange(accounts);
            context.SaveChanges();

            var searchUserNames = new List<string> { "user1", "user3" };

            // Act
            var result = repository.GetByUserNameList(searchUserNames).ToList();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(a => a.UserName == "user1");
            result.Should().Contain(a => a.UserName == "user3");
            result.Should().NotContain(a => a.UserName == "user2");
        }

        [Fact]
        public void GetByUserNameList_WithNonExistingUserNames_ShouldReturnEmptyList()
        {
            // Arrange
            var searchUserNames = new List<string> { "nonexistent1", "nonexistent2" };

            // Act
            var result = repository.GetByUserNameList(searchUserNames).ToList();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetByUserNameList_WithEmptyList_ShouldReturnEmptyList()
        {
            // Arrange
            var searchUserNames = new List<string>();

            // Act
            var result = repository.GetByUserNameList(searchUserNames).ToList();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void UpdatePassword_WithExistingUser_ShouldUpdatePassword()
        {
            // Arrange
            var account = new Account { UserName = "updateuser", Password = "oldpassword" };
            context.Set<Account>().Add(account);
            context.SaveChanges();

            var newPassword = "newhashedpassword";

            // Act
            repository.UpdatePassword("updateuser", newPassword);
            context.SaveChanges();

            // Assert
            var updatedAccount = repository.GetByUserName("updateuser");
            updatedAccount.Should().NotBeNull();
            updatedAccount!.Password.Should().Be(newPassword);
        }

        [Fact]
        public void UpdatePassword_WithNonExistingUser_ShouldNotThrowException()
        {
            // Act
            System.Action act = () => repository.UpdatePassword("nonexistentuser", "newpassword");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void UpdateUserName_WithExistingUser_ShouldUpdateUserName()
        {
            // Arrange
            var account = new Account { UserName = "oldusername", Password = "password" };
            context.Set<Account>().Add(account);
            context.SaveChanges();

            var newUserName = "newusername";

            // Act
            repository.UpdateUserName("oldusername", newUserName);
            context.SaveChanges();

            // Assert
            var updatedAccount = repository.GetByUserName(newUserName);
            updatedAccount.Should().NotBeNull();
            updatedAccount!.UserName.Should().Be(newUserName);

            var oldAccount = repository.GetByUserName("oldusername");
            oldAccount.Should().BeNull();
        }

        [Fact]
        public void UpdateUserName_WithNonExistingUser_ShouldNotThrowException()
        {
            // Act
            System.Action act = () => repository.UpdateUserName("nonexistentuser", "newusername");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void DeleteByUserName_WithExistingUser_ShouldRemoveAccount()
        {
            // Arrange
            var account = new Account { UserName = "deleteuser", Password = "password" };
            context.Set<Account>().Add(account);
            context.SaveChanges();

            // Act
            repository.DeleteByUserName("deleteuser");
            context.SaveChanges();

            // Assert
            var deletedAccount = repository.GetByUserName("deleteuser");
            deletedAccount.Should().NotBeNull();
            deletedAccount!.IsActive.Should().BeFalse();
        }

        [Fact]
        public void DeleteByUserName_WithNonExistingUser_ShouldNotThrowException()
        {
            // Act
            System.Action act = () => repository.DeleteByUserName("nonexistentuser");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void DeleteByUserNameList_WithExistingUsers_ShouldRemoveMatchingAccounts()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { UserName = "delete1", Password = "pass1" },
                new Account { UserName = "delete2", Password = "pass2" },
                new Account { UserName = "keep1", Password = "pass3" },
            };
            context.Set<Account>().AddRange(accounts);
            context.SaveChanges();

            var usersToDelete = new List<string> { "delete1", "delete2" };

            // Act
            repository.DeleteByUserNameList(usersToDelete);
            context.SaveChanges();

            // Assert
            var remainingAccounts = context.Set<Account>().Where(a => a.IsActive).ToList();
            remainingAccounts.Should().HaveCount(1);
            remainingAccounts.Should().Contain(a => a.UserName == "keep1");
            remainingAccounts.Should().NotContain(a => a.UserName == "delete1");
            remainingAccounts.Should().NotContain(a => a.UserName == "delete2");
        }

        [Fact]
        public void DeleteByUserNameList_WithNonExistingUsers_ShouldNotThrowException()
        {
            // Arrange
            var usersToDelete = new List<string> { "nonexistent1", "nonexistent2" };

            // Act
            System.Action act = () => repository.DeleteByUserNameList(usersToDelete);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void DeleteByUserNameList_WithEmptyList_ShouldNotThrowException()
        {
            // Arrange
            var usersToDelete = new List<string>();

            // Act
            System.Action act = () => repository.DeleteByUserNameList(usersToDelete);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void DeleteByUserNameAndPassword_WithExistingAccount_ShouldRemoveAccount()
        {
            // Arrange
            var account = new Account { UserName = "deleteaccountuser", Password = "password" };
            context.Set<Account>().Add(account);
            context.SaveChanges();

            var accountToDelete = new Account { UserName = "deleteaccountuser", Password = "password" };

            // Act
            repository.DeleteByUserNameAndPassword(accountToDelete);
            context.SaveChanges();

            // Assert
            var deletedAccount = repository.GetByUserName("deleteaccountuser");
            deletedAccount.Should().NotBeNull();
            deletedAccount!.IsActive.Should().BeFalse();
        }

        [Fact]
        public void DeleteByUserNameAndPassword_WithNonExistingAccount_ShouldNotThrowException()
        {
            // Arrange
            var accountToDelete = new Account { UserName = "nonexistentuser", Password = "password" };

            // Act
            System.Action act = () => repository.DeleteByUserNameAndPassword(accountToDelete);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void GetByUserNameAndPasswordList_WithExistingUserNames_ShouldReturnMatchingAccounts()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { UserName = "authuser1", Password = "pass1" },
                new Account { UserName = "authuser2", Password = "pass2" },
                new Account { UserName = "authuser3", Password = "pass3" },
            };
            context.Set<Account>().AddRange(accounts);
            context.SaveChanges();

            var searchUserNames = new List<string> { "authuser1", "authuser3" };

            // Act
            var result = repository.GetByUserNameAndPasswordList(searchUserNames).ToList();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(a => a.UserName == "authuser1");
            result.Should().Contain(a => a.UserName == "authuser3");
            result.Should().NotContain(a => a.UserName == "authuser2");
        }

        [Fact]
        public void Add_WithNewAccount_ShouldAddToDatabase()
        {
            // Arrange
            var account = new Account { UserName = "newuser", Password = "newpassword" };

            // Act
            repository.Add(account);
            context.SaveChanges();

            // Assert
            var addedAccount = repository.GetByUserName("newuser");
            addedAccount.Should().NotBeNull();
            addedAccount!.UserName.Should().Be("newuser");
            addedAccount.Password.Should().Be("newpassword");
        }

        [Fact]
        public void Update_WithExistingAccount_ShouldUpdateInDatabase()
        {
            // Arrange
            var account = new Account { UserName = "updateuser", Password = "oldpassword" };
            context.Set<Account>().Add(account);
            context.SaveChanges();

            // Act
            account.Password = "updatedpassword";
            repository.Update(account);
            context.SaveChanges();

            // Assert
            var updatedAccount = repository.GetByUserName("updateuser");
            updatedAccount.Should().NotBeNull();
            updatedAccount!.Password.Should().Be("updatedpassword");
        }

        [Fact]
        public void GetById_WithExistingId_ShouldReturnAccount()
        {
            // Arrange
            var account = new Account { UserName = "getbyiduser", Password = "password" };
            context.Set<Account>().Add(account);
            context.SaveChanges();

            var accountId = account.Id;

            // Act
            var result = repository.GetById(accountId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(accountId);
            result.UserName.Should().Be("getbyiduser");
        }

        [Fact]
        public void GetAll_WithMultipleAccounts_ShouldReturnAllAccounts()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { UserName = "getall1", Password = "pass1" },
                new Account { UserName = "getall2", Password = "pass2" },
                new Account { UserName = "getall3", Password = "pass3" },
            };
            context.Set<Account>().AddRange(accounts);
            context.SaveChanges();

            // Act
            var result = repository.GetAll().ToList();

            // Assert
            result.Should().HaveCount(3);
            result.Should().Contain(a => a.UserName == "getall1");
            result.Should().Contain(a => a.UserName == "getall2");
            result.Should().Contain(a => a.UserName == "getall3");
        }

        [Fact]
        public void GetByUserName_WithVeryLongUserName_ShouldHandleCorrectly()
        {
            // Arrange
            var longUserName = new string('a', 1000);
            var account = new Account { UserName = longUserName, Password = "password" };
            context.Set<Account>().Add(account);
            context.SaveChanges();

            // Act
            var result = repository.GetByUserName(longUserName);

            // Assert
            result.Should().NotBeNull();
            result!.UserName.Should().Be(longUserName);
        }

        [Fact]
        public void Operations_WithSpecialCharactersInUserName_ShouldWork()
        {
            // Arrange
            var specialUserName = "user@domain.com";
            var account = new Account { UserName = specialUserName, Password = "password" };

            // Act & Assert - Add
            repository.Add(account);
            context.SaveChanges();

            var retrievedAccount = repository.GetByUserName(specialUserName);
            retrievedAccount.Should().NotBeNull();
            retrievedAccount!.UserName.Should().Be(specialUserName);

            // Update
            repository.UpdatePassword(specialUserName, "newpassword");
            context.SaveChanges();

            var updatedAccount = repository.GetByUserName(specialUserName);
            updatedAccount!.Password.Should().Be("newpassword");

            // Delete
            repository.DeleteByUserName(specialUserName);
            context.SaveChanges();

            var deletedAccount = repository.GetByUserName(specialUserName);
            deletedAccount.Should().NotBeNull();
            deletedAccount!.IsActive.Should().BeFalse();
        }

        [Fact]
        public void ConcurrentOperations_ShouldHandleCorrectly()
        {
            // Arrange
            var account1 = new Account { UserName = "concurrent1", Password = "pass1" };
            var account2 = new Account { UserName = "concurrent2", Password = "pass2" };

            // Act
            repository.Add(account1);
            repository.Add(account2);
            context.SaveChanges();

            // Assert
            var allAccounts = repository.GetAll().ToList();
            allAccounts.Should().HaveCount(2);
            allAccounts.Should().Contain(a => a.UserName == "concurrent1");
            allAccounts.Should().Contain(a => a.UserName == "concurrent2");
        }
    }
}
