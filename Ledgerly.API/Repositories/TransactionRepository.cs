using Ledgerly.API.Models.Domains;
using Ledgerly.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Udemy.Data;

namespace Ledgerly.API.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly LedgerlyDbContext _db;

        public TransactionRepository(LedgerlyDbContext db)
        {
            _db = db;
        }

        public async Task<Transaction> CreateTransaction(Transaction req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var newTransaction = await _db.Transaction.AddAsync(req);
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

        public async Task<Transaction> GetTransaction(int id)
        {
            try
            {
                var transaction = await _db.Transaction
                    .Include(x => x.Status)
                    .Include(x => x.Category)
                    .Include(x => x.Account)
                    .FirstOrDefaultAsync(t => t.Id == id);
                return transaction;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            try
            {
                var transactions = await _db.Transaction
                    .Include(x => x.Status)
                    .Include(x => x.Category)
                    .Include(x => x.Account)
                    .ToListAsync();
                return transactions;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Transaction> UpdateTransaction(Transaction req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var currentTransaction = await _db.Transaction.FindAsync(req.Id);
                currentTransaction.AccountId = req.AccountId;
                currentTransaction.CategoryId = req.CategoryId;
                currentTransaction.Amount = req.Amount;
                currentTransaction.TransactionDate = req.TransactionDate;
                currentTransaction.Memo = req.Memo;
                currentTransaction.Type = req.Type;
                currentTransaction.UserId = req.UserId;
                currentTransaction.StatusId = req.StatusId;
                currentTransaction.ModifiedBy = req.ModifiedBy;
                currentTransaction.ModifiedDate = req.ModifiedDate;
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return currentTransaction;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
