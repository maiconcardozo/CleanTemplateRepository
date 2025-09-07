using Authentication.Login.Domain.Implementation;
using Authentication.Login.Repository.Implementation;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class AccountRepositoryTests : IDisposable
    {
        private readonly DbContext _context;
        private readonly AccountRepository _repository;

        public AccountRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);
            _repository = new AccountRepository(_context);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        #region GetByUserName Tests

        [Fact]
        public void GetByUserName_WithExistingUserName_ShouldReturnAccount()
        {
            // Arrange
            var account = new Account { UserName = "testuser", Password = "hashedpassword" };
            _context.Set<Account>().Add(account);
            _context.SaveChanges();

            // Act
            var result = _repository.GetByUserName("testuser");

            // Assert
            result.Should().NotBeNull();
            result!.UserName.Should().Be("testuser");
            result.Password.Should().Be("hashedpassword");
        }

        [Fact]
        public void GetByUserName_WithNonExistingUserName_ShouldReturnNull()
        {
            // Act
            var result = _repository.GetByUserName("nonexistentuser");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetByUserName_WithEmptyString_ShouldReturnNull()
        {
            // Act
            var result = _repository.GetByUserName("");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetByUserName_WithCaseSensitiveUserName_ShouldRespectCase()
        {
            // Arrange
            var account = new Account { UserName = "TestUser", Password = "hashedpassword" };
            _context.Set<Account>().Add(account);
            _context.SaveChanges();

            // Act
            var resultExact = _repository.GetByUserName("TestUser");
            var resultLower = _repository.GetByUserName("testuser");

            // Assert
            resultExact.Should().NotBeNull();
            resultLower.Should().BeNull(); // Case sensitive
        }

        #endregion

        #region GetByUserNameAsync Tests

        [Fact]
        public async Task GetByUserNameAsync_WithExistingUserName_ShouldReturnAccount()
        {
            // Arrange
            var account = new Account { UserName = "asyncuser", Password = "hashedpassword" };
            _context.Set<Account>().Add(account);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUserNameAsync("asyncuser");

            // Assert
            result.Should().NotBeNull();
            result!.UserName.Should().Be("asyncuser");
            result.Password.Should().Be("hashedpassword");
        }

        [Fact]
        public async Task GetByUserNameAsync_WithNonExistingUserName_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByUserNameAsync("nonexistentuser");

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region GetByUserNameList Tests

        [Fact]
        public void GetByUserNameList_WithExistingUserNames_ShouldReturnMatchingAccounts()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { UserName = "user1", Password = "pass1" },
                new Account { UserName = "user2", Password = "pass2" },
                new Account { UserName = "user3", Password = "pass3" }
            };
            _context.Set<Account>().AddRange(accounts);
            _context.SaveChanges();

            var searchUserNames = new List<string> { "user1", "user3" };

            // Act
            var result = _repository.GetByUserNameList(searchUserNames).ToList();

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
            var result = _repository.GetByUserNameList(searchUserNames).ToList();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetByUserNameList_WithEmptyList_ShouldReturnEmptyList()
        {
            // Arrange
            var searchUserNames = new List<string>();

            // Act
            var result = _repository.GetByUserNameList(searchUserNames).ToList();

            // Assert
            result.Should().BeEmpty();
        }

        #endregion

        #region UpdatePassword Tests

        [Fact]
        public void UpdatePassword_WithExistingUser_ShouldUpdatePassword()
        {
            // Arrange
            var account = new Account { UserName = "updateuser", Password = "oldpassword" };
            _context.Set<Account>().Add(account);
            _context.SaveChanges();

            var newPassword = "newhashedpassword";

            // Act
            _repository.UpdatePassword("updateuser", newPassword);
            _context.SaveChanges();

            // Assert
            var updatedAccount = _repository.GetByUserName("updateuser");
            updatedAccount.Should().NotBeNull();
            updatedAccount!.Password.Should().Be(newPassword);
        }

        [Fact]
        public void UpdatePassword_WithNonExistingUser_ShouldNotThrowException()
        {
            // Act
            System.Action act = () => _repository.UpdatePassword("nonexistentuser", "newpassword");

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region UpdateUserName Tests

        [Fact]
        public void UpdateUserName_WithExistingUser_ShouldUpdateUserName()
        {
            // Arrange
            var account = new Account { UserName = "oldusername", Password = "password" };
            _context.Set<Account>().Add(account);
            _context.SaveChanges();

            var newUserName = "newusername";

            // Act
            _repository.UpdateUserName("oldusername", newUserName);
            _context.SaveChanges();

            // Assert
            var updatedAccount = _repository.GetByUserName(newUserName);
            updatedAccount.Should().NotBeNull();
            updatedAccount!.UserName.Should().Be(newUserName);

            var oldAccount = _repository.GetByUserName("oldusername");
            oldAccount.Should().BeNull();
        }

        [Fact]
        public void UpdateUserName_WithNonExistingUser_ShouldNotThrowException()
        {
            // Act
            System.Action act = () => _repository.UpdateUserName("nonexistentuser", "newusername");

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region DeleteByUserName Tests

        [Fact]
        public void DeleteByUserName_WithExistingUser_ShouldRemoveAccount()
        {
            // Arrange
            var account = new Account { UserName = "deleteuser", Password = "password" };
            _context.Set<Account>().Add(account);
            _context.SaveChanges();

            // Act
            _repository.DeleteByUserName("deleteuser");
            _context.SaveChanges();

            // Assert
            var deletedAccount = _repository.GetByUserName("deleteuser");
            deletedAccount.Should().NotBeNull();
            deletedAccount!.IsActive.Should().BeFalse();
        }

        [Fact]
        public void DeleteByUserName_WithNonExistingUser_ShouldNotThrowException()
        {
            // Act
            System.Action act = () => _repository.DeleteByUserName("nonexistentuser");

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region DeleteByUserNameList Tests

        [Fact]
        public void DeleteByUserNameList_WithExistingUsers_ShouldRemoveMatchingAccounts()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { UserName = "delete1", Password = "pass1" },
                new Account { UserName = "delete2", Password = "pass2" },
                new Account { UserName = "keep1", Password = "pass3" }
            };
            _context.Set<Account>().AddRange(accounts);
            _context.SaveChanges();

            var usersToDelete = new List<string> { "delete1", "delete2" };

            // Act
            _repository.DeleteByUserNameList(usersToDelete);
            _context.SaveChanges();

            // Assert
            var remainingAccounts = _context.Set<Account>().Where(a => a.IsActive).ToList();
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
            System.Action act = () => _repository.DeleteByUserNameList(usersToDelete);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void DeleteByUserNameList_WithEmptyList_ShouldNotThrowException()
        {
            // Arrange
            var usersToDelete = new List<string>();

            // Act
            System.Action act = () => _repository.DeleteByUserNameList(usersToDelete);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region DeleteByUserNameAndPassword Tests

        [Fact]
        public void DeleteByUserNameAndPassword_WithExistingAccount_ShouldRemoveAccount()
        {
            // Arrange
            var account = new Account { UserName = "deleteaccountuser", Password = "password" };
            _context.Set<Account>().Add(account);
            _context.SaveChanges();

            var accountToDelete = new Account { UserName = "deleteaccountuser", Password = "password" };

            // Act
            _repository.DeleteByUserNameAndPassword(accountToDelete);
            _context.SaveChanges();

            // Assert
            var deletedAccount = _repository.GetByUserName("deleteaccountuser");
            deletedAccount.Should().NotBeNull();
            deletedAccount!.IsActive.Should().BeFalse();
        }

        [Fact]
        public void DeleteByUserNameAndPassword_WithNonExistingAccount_ShouldNotThrowException()
        {
            // Arrange
            var accountToDelete = new Account { UserName = "nonexistentuser", Password = "password" };

            // Act
            System.Action act = () => _repository.DeleteByUserNameAndPassword(accountToDelete);

            // Assert
            act.Should().NotThrow();
        }

        #endregion

        #region GetByUserNameAndPasswordList Tests

        [Fact]
        public void GetByUserNameAndPasswordList_WithExistingUserNames_ShouldReturnMatchingAccounts()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { UserName = "authuser1", Password = "pass1" },
                new Account { UserName = "authuser2", Password = "pass2" },
                new Account { UserName = "authuser3", Password = "pass3" }
            };
            _context.Set<Account>().AddRange(accounts);
            _context.SaveChanges();

            var searchUserNames = new List<string> { "authuser1", "authuser3" };

            // Act
            var result = _repository.GetByUserNameAndPasswordList(searchUserNames).ToList();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(a => a.UserName == "authuser1");
            result.Should().Contain(a => a.UserName == "authuser3");
            result.Should().NotContain(a => a.UserName == "authuser2");
        }

        #endregion

        #region Base Repository Functionality Tests

        [Fact]
        public void Add_WithNewAccount_ShouldAddToDatabase()
        {
            // Arrange
            var account = new Account { UserName = "newuser", Password = "newpassword" };

            // Act
            _repository.Add(account);
            _context.SaveChanges();

            // Assert
            var addedAccount = _repository.GetByUserName("newuser");
            addedAccount.Should().NotBeNull();
            addedAccount!.UserName.Should().Be("newuser");
            addedAccount.Password.Should().Be("newpassword");
        }

        [Fact]
        public void Update_WithExistingAccount_ShouldUpdateInDatabase()
        {
            // Arrange
            var account = new Account { UserName = "updateuser", Password = "oldpassword" };
            _context.Set<Account>().Add(account);
            _context.SaveChanges();

            // Act
            account.Password = "updatedpassword";
            _repository.Update(account);
            _context.SaveChanges();

            // Assert
            var updatedAccount = _repository.GetByUserName("updateuser");
            updatedAccount.Should().NotBeNull();
            updatedAccount!.Password.Should().Be("updatedpassword");
        }

        [Fact]
        public void GetById_WithExistingId_ShouldReturnAccount()
        {
            // Arrange
            var account = new Account { UserName = "getbyiduser", Password = "password" };
            _context.Set<Account>().Add(account);
            _context.SaveChanges();

            var accountId = account.Id;

            // Act
            var result = _repository.GetById(accountId);

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
                new Account { UserName = "getall3", Password = "pass3" }
            };
            _context.Set<Account>().AddRange(accounts);
            _context.SaveChanges();

            // Act
            var result = _repository.GetAll().ToList();

            // Assert
            result.Should().HaveCount(3);
            result.Should().Contain(a => a.UserName == "getall1");
            result.Should().Contain(a => a.UserName == "getall2");
            result.Should().Contain(a => a.UserName == "getall3");
        }

        #endregion

        #region Edge Cases and Boundary Tests

        [Fact]
        public void GetByUserName_WithVeryLongUserName_ShouldHandleCorrectly()
        {
            // Arrange
            var longUserName = new string('a', 1000);
            var account = new Account { UserName = longUserName, Password = "password" };
            _context.Set<Account>().Add(account);
            _context.SaveChanges();

            // Act
            var result = _repository.GetByUserName(longUserName);

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
            _repository.Add(account);
            _context.SaveChanges();

            var retrievedAccount = _repository.GetByUserName(specialUserName);
            retrievedAccount.Should().NotBeNull();
            retrievedAccount!.UserName.Should().Be(specialUserName);

            // Update
            _repository.UpdatePassword(specialUserName, "newpassword");
            _context.SaveChanges();

            var updatedAccount = _repository.GetByUserName(specialUserName);
            updatedAccount!.Password.Should().Be("newpassword");

            // Delete
            _repository.DeleteByUserName(specialUserName);
            _context.SaveChanges();

            var deletedAccount = _repository.GetByUserName(specialUserName);
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
            _repository.Add(account1);
            _repository.Add(account2);
            _context.SaveChanges();

            // Assert
            var allAccounts = _repository.GetAll().ToList();
            allAccounts.Should().HaveCount(2);
            allAccounts.Should().Contain(a => a.UserName == "concurrent1");
            allAccounts.Should().Contain(a => a.UserName == "concurrent2");
        }

        #endregion
    }

    // Test DbContext for in-memory testing
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired();
                entity.Property(e => e.Password).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
