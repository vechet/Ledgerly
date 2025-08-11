using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledgerly.API.Models.DTOs.Account
{
    public class RecordAuditLogAccount
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Currency { get; set; } = null!;

        public string? Memo { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public string StatusName { get; set; } = null!;

        [Required]
        public string CreatedBy { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string? ModifiedBy { get; set; }

        [Required]
        public DateTime? ModifiedDate { get; set; }
    }
}
