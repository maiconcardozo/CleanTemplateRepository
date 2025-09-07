using Authentication.Login.Enum;

namespace Authentication.Login.DTO
{
    public class ClaimPayLoadDTO
    {
        public ClaimType Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
    }
}