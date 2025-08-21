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
        public async Task<ApiResponse<CreateCategoryResponse>> CreateCategoryAsync([FromBody] CreateCategoryRequest req)
        {
            return await _categoryService.CreateCategoryAsync(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.CATEGORY_VIEW))]
        [HttpPost("v1/category/get-categories")]
        public async Task<ApiResponse<GetCategoriesResponse>> GetCategoriesAsync([FromBody] PaginationRequest req)
        {
            return await _categoryService.GetCategoriesAsync(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.CATEGORY_UPDATE))]
        [HttpPost("v1/category/update-category")]
        public async Task<ApiResponse<UpdateCategoryResponse>> UpdateCategoryAsync([FromBody] UpdateCategoryRequest req)
        {
            return await _categoryService.UpdateCategoryAsync(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.CATEGORY_VIEW))]
        [HttpPost("v1/category/get-category")]
        public async Task<ApiResponse<GetCategoryResponse>> GetCategoryAsync([FromBody] GetCategoryRequest req)
        {
            return await _categoryService.GetCategoryAsync(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.CATEGORY_UPDATE))]
        [HttpPost("v1/category/delete-category")]
        public async Task<ApiResponse<DeleteCategoryResponse>> DeleteCategoryAsync([FromBody] DeleteCategoryRequest req)
        {
            return await _categoryService.DeleteCategoryAsync(req);
        }
    }
}
