using Application.Common.Features.ComplsintUseCase.DTOs;
using AutoMapper;
using Domain.Entities;


namespace Application.Common.Features.ComplsintUseCase.Mapping
{
    public class ComplaintProfile : Profile
    {
        public ComplaintProfile()
        {
            CreateMap<ComplaintCreateDto, Complaint>();
            CreateMap<ComplaintUpdateDto, Complaint>();

            CreateMap<Complaint, ComplaintListDto>();
            CreateMap<Complaint, ComplaintDetailsDto>();
        }
    }
}
