using AutoMapper;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.Account;

namespace Ledgerly.API.Mappings
{
    public class AccountProfile : Profile
    {
        public AccountProfile() 
        {
            CreateMap<CreateAccountRequest, Account>();
            CreateMap<Account, CreateAccountResponse>();
            CreateMap<Account, GetAccountResponse>();
            CreateMap<UpdateAccountRequest, Account>();
            CreateMap<Account, UpdateAccountResponse>();
            CreateMap<Account, AccountsResponse>();
            CreateMap<Account, RecordAuditLogAccount>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.Memo, opt => opt.MapFrom(src => src.Memo))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate));
        }
    }
}
