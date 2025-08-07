using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledgerly.API.Models.DTOs.Category
{
    public class RecordAuditLogCategory
    {
        [Required]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string? ParentName { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int CategoryTypeId { get; set; }

        [Required]
        public string CategoryTypeName { get; set; } = null!;

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
