using System;
using System.Diagnostics;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("SKU={SKU}, IdProduct={IdProduct}, Id={Id}, IsActive={IsActive}")]
    public class ProductVariantResponseDTO
    {
        public int Id { get; set; }

        public int IdProduct { get; set; }

        public string SKU { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }

        public DateTime DtCreated { get; set; }

        public DateTime? DtDeleted { get; set; }

        public DateTime? DtUpdated { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public string? UpdatedBy { get; set; }

        public string? DeletedBy { get; set; }
    }
}
