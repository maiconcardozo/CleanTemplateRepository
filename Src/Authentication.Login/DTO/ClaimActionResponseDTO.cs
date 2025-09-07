namespace Authentication.Login.DTO
{
    public class ClaimActionResponseDTO
    {
        public int Id { get; set; }
        public int IdClaim { get; set; }
        public int IdAction { get; set; }
        public ClaimResponseDTO? Claim { get; set; }
        public ActionResponseDTO? Action { get; set; }
        public DateTime DtCreated { get; set; }
        public DateTime? DtDeleted { get; set; }
        public DateTime? DtUpdated { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
    }
}