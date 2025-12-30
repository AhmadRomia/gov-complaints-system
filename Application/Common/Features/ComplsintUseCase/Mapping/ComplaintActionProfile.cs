using Application.Common.Features.ComplsintUseCase.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Features.ComplsintUseCase.Mapping
{
    public class ComplaintActionProfile : Profile
    {
        public ComplaintActionProfile()
        {
            CreateMap<ComplaintAction, ComplaintActionDto>();
        }
    }
}
