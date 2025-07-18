using Ledgerly.API.Models.Domains;
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
                var newTransaction = await _db.transactionTypes.AddAsync(req);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return newTransaction.Entity;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<TransactionType> GetTransactionType(int id)
        {
            var transactionType = await _db.transactionTypes.FirstOrDefaultAsync(t => t.Id == id);
            return transactionType;
        }

        public async Task<List<TransactionType>> GetTransactionTypes()
        {
            var transactionTypes =  await _db.transactionTypes.ToListAsync();
            return transactionTypes;
        }

        public async Task<TransactionType> UpdateTransactionType(TransactionType req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var currentTransaction = await _db.transactionTypes.FindAsync(req.Id);
                currentTransaction.Name = req.Name;
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return currentTransaction;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
