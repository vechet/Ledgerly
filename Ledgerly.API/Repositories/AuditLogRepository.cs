using Ledgerly.API.Repositories.Interfaces;
using Ledgerly.Models;
using Microsoft.EntityFrameworkCore;
using Udemy.Data;

namespace Ledgerly.API.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly LedgerlyDbContext _db;

        public AuditLogRepository(LedgerlyDbContext db)
        {
            _db = db;
        }

        public async Task<AuditLog> CreateAuditLogAsync(AuditLog req)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var newAuditLog = await _db.AuditLog.AddAsync(req);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return newAuditLog.Entity;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<AuditLog> GetAuditLogAsync(int id)
        {
            try
            {
                var auditLog = await _db.AuditLog.FirstOrDefaultAsync(t => t.Id == id);
                return auditLog;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<AuditLog>> GetAuditLogsAsync()
        {
            try
            {
                var auditLogs = await _db.AuditLog.ToListAsync();
                return auditLogs;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
