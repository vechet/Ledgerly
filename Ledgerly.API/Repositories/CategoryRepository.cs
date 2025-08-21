using Ledgerly.API.Models.Domains;
using Ledgerly.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Udemy.Data;

namespace Ledgerly.API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LedgerlyDbContext _db;

        public CategoryRepository(LedgerlyDbContext db)
        {
            _db = db;
        }

        public async Task<Category> CreateCategoryAsync(Category req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var newCategory = await _db.Category.AddAsync(req);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return newCategory.Entity;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Category> DeleteCategoryAsync(Category req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var currentCategory = await _db.Category.FindAsync(req.Id);
                currentCategory.UserId = req.UserId;
                currentCategory.StatusId = req.StatusId;
                currentCategory.ModifiedBy = req.ModifiedBy;
                currentCategory.ModifiedDate = req.ModifiedDate;
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return currentCategory;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                var categories = await _db.Category
                    .Include(x => x.Parent)
                    .Include(x => x.GlobalParam)
                    .ToListAsync();
                return categories;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            try
            {
                var category = await _db.Category
                    .Include(x => x.Parent)
                    .Include(x => x.GlobalParam)
                    .FirstOrDefaultAsync(t => t.Id == id);
                return category;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<bool> HasTransactionsLinkedAsync(int id, string userId)
        {
            try
            {
                var category = await _db.Transaction.AnyAsync(x => x.CategoryId == id && x.Category.UserId == userId && !x.Category.IsSystemValue);
                return category;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Category> UpdateCategoryAsync(Category req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var currentCategory = await _db.Category.FindAsync(req.Id);
                currentCategory.ParentId = req.ParentId;
                currentCategory.Name = req.Name;
                currentCategory.Memo = req.Memo;
                currentCategory.UserId = req.UserId;
                currentCategory.ModifiedBy = req.ModifiedBy;
                currentCategory.ModifiedDate = req.ModifiedDate;
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return currentCategory;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
