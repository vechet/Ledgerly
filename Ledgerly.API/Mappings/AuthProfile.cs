using AutoMapper;
using Ledgerly.API.Models.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace Ledgerly.API.Mappings
{
    public class AuthProfile : Profile
    {
        public AuthProfile() 
        {
            CreateMap<RegisterRequest, IdentityUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));
            CreateMap<IdentityUser, RegisterResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));

        }
    }
}
