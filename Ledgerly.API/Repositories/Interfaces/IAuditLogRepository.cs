using Ledgerly.Models;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface IAuditLogRepository
    {
        Task<List<AuditLog>> GetAuditLogs();

        Task<AuditLog> GetAuditLog(int id);

        Task<AuditLog> CreateAuditLog(AuditLog req);

        Task<AuditLog> UpdateAuditLog(AuditLog req);
    }
}
