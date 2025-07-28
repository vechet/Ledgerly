using AutoMapper;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.AuditLog;
using Ledgerly.API.Models.DTOs.TransactionType;
using Ledgerly.API.Repositories.Interfaces;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Ledgerly.Models;

namespace Ledgerly.API.Services
{
    public class AuditLogService(IMapper mapper,
        IAuditLogRepository auditLogRepository) : IAuditLogService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IAuditLogRepository _auditLogRepository = auditLogRepository;
        public async Task RecordAuditLog(RecordAuditLog log)
        {
            var auditLog = _mapper.Map<AuditLog>(log);
            await _auditLogRepository.CreateAuditLog(auditLog);
        }
    }
}
