using System.Diagnostics;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("IdAccount={IdAccount}, IdClaimAction={IdClaimAction}, Id={Id}")]
    public class AccountClaimActionPayLoadDTO
    {
        public int Id { get; set; }

        public int IdAccount { get; set; }

        public int IdClaimAction { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
