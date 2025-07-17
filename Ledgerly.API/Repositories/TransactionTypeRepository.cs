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
            var newTransaction = await _db.transactionTypes.AddAsync(req);
            await _db.SaveChangesAsync();
            return newTransaction.Entity;
        }

        public async Task<TransactionType> GetTransactionType(int id)
        {
            var transactionType = await _db.transactionTypes.FirstOrDefaultAsync(t => t.Id == id);
            return transactionType!;
        }

        public async Task<List<TransactionType>> GetTransactionTypes()
        {
            var transactionTypes =  await _db.transactionTypes.ToListAsync();
            return transactionTypes;
        }

        public async Task<TransactionType> UpdateTransactionType(TransactionType req)
        {
            var currentTransaction = await _db.transactionTypes.FindAsync(req.Id);
            currentTransaction.Name = req.Name;
            await _db.SaveChangesAsync();
            return currentTransaction;
        }
    }
}
