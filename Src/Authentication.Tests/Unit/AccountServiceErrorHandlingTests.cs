using Authentication.Login.Domain.Implementation;
using Authentication.Login.Exceptions;
using Authentication.Login.Services.Implementation;
using Authentication.Login.UnitOfWork.Interface;
using Authentication.Login.Repository.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class AccountServiceErrorHandlingTests
    {
        private readonly Mock<ILoginUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IAccountClaimActionRepository> _mockAccountClaimActionRepository;
        private readonly AccountService _accountService;

        public AccountServiceErrorHandlingTests()
        {
            _mockUnitOfWork = new Mock<ILoginUnitOfWork>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockAccountClaimActionRepository = new Mock<IAccountClaimActionRepository>();

            _mockUnitOfWork.Setup(x => x.AccountRepository).Returns(_mockAccountRepository.Object);
            _mockUnitOfWork.Setup(x => x.AccountClaimActionRepository).Returns(_mockAccountClaimActionRepository.Object);

            _accountService = new AccountService(_mockUnitOfWork.Object);
        }

        #region Null Parameter Tests

        [Fact]
        public void GetAccountByUserName_WithNullUserName_ShouldNotThrow()
        {
            // Arrange
            _mockAccountRepository.Setup(x => x.GetByUserName(null!)).Returns((Account?)null);

            // Act
            var result = _accountService.GetAccountByUserName(null!);

            // Assert
            result.Should().BeNull();
            _mockAccountRepository.Verify(x => x.GetByUserName(null!), Times.Once);
        }

        [Fact]
        public async Task GetAccountByUserNameAsync_WithNullUserName_ShouldNotThrow()
        {
            // Arrange
            _mockAccountRepository.Setup(x => x.GetByUserNameAsync(null!)).ReturnsAsync((Account?)null);

            // Act
            var result = await _accountService.GetAccountByUserNameAsync(null!);

            // Assert
            result.Should().BeNull();
            _mockAccountRepository.Verify(x => x.GetByUserNameAsync(null!), Times.Once);
        }

        [Fact]
        public void AddAccount_WithNullAccount_ShouldThrow()
        {
            // Act
            System.Action act = () => _accountService.AddAccount(null!);

            // Assert
            act.Should().Throw<Exception>()
                ;
        }

        [Fact]
        public void UpdateAccount_WithNullAccount_ShouldThrow()
        {
            // Act
            System.Action act = () => _accountService.UpdateAccount(null!);

            // Assert
            act.Should().Throw<Exception>()
                ;
        }

      

        #endregion

        #region Repository Exception Tests

        [Fact]
        public void GetAllAccounts_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            _mockAccountRepository.Setup(x => x.GetAll()).Throws<InvalidOperationException>();

            // Act
            System.Action act = () => _accountService.GetAllAccounts();

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetById_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            var accountId = 1;
            _mockAccountRepository.Setup(x => x.GetById(accountId)).Throws<ArgumentException>();

            // Act
            System.Action act = () => _accountService.GetById(accountId);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void DeleteAccountByUserName_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            var userName = "testuser";
            _mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action())
                .Throws<InvalidOperationException>();

            // Act
            System.Action act = () => _accountService.DeleteAccountByUserName(userName);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        #endregion

        #region Transaction Failure Tests

        [Fact]
        public void AddAccounts_WhenTransactionFails_ShouldThrow()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { UserName = "user1", Password = "pass1" }
            };
            
            _mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Throws<InvalidOperationException>();

            // Act
            System.Action act = () => _accountService.AddAccounts(accounts);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void DeleteAccounts_WhenTransactionFails_ShouldThrow()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { Id = 1, UserName = "user1", Password = "pass1" }
            };
            
            _mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Throws<InvalidOperationException>();

            // Act
            System.Action act = () => _accountService.DeleteAccounts(accounts);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        #endregion

        #region Edge Case Values Tests

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        public void GetById_WithEdgeCaseIds_ShouldHandleCorrectly(int accountId)
        {
            // Arrange
            _mockAccountRepository.Setup(x => x.GetById(accountId)).Returns((Account?)null);

            // Act
            var result = _accountService.GetById(accountId);

            // Assert
            result.Should().BeNull();
            _mockAccountRepository.Verify(x => x.GetById(accountId), Times.Once);
        }

        [Fact]
        public void GetAccountsByIds_WithEmptyList_ShouldReturnEmpty()
        {
            // Arrange
            var emptyList = new List<int>();
            _mockAccountRepository.Setup(x => x.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<Account, bool>>>()))
                .Returns(new List<Account>());

            // Act
            var result = _accountService.GetAccountsByIds(emptyList);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void DeleteAccountsByUserNames_WithEmptyList_ShouldNotThrow()
        {
            // Arrange
            var emptyList = new List<string>();
            _mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act
            System.Action act = () => _accountService.DeleteAccountsByUserNames(emptyList);

            // Assert
            act.Should().NotThrow();
            _mockAccountRepository.Verify(x => x.DeleteByUserNameList(emptyList), Times.Once);
        }

        #endregion

        #region Concurrent Access Tests

        [Fact]
        public void AddAccount_WithConcurrentDuplicateUsername_ShouldHandleConflict()
        {
            // Arrange
            var account = new Account { UserName = "concurrentuser", Password = "password" };

            // Simula que o usuário já existe no repositório
            _mockAccountRepository.Setup(x => x.GetByUserName(account.UserName))
                .Returns(new Account { Id = 1, UserName = "concurrentuser", Password = "existing" });

            _mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act
            System.Action act = () => _accountService.AddAccount(account);

            // Assert
            act.Should().Throw<ConflictException>();
        }

        #endregion

        #region Memory and Performance Edge Cases

        [Fact]
        public void GetAccounts_WithComplexPredicate_ShouldHandleCorrectly()
        {
            // Arrange
            var complexPredicate = System.Linq.Expressions.Expression
                .Lambda<System.Func<Account, bool>>(
                    System.Linq.Expressions.Expression.AndAlso(
                        System.Linq.Expressions.Expression.GreaterThan(
                            System.Linq.Expressions.Expression.Property(
                                System.Linq.Expressions.Expression.Parameter(typeof(Account), "a"), 
                                "Id"),
                            System.Linq.Expressions.Expression.Constant(0)),
                        System.Linq.Expressions.Expression.NotEqual(
                            System.Linq.Expressions.Expression.Property(
                                System.Linq.Expressions.Expression.Parameter(typeof(Account), "a"), 
                                "UserName"),
                            System.Linq.Expressions.Expression.Constant(null, typeof(string)))),
                    System.Linq.Expressions.Expression.Parameter(typeof(Account), "a"));

            _mockAccountRepository.Setup(x => x.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<Account, bool>>>()))
                .Returns(new List<Account>());

            // Act
            System.Action act = () => _accountService.GetAccounts(complexPredicate);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void AddAccounts_WithLargeList_ShouldHandleCorrectly()
        {
            // Arrange
            var largeAccountList = new List<Account>();
            for (int i = 0; i < 1000; i++)
            {
                largeAccountList.Add(new Account { UserName = $"user{i}", Password = $"pass{i}" });
            }

            _mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act
            System.Action act = () => _accountService.AddAccounts(largeAccountList);

            // Assert
            act.Should().NotThrow();
            _mockAccountRepository.Verify(x => x.AddRange(largeAccountList), Times.Once);
        }

        #endregion

        #region Business Logic Edge Cases

        [Fact]
        public void UpdateAccountPassword_WithNullValues_ShouldHandleCorrectly()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act & Assert
            System.Action act1 = () => _accountService.UpdateAccountPassword(null!, "newpassword");
            System.Action act2 = () => _accountService.UpdateAccountPassword("username", null!);
            System.Action act3 = () => _accountService.UpdateAccountPassword(null!, null!);

            act1.Should().NotThrow();
            act2.Should().NotThrow();
            act3.Should().NotThrow();
        }

        [Fact]
        public void UpdateAccountUserName_WithNullValues_ShouldHandleCorrectly()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act & Assert
            System.Action act1 = () => _accountService.UpdateAccountUserName(null!, "newusername");
            System.Action act2 = () => _accountService.UpdateAccountUserName("oldusername", null!);
            System.Action act3 = () => _accountService.UpdateAccountUserName(null!, null!);

            act1.Should().NotThrow();
            act2.Should().NotThrow();
            act3.Should().NotThrow();
        }

    

        #endregion
    }
}