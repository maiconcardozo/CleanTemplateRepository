using System.Diagnostics;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("IdApplication={IdApplication}, IdClaim={IdClaim}, Id={Id}")]
    public class ApplicationClaimPayLoadDTO
    {
        public int Id { get; set; }

        public int IdApplication { get; set; }

        public int IdClaim { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
