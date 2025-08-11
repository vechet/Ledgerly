using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.Category
{
    public class UpdateCategoryRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int? ParentId { get; set; }

        public string? Memo { get; set; }
    }
}
