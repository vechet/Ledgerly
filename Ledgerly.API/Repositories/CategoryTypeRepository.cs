using Ledgerly.API.Models.Domains;
using Ledgerly.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Udemy.Data;

namespace Ledgerly.API.Repositories
{
    public class CategoryTypeRepository : ICategoryTypeRepository
    {
        private readonly LedgerlyDbContext _db;

        public CategoryTypeRepository(LedgerlyDbContext db)
        {
            _db = db;
        }

        public async Task<CategoryType> CreateCategoryType(CategoryType req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var newCategoryType = await _db.CategoryType.AddAsync(req);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return newCategoryType.Entity;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<CategoryType> GetCategoryType(int id)
        {
            try
            {
                var categoryTypes = await _db.CategoryType
                    .Include(x => x.Status)
                    .FirstOrDefaultAsync(t => t.Id == id);
                return categoryTypes;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<CategoryType>> GetCategoryTypes()
        {
            try
            {
                var categoryTypes = await _db.CategoryType
                    .Include(x => x.Status)
                    .ToListAsync();
                return categoryTypes;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<CategoryType> UpdateCategoryType(CategoryType req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var currentCategoryType = await _db.CategoryType.FindAsync(req.Id);
                currentCategoryType.Name = req.Name;
                currentCategoryType.StatusId = req.StatusId;
                currentCategoryType.ModifiedBy = req.ModifiedBy;
                currentCategoryType.ModifiedDate = req.ModifiedDate;
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return currentCategoryType;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
