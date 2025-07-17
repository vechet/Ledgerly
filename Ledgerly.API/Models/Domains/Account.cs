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

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

}
