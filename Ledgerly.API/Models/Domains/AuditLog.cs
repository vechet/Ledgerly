using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledgerly.Models
{
    [Table("AuditLog")]
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ControllerName { get; set; } = null!;

        [Required]
        public string MethodName { get; set; } = null!;

        public int? TransactionId { get; set; }

        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string? TransactionKeyValue { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        [Required]
        [StringLength(450)]
        [Column(TypeName = "nvarchar")]
        public string CreatedBy { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}