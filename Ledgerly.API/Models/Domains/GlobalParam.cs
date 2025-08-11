using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledgerly.API.Models.Domains
{
    [Table("GlobalParam")]
    public class GlobalParam
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
        public string KeyName { get; set; } = null!;

        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string? Type { get; set; }

        [StringLength(500)]
        [Column(TypeName = "nvarchar")]
        public string? Memo { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public ICollection<Account> Accounts { get; set; } = new List<Account>();

        public ICollection<Category> Categories { get; set; } = new List<Category>();

    }
}
