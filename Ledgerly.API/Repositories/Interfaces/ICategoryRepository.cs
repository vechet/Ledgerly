using Ledgerly.API.Models.Domains;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();

        Task<Category> GetCategory(int id);

        Task<Category> CreateCategory(Category req);

        Task<Category> UpdateCategory(Category req);
    }
}
