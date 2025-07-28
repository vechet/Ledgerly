using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.AuditLog
{
    public class RecordAuditLog
    {
        [Required]
        public string ControllerName { get; set; } = null!;

        [Required]
        public string MethodName { get; set; } = null!;

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public string TransactionKeyValue { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string CreatedBy { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
