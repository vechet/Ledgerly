using AutoMapper;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.TransactionType;

namespace Ledgerly.API.Mappings
{
    public class TransactionTypeProfile : Profile
    {
        public TransactionTypeProfile() 
        {
            CreateMap<CreateTransactionTypeRequest, TransactionType>();
            CreateMap<TransactionType, CreateTransactionTypeResponse>();
            CreateMap<TransactionType, GetTransactionTypeResponse>();
            CreateMap<UpdateTransactionTypeRequest, TransactionType>();
            CreateMap<TransactionType, UpdateTransactionTypeResponse>();
            CreateMap<TransactionType, TransactionTypesResponse>();
            CreateMap<TransactionType, RecordAuditLogTransactionType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate));
        }
    }
}
