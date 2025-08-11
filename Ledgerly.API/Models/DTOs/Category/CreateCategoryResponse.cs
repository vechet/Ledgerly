using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.Category
{
    public class CreateCategoryResponse
    {
        [Required]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Memo { get; set; }
    }
}
