using System.Diagnostics;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("IdClaim={IdClaim}, IdAction={IdAction}, Id={Id}")]
    public class ClaimActionPayLoadDTO
    {
        public int Id { get; set; }

        public int IdClaim { get; set; }

        public int IdAction { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
