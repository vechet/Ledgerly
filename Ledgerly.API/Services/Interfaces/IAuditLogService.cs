using Ledgerly.API.Models.DTOs.AuditLog;
using Ledgerly.API.Models.DTOs.TransactionType;
using Ledgerly.Helpers;

namespace Ledgerly.API.Services.Interfaces
{
    public interface IAuditLogService
    {
        Task RecordAuditLog(RecordAuditLog log);
    }
}
