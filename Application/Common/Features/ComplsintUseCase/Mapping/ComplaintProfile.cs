using Application.Common.Features.ComplsintUseCase.DTOs;
using AutoMapper;
using Domain.Entities;
using System.Text.Json;
using Domain.Enums;


namespace Application.Common.Features.ComplsintUseCase.Mapping
{
    public class ComplaintProfile : Profile
    {
        public ComplaintProfile()
        {
            CreateMap<ComplaintCreateDto, Complaint>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => MapType(s.Type)))
                .ForMember(d => d.Status, opt => opt.MapFrom(_ => ComplaintStatus.New));

            CreateMap<ComplaintUpdateDto, Complaint>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => MapStatus(s.Status)));

            CreateMap<Complaint, ComplaintListDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

            CreateMap<Domain.ValueObjects.Attachment, AttachmentDto>();

            CreateMap<Complaint, ComplaintDetailsDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type.ToString()))
                .ForMember(d => d.Attachments, opt => opt.MapFrom(s => s.Attachments));
        }

        private static ComplaintType MapType(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) return ComplaintType.Other;
            return Enum.TryParse<ComplaintType>(type, true, out var parsed) ? parsed : ComplaintType.Other;
        }

        private static ComplaintStatus MapStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return ComplaintStatus.New;
            return Enum.TryParse<ComplaintStatus>(status, true, out var parsed) ? parsed : ComplaintStatus.New;
        }
    }
}
