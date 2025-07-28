using Ledgerly.API.Repositories.Interfaces;
using Ledgerly.Models;
using Microsoft.EntityFrameworkCore;
using Udemy.Data;

namespace Ledgerly.API.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly LedgerlyDbContext _db;

        public StatusRepository(LedgerlyDbContext db)
        {
            _db = db;
        }

        public async Task<Status> CreateStatus(Status req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var newStatus = await _db.Status.AddAsync(req);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return newStatus.Entity;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Status> GetStatus(int id)
        {
            try
            {
                var status = await _db.Status.FirstOrDefaultAsync(t => t.Id == id);
                return status;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<Status>> GetStatuses()
        {
            try
            {
                var statuses = await _db.Status.ToListAsync();
                return statuses;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Status> UpdateStatus(Status req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var currentStatus = await _db.Status.FindAsync(req.Id);
                currentStatus.Name = req.Name;
                currentStatus.KeyName = req.KeyName;
                currentStatus.ModifiedBy = req.ModifiedBy;
                currentStatus.ModifiedDate = req.ModifiedDate;
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return currentStatus;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
