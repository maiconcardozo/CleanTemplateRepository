using Authentication.Login.Domain.Interface;

namespace Authentication.Login.Domain.Implementation
{
    public class JwtSettings : IJwtSettings
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string SecretKey { get; set; }
    }
}
