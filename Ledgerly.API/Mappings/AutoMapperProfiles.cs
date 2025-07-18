using AutoMapper;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.TransactionType;

namespace Ledgerly.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateTransactionTypeRequest, TransactionType>();
            CreateMap<TransactionType, CreateTransactionTypeResponse>();
            CreateMap<TransactionType, GetTransactionTypeResponse>();
            CreateMap<UpdateTransactionTypeRequest, TransactionType>();
            CreateMap<TransactionType, UpdateTransactionTypeResponse>();
            CreateMap<TransactionType, TransactionTypesResponse>();
            //CreateMap<Brand, BrandDto>();
            //CreateMap<Brand, CreateBrandResDto>();
            //CreateMap<CreateBrandReqDto, Brand>()
            //    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "1"))
            //    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => "2025-06-30"))
            //    .ForMember(dest => dest.IsSystemValue, opt => opt.MapFrom(src => true))
            //    .ForMember(dest => dest.Version, opt => opt.MapFrom(src => 1))
            //    .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => 1));
        }
    }
}
