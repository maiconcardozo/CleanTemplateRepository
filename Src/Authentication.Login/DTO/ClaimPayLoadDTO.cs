using Authentication.Login.Enum;
using System.Diagnostics;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("Value={Value}, Type={Type}, Id={Id}")]
    public class ClaimPayLoadDTO
    {
        public int Id { get; set; }

        public ClaimType Type { get; set; }

        public string Value { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
