using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.Category
{
    public class DeleteCategoryRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
