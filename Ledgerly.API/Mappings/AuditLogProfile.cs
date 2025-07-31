using AutoMapper;
using Ledgerly.API.Models.DTOs.AuditLog;
using Ledgerly.Models;

namespace Ledgerly.API.Mappings
{
    public class AuditLogProfile : Profile
    {
        public AuditLogProfile() 
        {
            CreateMap<RecordAuditLog, AuditLog>();
        }
    }
}
