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
                .ForMember(d => d.Status, opt => opt.MapFrom(_ => ComplaintStatus.New));

            CreateMap<ComplaintUpdateDto, Complaint>();
            CreateMap<ComplaintUpdateDto, ComplaintDetailsDto>().ReverseMap();

            CreateMap<Complaint, ComplaintListDto>()
      .ForMember(d => d.GovernmentEntityName,
          opt => opt.MapFrom(s => s.GovernmentEntity != null ? s.GovernmentEntity.Name : null))

      .ForMember(d => d.CitizenName,
          opt => opt.MapFrom(s => s.Citizen != null ? s.Citizen.FullName : null))

      .ForMember(d => d.CitizenPhoneNumber,
          opt => opt.MapFrom(s => s.Citizen != null ? s.Citizen.PhoneNumber : null))

      .ForMember(d => d.CitizenEmail,
          opt => opt.MapFrom(s => s.Citizen != null ? s.Citizen.Email : null));
            ;

            CreateMap<Domain.ValueObjects.Attachment, AttachmentDto>();

            // Map enums directly (was incorrectly mapping to string)
            // Map enums directly (was incorrectly mapping to string)
            CreateMap<Complaint, ComplaintDetailsDto>();

            CreateMap<AgencyNote, AgencyNoteDto>();
            CreateMap<AdditionalInfoRequest, AdditionalInfoRequestDto>();
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
