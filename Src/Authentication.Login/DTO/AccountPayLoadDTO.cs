using System.Diagnostics;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("UserName={UserName}, Id={Id}")]
    public class AccountPayLoadDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
