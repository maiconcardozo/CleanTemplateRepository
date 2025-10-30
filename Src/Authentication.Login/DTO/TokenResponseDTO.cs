using System.Diagnostics;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("UserName={UserName}, AccessToken={AccessToken}")]
    public class TokenResponseDTO
    {
        public required string AccessToken { get; set; }

        public DateTime Expiration { get; set; }

        public required string UserName { get; set; }
    }
}
