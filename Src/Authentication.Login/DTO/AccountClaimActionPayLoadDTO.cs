namespace Authentication.Login.DTO
{
    public class AccountClaimActionPayLoadDTO
    {
        public int IdAccount { get; set; }
        public int IdClaimAction { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
    }
}