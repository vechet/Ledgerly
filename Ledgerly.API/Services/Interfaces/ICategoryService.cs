using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.Category;
using Ledgerly.API.Models.DTOs.Transaction;
using Ledgerly.Helpers;

namespace Ledgerly.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<GetCategoriesResponse>> GetCategoriesAsync(PaginationRequest req);

        Task<ApiResponse<GetCategoryResponse>> GetCategoryAsync(GetCategoryRequest req);

        Task<ApiResponse<CreateCategoryResponse>> CreateCategoryAsync(CreateCategoryRequest req);

        Task<ApiResponse<UpdateCategoryResponse>> UpdateCategoryAsync(UpdateCategoryRequest req);

        Task<ApiResponse<DeleteCategoryResponse>> DeleteCategoryAsync(DeleteCategoryRequest req);

    }
}
