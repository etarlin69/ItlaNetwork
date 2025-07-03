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
            // Mapeo desde el modelo de infraestructura (ApplicationUser) al modelo de dominio (User)
            CreateMap<ApplicationUser, User>()
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap();

            // Mapeo desde el modelo de infraestructura (ApplicationUser) a la respuesta de autenticación (DTO)
            CreateMap<ApplicationUser, AuthenticationResponse>()
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ReverseMap();
        }
    }
}