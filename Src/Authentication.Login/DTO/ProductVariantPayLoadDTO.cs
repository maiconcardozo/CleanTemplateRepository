using System.Diagnostics;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("SKU={SKU}, IdProduct={IdProduct}, Id={Id}")]
    public class ProductVariantPayLoadDTO
    {
        public int Id { get; set; }

        public int IdProduct { get; set; }

        public string SKU { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public int StockQuantity { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
