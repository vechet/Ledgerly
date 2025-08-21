using Ledgerly.API.Models.Domains;
using Ledgerly.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Udemy.Data;

namespace Ledgerly.API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly LedgerlyDbContext _db;

        public AccountRepository(LedgerlyDbContext db)
        {
            _db = db;
        }

        public async Task<Account> CreateAccountAsync(Account req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var newAccount = await _db.Account.AddAsync(req);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return newAccount.Entity;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Account> DeleteAccountAsync(Account req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var currentAccount = await _db.Account.FindAsync(req.Id);
                currentAccount.UserId = req.UserId;
                currentAccount.StatusId = req.StatusId;
                currentAccount.ModifiedBy = req.ModifiedBy;
                currentAccount.ModifiedDate = req.ModifiedDate;
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return currentAccount;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Account> GetAccountAsync(int id)
        {
            try
            {
                var account = await _db.Account
                    .Include(x => x.GlobalParam)
                    .FirstOrDefaultAsync(t => t.Id == id);
                return account;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<Account>> GetAccountsAsync()
        {
            try
            {
                var accounts = await _db.Account
                    .Include(x => x.GlobalParam)
                    .ToListAsync();
                return accounts;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Account> UpdateAccountAsync(Account req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var currentAccount = await _db.Account.FindAsync(req.Id);
                currentAccount.Name = req.Name;
                currentAccount.Currency = req.Currency;
                currentAccount.Memo = req.Memo;
                currentAccount.UserId = req.UserId;
                currentAccount.ModifiedBy = req.ModifiedBy;
                currentAccount.ModifiedDate = req.ModifiedDate;
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return currentAccount;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
