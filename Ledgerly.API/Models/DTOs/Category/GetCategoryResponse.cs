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

        [Required]
        public int CategoryTypeId { get; set; }

        [Required]
        public string CategoryTypeName { get; set; } = null!;

        [Required]
        public short StatusId { get; set; }

        [Required]
        public string StatusName { get; set; } = null!;
    }
}
