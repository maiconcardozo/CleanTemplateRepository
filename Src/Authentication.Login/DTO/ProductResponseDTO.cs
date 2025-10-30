using System;
using System.Diagnostics;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("Name={Name}, Id={Id}, IsActive={IsActive}")]
    public class ProductResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime DtCreated { get; set; }

        public DateTime? DtDeleted { get; set; }

        public DateTime? DtUpdated { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public string? UpdatedBy { get; set; }

        public string? DeletedBy { get; set; }
    }
}
