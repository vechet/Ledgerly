using Ledgerly.API.Models.Domains;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledgerly.Models
{
    [Table("Status")]
    public class Status
    {
        [Key]
        public short Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string KeyName { get; set; } = null!;

        [Required]
        public string CreatedBy { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<CategoryType> CategoryTypes { get; set; } = new List<CategoryType>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<TransactionType> TransactionTypes { get; set; } = new List<TransactionType>();
        public ICollection<Account> Accounts { get; set; } = new List<Account>();


    }
}