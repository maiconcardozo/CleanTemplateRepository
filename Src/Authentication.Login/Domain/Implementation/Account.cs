using Authentication.Login.Domain.Interface;
using Foundation.Base.Domain.Implementation;

namespace Authentication.Login.Domain.Implementation
{
    /// <summary>
    /// Represents a user account entity in the authentication system.
    /// Inherits from Entity base class providing audit fields and implements IAccount interface.
    /// </summary>
    public class Account : Entity, IAccount
    {
        /// <summary>
        /// Gets or sets the unique username for this account.
        /// Must be unique across the system and is used for authentication.
        /// Cannot contain spaces or be null/empty.
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the password for this account.
        /// Automatically hashed using Argon2 algorithm when stored.
        /// Plain text passwords are only used during authentication validation.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
