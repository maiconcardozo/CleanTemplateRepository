namespace Authentication.Login.DTO
{
    public class TokenResponseDTO
    {
        public required string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public required string UserName { get; set; }
    }
}
