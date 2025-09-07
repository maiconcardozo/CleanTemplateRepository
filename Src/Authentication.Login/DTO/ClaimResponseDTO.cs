using Authentication.Login.Enum;

namespace Authentication.Login.DTO
{
    public class ClaimResponseDTO
    {
        public int Id { get; set; }
        public ClaimType Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DtCreated { get; set; }
        public DateTime? DtDeleted { get; set; }
        public DateTime? DtUpdated { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
    }
}