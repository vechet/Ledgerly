using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledgerly.API.Models.DTOs.TransactionType
{
    public class RecordAuditLogTransactionType
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public short StatusId { get; set; }

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
