using Authentication.Login.Constants;
using Authentication.Login.Domain.Implementation;
using Authentication.Login.Domain.Interface;
using Authentication.Login.Resource;
using Authentication.Login.Services.Interface;
using Authentication.Login.UnitOfWork.Interface;
using Authentication.Login.Exceptions;
using Foundation.Base.Util;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Text;

namespace Authentication.Login.Services.Implementation
{
    /// <summary>
    /// Service implementation for account management operations.
    /// Handles user account CRUD operations, authentication, password hashing, and JWT token generation.
    /// Implements comprehensive business logic with transaction management and security validations.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly ILoginUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of AccountService with required dependencies.
        /// </summary>
        /// <param name="unitOfWork">Unit of work for transaction management and repository access</param>
        public AccountService(ILoginUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Consultas

        public IEnumerable<Account> GetAllAccounts()
        {
            return _unitOfWork.AccountRepository.GetAll();
        }

        public Account? GetAccountByUserName(string userName)
        {
            return _unitOfWork.AccountRepository.GetByUserName(userName);
        }

        public async Task<Account?> GetAccountByUserNameAsync(string userName)
        {
            return await _unitOfWork.AccountRepository.GetByUserNameAsync(userName);
        }

        /// <summary>
        /// Authenticates user credentials by verifying username and password.
        /// Uses Argon2 hash verification for secure password validation.
        /// </summary>
        /// <param name="account">Account with username and plain text password</param>
        /// <returns>Authenticated account entity from database</returns>
        /// <exception cref="InvalidOperationException">When username is not found</exception>
        /// <exception cref="UnauthorizedAccessException">When password verification fails</exception>
        public Account GetAccountByUserNameAndPassword(Account account)
        {
            // First, retrieve the account by username from the database
            var dbAccount = _unitOfWork.AccountRepository.GetByUserName(account.UserName);

            // If no account exists with this username, throw exception
            if (dbAccount == null)
                throw new InvalidOperationException(ResourceLogin.AccountNotFound);

            // Verify the provided plain text password against the stored Argon2 hash
            // StringHelper.VerifyArgon2Hash compares the plain text password with the hashed version
            if (StringHelper.VerifyArgon2Hash(account.Password, dbAccount.Password))
                return dbAccount;

            // If password verification fails, throw unauthorized access exception
            throw new UnauthorizedAccessException(ResourceLogin.InvalidPassword);
        }

        public Account? GetAccount(Account account)
        {
            return _unitOfWork.AccountRepository.Get(account);
        }

        public IEnumerable<Account> GetAccountsByIds(Account account)
        {
            return _unitOfWork.AccountRepository.GetByLstId(account);
        }

        public IEnumerable<Account> GetAllAccountEntities()
        {
            return _unitOfWork.AccountRepository.GetAll();
        }

        public IEnumerable<Account> GetAccounts(Expression<Func<Account, bool>> predicate)
        {
            return _unitOfWork.AccountRepository.Find(predicate);
        }

        public Account? GetSingleOrDefaultAccount(Expression<Func<Account, bool>> predicate)
        {
            return _unitOfWork.AccountRepository.Find(predicate).SingleOrDefault();
        }

        /// <summary>
        /// Creates a new user account with security validations.
        /// Performs username uniqueness check, password hashing, and audit field population.
        /// </summary>
        /// <param name="account">Account entity to create with plain text password</param>
        /// <exception cref="ConflictException">When username already exists in the system</exception>
        public void AddAccount(Account account)
        {
            // Business rule: Ensure username uniqueness across the system
            // This prevents duplicate user registrations and maintains data integrity
            var existingAccount = _unitOfWork.AccountRepository.GetByUserName(account.UserName);
            if (existingAccount != null)
            {
                throw new ConflictException(ResourceLogin.DuplicateUserName);
            }

            // Set audit fields for tracking when and by whom the account was created
            account.DtCreated = DateTime.Now;
            // Use the CreatedBy value from the DTO instead of a default value
            // This allows proper audit tracking of who actually created the account
            if (string.IsNullOrEmpty(account.CreatedBy))
            {
                account.CreatedBy = ApplicationConstants.DefaultCreatedByUser;
            }

            // Security: Hash the plain text password using Argon2 algorithm
            // This ensures passwords are never stored in plain text in the database
            // Argon2 is a memory-hard function resistant to GPU-based attacks
            account.Password = StringHelper.ComputeArgon2Hash(account.Password);

            // Execute the database operation within a transaction
            // This ensures data consistency and allows rollback if any error occurs
            _unitOfWork.ExecuteInTransaction(() =>
            {
                _unitOfWork.AccountRepository.Add(account);
            });
        }

        public void AddAccounts(IEnumerable<Account> accounts)
        {
            _unitOfWork.ExecuteInTransaction(() =>
            {
                _unitOfWork.AccountRepository.AddRange(accounts);
            });
        }

        public void UpdateAccountPassword(string userName, string newPassword)
        {
            _unitOfWork.ExecuteInTransaction(() =>
            {
                _unitOfWork.AccountRepository.UpdatePassword(userName, newPassword);
            });
        }

        public void UpdateAccountUserName(string oldUserName, string newUserName)
        {
            _unitOfWork.ExecuteInTransaction(() =>
            {
                _unitOfWork.AccountRepository.UpdateUserName(oldUserName, newUserName);
            });
        }

        public void DeleteAccountByUserName(string userName)
        {
            _unitOfWork.ExecuteInTransaction(() =>
            {
                _unitOfWork.AccountRepository.DeleteByUserName(userName);
            });
        }

        public void DeleteAccountsByUserNames(IEnumerable<string> userNames)
        {
            _unitOfWork.ExecuteInTransaction(() =>
            {
                _unitOfWork.AccountRepository.DeleteByUserNameList(userNames);
            });
        }

        public void DeleteAccount(Account account)
        {
            _unitOfWork.ExecuteInTransaction(() =>
            {
                _unitOfWork.AccountRepository.Remove(account);
            });
        }

        public void DeleteAccounts(IEnumerable<Account> accounts)
        {
            _unitOfWork.ExecuteInTransaction(() =>
              {
                  _unitOfWork.AccountRepository.RemoveRange(accounts);
              });
        }
        #endregion

        /// <summary>
        /// Generates a JWT token for an authenticated user with role-based access control (RBAC) claims.
        /// Creates a signed JWT containing user identity and permission claims for authorization.
        /// </summary>
        /// <param name="account">Account with username and password for authentication</param>
        /// <param name="jwtSettings">JWT configuration including secret key, issuer, and audience</param>
        /// <returns>JWT token object with access token string, expiration, and username</returns>
        /// <exception cref="InvalidOperationException">When account authentication fails</exception>
        /// <exception cref="UnauthorizedAccessException">When password verification fails</exception>
        Token? IAccountService.GenerateToken(Account account, IJwtSettings jwtSettings)
        {
            // Step 1: Authenticate the user credentials
            // This validates username exists and password matches the stored Argon2 hash
            var isAccountValid = GetAccountByUserNameAndPassword(account);

            // Step 2: Retrieve user permissions from RBAC system
            // Gets all claim-action mappings associated with this user account
            // This enables role-based access control for different system resources
            var accountClaimActions = _unitOfWork.AccountClaimActionRepository
                .GetByIdAccount(isAccountValid.Id)
                .ToList();

            // Step 3: Build JWT claims collection
            // Start with the standard username claim for user identification
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, isAccountValid.UserName)
            };

