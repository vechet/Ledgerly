using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Ledgerly.Models;

namespace Ledgerly.API.Models.Domains
{
    [Table("TransactionType")]
    public class TransactionType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; } = null!;

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


        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public Status Status { get; set; } = null!;
    }
}
