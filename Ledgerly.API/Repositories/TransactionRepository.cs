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

        public async Task<Transaction> CreateTransactionAsync(Transaction req)
        {
            var newTransaction = await _db.Transaction.AddAsync(req);
            return newTransaction.Entity;
        }

        public async Task<Transaction> DeleteTransactionAsync(Transaction req)
        {
            var currentTransaction = await _db.Transaction.FindAsync(req.Id);
            currentTransaction.UserId = req.UserId;
            currentTransaction.StatusId = req.StatusId;
            currentTransaction.ModifiedBy = req.ModifiedBy;
            currentTransaction.ModifiedDate = req.ModifiedDate;
            return currentTransaction;
        }

        public async Task<Transaction> GetTransactionAsync(int id)
        {
            try
            {
                var transaction = await _db.Transaction
                    .Include(x => x.GlobalParam)
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

        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            try
            {
                var transactions = await _db.Transaction
                    .Include(x => x.GlobalParam)
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

        public async Task<Transaction> UpdateTransactionAsync(Transaction req)
        {
            var currentTransaction = await _db.Transaction.FindAsync(req.Id);
            currentTransaction.AccountId = req.AccountId;
            currentTransaction.CategoryId = req.CategoryId;
            currentTransaction.Amount = req.Amount;
            currentTransaction.TransactionDate = req.TransactionDate;
            currentTransaction.Memo = req.Memo;
            currentTransaction.Type = req.Type;
            currentTransaction.UserId = req.UserId;
            currentTransaction.ModifiedBy = req.ModifiedBy;
            currentTransaction.ModifiedDate = req.ModifiedDate;
            return currentTransaction;
        }
    }
}
