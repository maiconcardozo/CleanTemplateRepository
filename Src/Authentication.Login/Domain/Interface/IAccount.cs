using Foundation.Base.Domain.Interface;

namespace Authentication.Login.Domain.Interface
{
    /// <summary>
    /// Represents a user account interface that extends the base entity functionality.
    /// Defines the contract for user account objects with authentication properties.
    /// </summary>
    public interface IAccount : IEntity
    {
        /// <summary>
        /// Gets or sets the unique username for the account.
        /// Used for authentication and user identification.
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// Gets or sets the password for the account.
        /// Should be hashed using Argon2 algorithm before storage.
        /// </summary>
        public string Password { get; set; }
    }
}
