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
        public string UserId { get; set; } = null!; // FK to AspNetUsers

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }

        [Required]
        public int TransactionTypeId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int AccountId { get; set; }

        [StringLength(500)]
        [Column(TypeName = "nvarchar")]
        public string? Notes { get; set; }

        public Category Category { get; set; } = null!;
        public Account Account { get; set; } = null!;
        public TransactionType TransactionType { get; set; } = null!;
    }

}
