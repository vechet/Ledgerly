using Ledgerly.API.Models.Domains;
using Ledgerly.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Udemy.Data;

namespace Ledgerly.API.Repositories
{
    public class BrandRepository(LedgerlyDbContext db) : IBrandRepository
    {
        private readonly LedgerlyDbContext _db = db;

        public void Create(Account account)
        {
            _db.Account.Add(account);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _db.Database.BeginTransactionAsync();
        }

        public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
    }
}
