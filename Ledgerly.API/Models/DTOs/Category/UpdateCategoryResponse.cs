using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.Category
{
    public class UpdateCategoryResponse
    {
        [Required]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int CategoryTypeId { get; set; }

        [Required]
        public short StatusId { get; set; }
    }
}
