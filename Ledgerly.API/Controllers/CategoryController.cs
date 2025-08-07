using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.Category;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ledgerly.API.Controllers
{
    [Route("api")]
    [ApiController]
    //[Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _CategoryService;

        public CategoryController(ICategoryService CategoryService)
        {
            _CategoryService = CategoryService;
        }

        [HttpPost("v1/category/create-category")]
        public async Task<ApiResponse<CreateCategoryResponse>> CreateCategory([FromBody] CreateCategoryRequest req)
        {
            return await _CategoryService.CreateCategory(req);
        }

        [HttpPost("v1/category/get-categories")]
        public async Task<ApiResponse<GetCategoriesResponse>> GetCategories([FromBody] PaginationRequest req)
        {
            return await _CategoryService.GetCategories(req);
        }

        [HttpPost("v1/category/update-category")]
        public async Task<ApiResponse<UpdateCategoryResponse>> UpdateCategory([FromBody] UpdateCategoryRequest req)
        {
            return await _CategoryService.UpdateCategory(req);
        }

        [HttpPost("v1/category/get-category")]
        public async Task<ApiResponse<GetCategoryResponse>> GetCategory([FromBody] GetCategoryRequest req)
        {
            return await _CategoryService.GetCategory(req);
        }
    }
}
