using System.ComponentModel.DataAnnotations;
using Ledgerly.API.Helpers;

namespace Ledgerly.API.Models.DTOs.Category
{
    public class GetCategoriesResponse
    {
        public List<CategoriesResponse> Categories { get; set; }
        public PageInfo PageInfo { get; set; }

        public GetCategoriesResponse()
        { }

        public GetCategoriesResponse(List<CategoriesResponse> categories, PageInfo pageInfo)
        {
            Categories = categories;
            PageInfo = pageInfo;
        }
    }

    public class CategoriesResponse
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string? ParentName { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public string? Memo { get; set; }
    }
}
