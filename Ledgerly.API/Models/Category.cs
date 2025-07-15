using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledgerly.API.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; } = null!;

        [Required]
        public int CategoryTypeId { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public CategoryType CategoryType { get; set; } = null!;

    }

}
