using Ledgerly.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledgerly.API.Models.Domains
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Currency { get; set; } = null!;

        [StringLength(500)]
        [Column(TypeName = "nvarchar")]
        public string? Memo { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public int StatusId { get; set; }

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

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public GlobalParam GlobalParam { get; set; } = null!;
    }

}
