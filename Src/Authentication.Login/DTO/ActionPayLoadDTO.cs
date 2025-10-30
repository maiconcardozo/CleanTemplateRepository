using System.Diagnostics;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("Name={Name}, Id={Id}")]
    public class ActionPayLoadDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
