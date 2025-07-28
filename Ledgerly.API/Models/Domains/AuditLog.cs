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

        [Required]
        public int TransactionId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string TransactionNo { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(450)]
        [Column(TypeName = "nvarchar")]
        public string CreatedBy { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}