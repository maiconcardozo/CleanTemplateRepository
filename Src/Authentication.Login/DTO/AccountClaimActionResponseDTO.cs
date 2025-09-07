namespace Authentication.Login.DTO
{
    public class AccountClaimActionResponseDTO
    {
        public int Id { get; set; }
        public int IdAccount { get; set; }
        public int IdClaimAction { get; set; }
        public AccountResponseDTO? Account { get; set; }
        public ClaimActionResponseDTO? ClaimAction { get; set; }
        public DateTime DtCreated { get; set; }
        public DateTime? DtDeleted { get; set; }
        public DateTime? DtUpdated { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
    }
}