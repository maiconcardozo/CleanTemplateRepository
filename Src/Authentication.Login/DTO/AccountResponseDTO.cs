using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Login.DTO
{
    [DebuggerDisplay("UserName={UserName}, Id={Id}, IsActive={IsActive}")]
    public class AccountResponseDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime DtCreated { get; set; }

        public DateTime? DtDeleted { get; set; }

        public DateTime? DtUpdated { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public string? UpdatedBy { get; set; }

        public string? DeletedBy { get; set; }
    }
}
