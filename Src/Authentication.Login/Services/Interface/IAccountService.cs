using System.Linq.Expressions;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Domain.Interface;

namespace Authentication.Login.Services.Interface
{
    /// <summary>
    /// Service interface for account management operations.
    /// Provides comprehensive account CRUD operations, authentication, and token generation.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Retrieves all user accounts from the system.
        /// </summary>
        /// <returns>Collection of all account entities.</returns>
        IEnumerable<Account> GetAllAccounts();

        /// <summary>
        /// Finds an account by username.
        /// </summary>
        /// <param name="userName">The username to search for.</param>
        /// <returns>Account entity if found, null otherwise.</returns>
        Account? GetAccountByUserName(string userName);

        /// <summary>
        /// Asynchronously finds an account by username.
        /// </summary>
        /// <param name="userName">The username to search for.</param>
        /// <returns>Task containing account entity if found, null otherwise.</returns>
        Task<Account?> GetAccountByUserNameAsync(string userName);

        /// <summary>
        /// Authenticates user credentials and returns account if valid.
        /// Verifies username and password combination using Argon2 hash verification.
        /// </summary>
        /// <param name="account">Account with username and plain text password.</param>
        /// <returns>Authenticated account entity.</returns>
        /// <exception cref="InvalidOperationException">Thrown when account is not found.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when password is invalid.</exception>
        Account GetAccountByUserNameAndPassword(Account account);

        /// <summary>
        /// Updates the password for a specific user account.
        /// </summary>
        /// <param name="userName">Username of the account to update.</param>
        /// <param name="newPassword">New password (will be hashed with Argon2).</param>
        void UpdateAccountPassword(string userName, string newPassword);

        /// <summary>
        /// Updates the username for an existing account.
        /// </summary>
        /// <param name="oldUserName">Current username.</param>
        /// <param name="newUserName">New username.</param>
        void UpdateAccountUserName(string oldUserName, string newUserName);

        /// <summary>
        /// Deletes an account by username.
        /// </summary>
        /// <param name="userName">Username of the account to delete.</param>
        void DeleteAccountByUserName(string userName);

        /// <summary>
        /// Deletes multiple accounts by their usernames.
        /// </summary>
        /// <param name="userNames">Collection of usernames to delete.</param>
        void DeleteAccountsByUserNames(IEnumerable<string> userNames);

        /// <summary>
        /// Retrieves an account entity that matches the provided account criteria.
        /// </summary>
        /// <param name="account">Account criteria to match.</param>
        /// <returns>Matching account if found, null otherwise.</returns>
        Account? GetAccount(Account account);

        /// <summary>
        /// Retrieves an account by its unique identifier.
        /// </summary>
        /// <param name="id">Account ID.</param>
        /// <returns>Account entity if found, null otherwise.</returns>
        Account? GetById(int id);

        /// <summary>
        /// Retrieves multiple accounts by their IDs.
        /// </summary>
        /// <param name="accountIds">Collection of account IDs.</param>
        /// <returns>Collection of matching account entities.</returns>
        IEnumerable<Account> GetAccountsByIds(IEnumerable<int> accountIds);

        /// <summary>
        /// Retrieves all account entities (alias for GetAllAccounts).
        /// </summary>
        /// <returns>Collection of all account entities.</returns>
        IEnumerable<Account> GetAllAccountEntities();

        /// <summary>
        /// Retrieves accounts that match the specified predicate condition.
        /// </summary>
        /// <param name="predicate">LINQ expression to filter accounts.</param>
        /// <returns>Collection of matching account entities.</returns>
        IEnumerable<Account> GetAccounts(Expression<Func<Account, bool>> predicate);

        /// <summary>
        /// Retrieves a single account that matches the predicate, or null if none found.
        /// </summary>
        /// <param name="predicate">LINQ expression to filter accounts.</param>
        /// <returns>Single matching account or null.</returns>
        /// <exception cref="InvalidOperationException">Thrown when multiple accounts match the predicate.</exception>
        Account? GetSingleOrDefaultAccount(Expression<Func<Account, bool>> predicate);

        /// <summary>
        /// Creates a new user account in the system.
        /// Automatically hashes the password with Argon2 and validates username uniqueness.
        /// </summary>
        /// <param name="account">Account entity to create.</param>
        /// <exception cref="ConflictException">Thrown when username already exists.</exception>
        void AddAccount(Account account);

        /// <summary>
        /// Creates multiple user accounts in a single transaction.
        /// </summary>
        /// <param name="accounts">Collection of account entities to create.</param>
        void AddAccounts(IEnumerable<Account> accounts);

        /// <summary>
        /// Updates an existing account entity.
        /// </summary>
        /// <param name="account">Account with updated information.</param>
        void UpdateAccount(Account account);

        /// <summary>
        /// Deletes an account entity.
        /// </summary>
        /// <param name="account">Account entity to delete.</param>
        void DeleteAccount(Account account);

        /// <summary>
        /// Deletes an account by its ID.
        /// </summary>
        /// <param name="id">Account ID to delete.</param>
        void DeleteAccount(int id);

        /// <summary>
        /// Deletes multiple account entities.
        /// </summary>
        /// <param name="accounts">Collection of account entities to delete.</param>
        void DeleteAccounts(IEnumerable<Account> accounts);

        /// <summary>
        /// Generates a JWT token for an authenticated user account.
        /// Creates a token with user claims and configurable expiration.
        /// </summary>
        /// <param name="account">Authenticated account entity.</param>
        /// <param name="jwtSettings">JWT configuration settings.</param>
        /// <returns>JWT token object with access token and expiration.</returns>
        Token? GenerateToken(Account account, IJwtSettings jwtSettings);

        /// <summary>
        /// Asynchronously generates a JWT token for an authenticated user account.
        /// Creates a token with user claims and configurable expiration.
        /// </summary>
        /// <param name="account">Authenticated account entity.</param>
        /// <param name="jwtSettings">JWT configuration settings.</param>
        /// <returns>Task containing JWT token object with access token and expiration.</returns>
        Task<Token?> GenerateTokenAsync(Account account, IJwtSettings jwtSettings);
    }
}
