using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.Domains
{
    [Table("CategoryType")]
    public class CategoryType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Name { get; set; } = null!;

        public ICollection<Category> categories { get; set; } = new List<Category>();
    }
}
