using Ledgerly.API.Enums;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Policy = nameof(EnumPermissions.CATEGORY_CREATE))]
        [HttpPost("v1/category/create-category")]
        public async Task<ApiResponse<CreateCategoryResponse>> CreateCategory([FromBody] CreateCategoryRequest req)
        {
            return await _categoryService.CreateCategory(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.CATEGORY_VIEW))]
        [HttpPost("v1/category/get-categories")]
        public async Task<ApiResponse<GetCategoriesResponse>> GetCategories([FromBody] PaginationRequest req)
        {
            return await _categoryService.GetCategories(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.CATEGORY_UPDATE))]
        [HttpPost("v1/category/update-category")]
        public async Task<ApiResponse<UpdateCategoryResponse>> UpdateCategory([FromBody] UpdateCategoryRequest req)
        {
            return await _categoryService.UpdateCategory(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.CATEGORY_VIEW))]
        [HttpPost("v1/category/get-category")]
        public async Task<ApiResponse<GetCategoryResponse>> GetCategory([FromBody] GetCategoryRequest req)
        {
            return await _categoryService.GetCategory(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.CATEGORY_UPDATE))]
        [HttpPost("v1/category/delete-category")]
        public async Task<ApiResponse<DeleteCategoryResponse>> DeleteCategory([FromBody] DeleteCategoryRequest req)
        {
            return await _categoryService.DeleteCategory(req);
        }
    }
}
