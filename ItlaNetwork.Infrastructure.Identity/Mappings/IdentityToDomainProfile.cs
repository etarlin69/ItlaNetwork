using AutoMapper;
using ItlaNetwork.Core.Application.DTOs.Account;
using ItlaNetwork.Core.Domain.Entities;
using ItlaNetwork.Infrastructure.Identity.Models;

namespace ItlaNetwork.Infrastructure.Identity.Mappings
{
    public class IdentityToDomainProfile : Profile
    {
        public IdentityToDomainProfile()
        {
            
            CreateMap<ApplicationUser, User>()
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap();

            
            CreateMap<ApplicationUser, AuthenticationResponse>()
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ReverseMap();
        }
    }
}