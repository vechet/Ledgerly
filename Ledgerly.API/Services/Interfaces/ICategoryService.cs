using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.Category;
using Ledgerly.API.Models.DTOs.Transaction;
using Ledgerly.Helpers;

namespace Ledgerly.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<GetCategoriesResponse>> GetCategories(PaginationRequest req);

        Task<ApiResponse<GetCategoryResponse>> GetCategory(GetCategoryRequest req);

        Task<ApiResponse<CreateCategoryResponse>> CreateCategory(CreateCategoryRequest req);

        Task<ApiResponse<UpdateCategoryResponse>> UpdateCategory(UpdateCategoryRequest req);

        Task<ApiResponse<DeleteCategoryResponse>> DeleteCategory(DeleteCategoryRequest req);

    }
}
