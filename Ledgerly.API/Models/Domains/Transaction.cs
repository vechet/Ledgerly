using Ledgerly.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledgerly.API.Models.Domains
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [StringLength(500)]
        [Column(TypeName = "nvarchar")]
        public string? Memo { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Type { get; set; } = null!;

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

        public Category Category { get; set; } = null!;
        public Account Account { get; set; } = null!;

        public GlobalParam GlobalParam { get; set; } = null!;
    }

}
