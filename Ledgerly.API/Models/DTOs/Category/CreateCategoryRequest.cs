using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.Category
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        public int? ParentId { get; set; }

        [Required]
        public int CategoryTypeId { get; set; }

        [Required]
        public short StatusId { get; set; }
    }
}
