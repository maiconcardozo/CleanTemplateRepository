using System.Diagnostics;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("IdApplication={IdApplication}, IdClaim={IdClaim}, Id={Id}")]
    public class ApplicationClaimResponseDTO
    {
        public int Id { get; set; }

        public int IdApplication { get; set; }

        public int IdClaim { get; set; }

        public DateTime DtCreated { get; set; }

        public DateTime? DtDeleted { get; set; }

        public DateTime? DtUpdated { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public string? UpdatedBy { get; set; }

        public string? DeletedBy { get; set; }
    }
}
