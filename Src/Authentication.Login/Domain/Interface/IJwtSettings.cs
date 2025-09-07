namespace Authentication.Login.Domain.Interface
{
    public interface IJwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
    }
}
