using Ledgerly.Models;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface IAuditLogRepository
    {
        Task<List<AuditLog>> GetAuditLogsAsync();

        Task<AuditLog> GetAuditLogAsync(int id);

        Task<AuditLog> CreateAuditLogAsync(AuditLog req);
    }
}