            // Step 4: Add permission claims in "Resource:Action" format
            // Example: "PlanoSaude:Inserir" means user can insert into PlanoSaude resource
            // This creates fine-grained permissions for different system operations
            claims.AddRange(accountClaimActions.Select(aca =>
                new System.Security.Claims.Claim(
                    ApplicationConstants.ClaimTypes.Permission,
                    $"{aca.ClaimAction.Claim.Value}:{aca.ClaimAction.Action.Name}"
                )
            ));

            // Step 5: Create JWT signing credentials using HMAC-SHA256
            // Secret key must be at least 256 bits (32 characters) for security
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            // Step 6: Build the JWT security token with all components
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,           // Token issuer for validation
                audience: jwtSettings.Audience,       // Intended token audience
                claims: claims,                       // User identity and permissions
                expires: DateTime.Now.AddHours(ApplicationConstants.DefaultTokenExpirationHours), // Token lifetime
                signingCredentials: creds);           // Digital signature for integrity

            // Step 7: Serialize the JWT to a string format
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            // Step 8: Create response token object with metadata
            var token = new Token
            {
                AccessToken = tokenString,
                Expiration = DateTime.Now.AddHours(ApplicationConstants.DefaultTokenExpirationHours),
                UserName = isAccountValid.UserName
            };

            return token;
        }

        /// <summary>
        /// Asynchronously generates a JWT token for an authenticated user with RBAC claims.
        /// This is the async version of GenerateToken with identical functionality.
        /// </summary>
        /// <param name="account">Account with username and password for authentication</param>
        /// <param name="jwtSettings">JWT configuration including secret key, issuer, and audience</param>
        /// <returns>Task containing JWT token object with access token string, expiration, and username</returns>
        /// <exception cref="InvalidOperationException">When account authentication fails</exception>
        /// <exception cref="UnauthorizedAccessException">When password verification fails</exception>
        public async Task<Token?> GenerateTokenAsync(Account account, IJwtSettings jwtSettings)
        {
            // Authentication and validation (same as synchronous version)
            var isAccountValid = GetAccountByUserNameAndPassword(account);

            // Retrieve user permissions for RBAC implementation
            var accountClaimActions = _unitOfWork.AccountClaimActionRepository
                .GetByIdAccount(isAccountValid.Id)
                .ToList();

            // Build standard identity claims
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, isAccountValid.UserName)
            };

            // Add permission claims in "Resource:Action" format for fine-grained access control
            claims.AddRange(accountClaimActions.Select(aca =>
                new System.Security.Claims.Claim(
                    ApplicationConstants.ClaimTypes.Permission,
                    $"{aca.ClaimAction.Claim.Value}:{aca.ClaimAction.Action.Name}"
                )
            ));

            // Create JWT with HMAC-SHA256 signing
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(ApplicationConstants.DefaultTokenExpirationHours),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var token = new Token
            {
                AccessToken = tokenString,
                Expiration = DateTime.Now.AddHours(ApplicationConstants.DefaultTokenExpirationHours),
                UserName = isAccountValid.UserName
            };

            // Return as completed task for async compatibility
            return await Task.FromResult(token);
        }

        public Account? GetById(int id)
        {
            return _unitOfWork.AccountRepository.GetById(id);
        }

        public IEnumerable<Account> GetAccountsByIds(IEnumerable<int> accountIds)
        {
            return _unitOfWork.AccountRepository.Find(a => accountIds.Contains(a.Id));
        }

        /// <summary>
        /// Updates an existing account with validation and security measures.
        /// Handles username uniqueness validation and password re-hashing if changed.
        /// </summary>
        /// <param name="account">Account entity with updated information</param>
        /// <exception cref="ConflictException">When the new username is already taken by another account</exception>
        public void UpdateAccount(Account account)
        {
            // Business rule: Ensure username uniqueness when updating
            // Allow the same username if it belongs to the account being updated (no change)
            var existingAccount = _unitOfWork.AccountRepository.GetByUserName(account.UserName);
           
            if (existingAccount != null && existingAccount.Id != account.Id)
            {
                throw new ConflictException(ResourceLogin.DuplicateUserName);
            }

            // Update audit fields for tracking modifications
            account.DtUpdated = DateTime.Now;
            // Use the UpdatedBy value from the DTO instead of a default value
            // This allows proper audit tracking of who actually updated the account
            if (string.IsNullOrEmpty(account.UpdatedBy))
            {
                account.UpdatedBy = ApplicationConstants.DefaultCreatedByUser;
            }
            
            // Security: Re-hash password if it was provided/changed
            // Only hash if password field contains a value (not null/empty)
            // This allows partial updates without affecting existing password
            if (!string.IsNullOrEmpty(account.Password))
            {
                account.Password = StringHelper.ComputeArgon2Hash(account.Password);
            }

            // Execute update within transaction for data consistency
            _unitOfWork.ExecuteInTransaction(() =>
            {
                _unitOfWork.AccountRepository.Update(account);
            });
        }

        public void DeleteAccount(int id)
        {
            _unitOfWork.ExecuteInTransaction(() =>
            {
                var account = _unitOfWork.AccountRepository.GetById(id);
                if (account != null)
                {
                    _unitOfWork.AccountRepository.Remove(account);
                }
            });
        }
    }
}