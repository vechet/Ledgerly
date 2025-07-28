using Ledgerly.API.Models.Domains;
using Ledgerly.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Udemy.Data;

namespace Ledgerly.API.Repositories
{
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private readonly LedgerlyDbContext _db;

        public TransactionTypeRepository(LedgerlyDbContext db)
        {
            _db = db;
        }

        public async Task<TransactionType> CreateTransactionType(TransactionType req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var newTransactionType = await _db.TransactionType.AddAsync(req);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return newTransactionType.Entity;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<TransactionType> GetTransactionType(int id)
        {
            try
            {
                var transactionType = await _db.TransactionType
                    .Include(x => x.Status)
                    .FirstOrDefaultAsync(t => t.Id == id);
                return transactionType;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<TransactionType>> GetTransactionTypes()
        {
            try
            {
                var transactionTypes = await _db.TransactionType
                    .Include(x => x.Status)
                    .ToListAsync();
                return transactionTypes;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<TransactionType> UpdateTransactionType(TransactionType req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var currentTransactionType = await _db.TransactionType.FindAsync(req.Id);
                currentTransactionType.Name = req.Name;
                currentTransactionType.StatusId = req.StatusId;
                currentTransactionType.ModifiedBy = req.ModifiedBy;
                currentTransactionType.ModifiedDate = req.ModifiedDate;
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return currentTransactionType;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
