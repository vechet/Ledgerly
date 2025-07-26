using AutoMapper;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.TransactionType;
using Ledgerly.API.Models.DTOs.User;
using Microsoft.AspNetCore.Identity;

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
            CreateMap<RegisterRequest, IdentityUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));
            CreateMap<IdentityUser, RegisterResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));
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
