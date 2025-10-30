using System.Diagnostics;

namespace Authentication.Login.Domain.Implementation
{
    /// <summary>
    /// Represents a JWT authentication token with expiration and user information.
    /// Used as a response object for successful authentication operations.
    /// </summary>
    [DebuggerDisplay("UserName={UserName}, AccessToken={AccessToken}")]
    public class Token
    {
        /// <summary>
        /// Gets or sets the JWT access token string.
        /// This token should be included in the Authorization header as "Bearer {AccessToken}"
        /// for authenticated requests.
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the token.
        /// After this time, the token will be considered invalid.
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Gets or sets the username associated with this token.
        /// Used for logging and tracking purposes.
        /// </summary>
        public required string UserName { get; set; }
    }
}
