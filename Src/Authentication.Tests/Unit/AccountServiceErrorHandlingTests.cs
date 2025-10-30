using Authentication.Login.Domain.Implementation;
using Authentication.Login.Exceptions;
using Authentication.Login.Repository.Interface;
using Authentication.Login.Services.Implementation;
using Authentication.Login.UnitOfWork.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace Authentication.Tests.Unit
{
    public class AccountServiceErrorHandlingTests
    {
        private readonly Mock<ILoginUnitOfWork> mockUnitOfWork;
        private readonly Mock<IAccountRepository> mockAccountRepository;
        private readonly Mock<IAccountClaimActionRepository> mockAccountClaimActionRepository;
        private readonly AccountService accountService;

        public AccountServiceErrorHandlingTests()
        {
            mockUnitOfWork = new Mock<ILoginUnitOfWork>();
            mockAccountRepository = new Mock<IAccountRepository>();
            mockAccountClaimActionRepository = new Mock<IAccountClaimActionRepository>();

            mockUnitOfWork.Setup(x => x.AccountRepository).Returns(mockAccountRepository.Object);
            mockUnitOfWork.Setup(x => x.AccountClaimActionRepository).Returns(mockAccountClaimActionRepository.Object);

            accountService = new AccountService(mockUnitOfWork.Object);
        }

        [Fact]
        public void GetAccountByUserName_WithNullUserName_ShouldNotThrow()
        {
            // Arrange
            mockAccountRepository.Setup(x => x.GetByUserName(null!)).Returns((Account?)null);

            // Act
            var result = accountService.GetAccountByUserName(null!);

            // Assert
            result.Should().BeNull();
            mockAccountRepository.Verify(x => x.GetByUserName(null!), Times.Once);
        }

        [Fact]
        public async Task GetAccountByUserNameAsync_WithNullUserName_ShouldNotThrow()
        {
            // Arrange
            mockAccountRepository.Setup(x => x.GetByUserNameAsync(null!)).ReturnsAsync((Account?)null);

            // Act
            var result = await accountService.GetAccountByUserNameAsync(null!);

            // Assert
            result.Should().BeNull();
            mockAccountRepository.Verify(x => x.GetByUserNameAsync(null!), Times.Once);
        }

        [Fact]
        public void AddAccount_WithNullAccount_ShouldThrow()
        {
            // Act
            System.Action act = () => accountService.AddAccount(null!);

            // Assert
            act.Should().Throw<Exception>()
                ;
        }

        [Fact]
        public void UpdateAccount_WithNullAccount_ShouldThrow()
        {
            // Act
            System.Action act = () => accountService.UpdateAccount(null!);

            // Assert
            act.Should().Throw<Exception>()
                ;
        }

        [Fact]
        public void GetAllAccounts_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            mockAccountRepository.Setup(x => x.GetAll()).Throws<InvalidOperationException>();

            // Act
            System.Action act = () => accountService.GetAllAccounts();

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetById_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            var accountId = 1;
            mockAccountRepository.Setup(x => x.GetById(accountId)).Throws<ArgumentException>();

            // Act
            System.Action act = () => accountService.GetById(accountId);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void DeleteAccountByUserName_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            var userName = "testuser";
            mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action())
                .Throws<InvalidOperationException>();

            // Act
            System.Action act = () => accountService.DeleteAccountByUserName(userName);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddAccounts_WhenTransactionFails_ShouldThrow()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { UserName = "user1", Password = "pass1" },
            };

            mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Throws<InvalidOperationException>();

            // Act
            System.Action act = () => accountService.AddAccounts(accounts);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void DeleteAccounts_WhenTransactionFails_ShouldThrow()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { Id = 1, UserName = "user1", Password = "pass1" },
            };

            mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Throws<InvalidOperationException>();

            // Act
            System.Action act = () => accountService.DeleteAccounts(accounts);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        public void GetById_WithEdgeCaseIds_ShouldHandleCorrectly(int accountId)
        {
            // Arrange
            mockAccountRepository.Setup(x => x.GetById(accountId)).Returns((Account?)null);

            // Act
            var result = accountService.GetById(accountId);

            // Assert
            result.Should().BeNull();
            mockAccountRepository.Verify(x => x.GetById(accountId), Times.Once);
        }

        [Fact]
        public void GetAccountsByIds_WithEmptyList_ShouldReturnEmpty()
        {
            // Arrange
            var emptyList = new List<int>();
            mockAccountRepository.Setup(x => x.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<Account, bool>>>()))
                .Returns(new List<Account>());

            // Act
            var result = accountService.GetAccountsByIds(emptyList);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void DeleteAccountsByUserNames_WithEmptyList_ShouldNotThrow()
        {
            // Arrange
            var emptyList = new List<string>();
            mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act
            System.Action act = () => accountService.DeleteAccountsByUserNames(emptyList);

            // Assert
            act.Should().NotThrow();
            mockAccountRepository.Verify(x => x.DeleteByUserNameList(emptyList), Times.Once);
        }

        [Fact]
        public void AddAccount_WithConcurrentDuplicateUsername_ShouldHandleConflict()
        {
            // Arrange
            var account = new Account { UserName = "concurrentuser", Password = "password" };

            // Simula que o usu�rio j� existe no reposit�rio
            mockAccountRepository.Setup(x => x.GetByUserName(account.UserName))
                .Returns(new Account { Id = 1, UserName = "concurrentuser", Password = "existing" });

            mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act
            System.Action act = () => accountService.AddAccount(account);

            // Assert
            act.Should().Throw<ConflictException>();
        }

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

            mockAccountRepository.Setup(x => x.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<Account, bool>>>()))
                .Returns(new List<Account>());

            // Act
            System.Action act = () => accountService.GetAccounts(complexPredicate);

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

            mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act
            System.Action act = () => accountService.AddAccounts(largeAccountList);

            // Assert
            act.Should().NotThrow();
            mockAccountRepository.Verify(x => x.AddRange(largeAccountList), Times.Once);
        }

        [Fact]
        public void UpdateAccountPassword_WithNullValues_ShouldHandleCorrectly()
        {
            // Arrange
            mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act & Assert
            System.Action act1 = () => accountService.UpdateAccountPassword(null!, "newpassword");
            System.Action act2 = () => accountService.UpdateAccountPassword("username", null!);
            System.Action act3 = () => accountService.UpdateAccountPassword(null!, null!);

            act1.Should().NotThrow();
            act2.Should().NotThrow();
            act3.Should().NotThrow();
        }

        [Fact]
        public void UpdateAccountUserName_WithNullValues_ShouldHandleCorrectly()
        {
            // Arrange
            mockUnitOfWork.Setup(x => x.ExecuteInTransaction(It.IsAny<System.Action>()))
                .Callback<System.Action>(action => action());

            // Act & Assert
            System.Action act1 = () => accountService.UpdateAccountUserName(null!, "newusername");
            System.Action act2 = () => accountService.UpdateAccountUserName("oldusername", null!);
            System.Action act3 = () => accountService.UpdateAccountUserName(null!, null!);

            act1.Should().NotThrow();
            act2.Should().NotThrow();
            act3.Should().NotThrow();
        }
    }
}
