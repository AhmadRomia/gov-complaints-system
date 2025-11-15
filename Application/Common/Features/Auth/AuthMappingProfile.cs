using Application.Common.Features.Auth.DTOs;
using AutoMapper;
using Domain.Entities;


namespace Application.Common.Features.Auth
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>();
        }
    }
}
