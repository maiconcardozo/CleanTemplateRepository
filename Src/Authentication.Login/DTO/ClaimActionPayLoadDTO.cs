namespace Authentication.Login.DTO
{
    public class ClaimActionPayLoadDTO
    {
        public int IdClaim { get; set; }
        public int IdAction { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
    }
}