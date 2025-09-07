using Authentication.Login.Services;
// ...existing code...
    public class AccountService : IAccountService
    {
        private readonly ILoginUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public AccountService(ILoginUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        // ...existing code...
        public Account GetAccountByUserNameAndPassword(Account account)
        {
            var dbAccount = _unitOfWork.AccountRepository.GetByUserName(account.UserName);
            if (dbAccount == null)
                throw new InvalidOperationException(ResourceLogin.AccountNotFound);
            if (_passwordHasher.Verify(account.Password, dbAccount.Password))
                return dbAccount;
            throw new UnauthorizedAccessException(ResourceLogin.InvalidPassword);
        }
        // ...existing code...
        public void AddAccount(Account account)
        {
            // ...existing code...
            account.Password = _passwordHasher.Hash(account.Password);
            // ...existing code...
        }
        // ...existing code...
        public void UpdateAccount(Account account)
        {
            // ...existing code...
            account.Password = _passwordHasher.Hash(account.Password);
            // ...existing code...
        }
        // ...existing code...
        public void UpdateAccountPassword(string userName, string newPassword)
        {
            // ...existing code...
            var hashedPassword = _passwordHasher.Hash(newPassword);
            _unitOfWork.AccountRepository.UpdatePassword(userName, hashedPassword);
            // ...existing code...
        }
        // ...existing code...
    }
// ...existing code...