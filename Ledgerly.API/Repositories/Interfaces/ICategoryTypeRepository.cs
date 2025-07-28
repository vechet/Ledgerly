using Ledgerly.API.Models.Domains;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface ICategoryTypeRepository
    {
        Task<List<CategoryType>> GetCategoryTypes();

        Task<CategoryType> GetCategoryType(int id);

        Task<CategoryType> CreateCategoryType(CategoryType req);

        Task<CategoryType> UpdateCategoryType(CategoryType req);
    }
}
