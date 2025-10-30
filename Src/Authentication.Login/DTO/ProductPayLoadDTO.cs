using System.Diagnostics;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("Name={Name}, Id={Id}")]
    public class ProductPayLoadDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
