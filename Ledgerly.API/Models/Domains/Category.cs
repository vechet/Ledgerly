using Ledgerly.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledgerly.API.Models.Domains
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; } = null!;

        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string? IconName { get; set; }

        [StringLength(500)]
        [Column(TypeName = "nvarchar")]
        public string? Memo { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public bool IsSystemValue { get; set; }

        [Required]
        public short StatusId { get; set; }

        [Required]
        [StringLength(450)]
        [Column(TypeName = "nvarchar")]
        public string CreatedBy { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }

        [StringLength(450)]
        [Column(TypeName = "nvarchar")]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }


        public Category? Parent { get; set; }

        public ICollection<Category> Children { get; set; } = new List<Category>();

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public Status Status { get; set; } = null!;

    }

}
