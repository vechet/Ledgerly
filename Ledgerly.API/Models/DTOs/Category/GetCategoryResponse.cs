using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.Category
{
    public class GetCategoryResponse
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string? ParentName { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public string? Memo { get; set; }
    }
}
