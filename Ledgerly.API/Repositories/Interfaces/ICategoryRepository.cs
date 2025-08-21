using Ledgerly.API.Models.Domains;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesAsync();

        Task<Category> GetCategoryAsync(int id);

        Task<Category> CreateCategoryAsync(Category req);

        Task<Category> UpdateCategoryAsync(Category req);

        Task<Category> DeleteCategoryAsync(Category req);

        Task<bool> HasTransactionsLinkedAsync(int id, string userId);

    }
}
