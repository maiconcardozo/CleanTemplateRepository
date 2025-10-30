using Authentication.Login.Domain.Interface;
using System.Diagnostics;

namespace Authentication.Login.Domain.Implementation
{
    [DebuggerDisplay("Issuer={Issuer}, Audience={Audience}")]
    public class JwtSettings : IJwtSettings
    {
        public required string Issuer { get; set; }

        public required string Audience { get; set; }

        public required string SecretKey { get; set; }
    }
}
