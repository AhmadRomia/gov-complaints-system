

using Domain.Enums;

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Severity { get; set; }
        public ComplaintStatus Status { get; set; }
        public Guid CitizenId { get; set; }
        public required string ReferenceNumber { get; set; }
        public ComplaintType Type { get; set; } = default!;
        public Guid? GovernmentEntityId { get; set; }
       public decimal LocationLong { get; set; }

        public decimal LocationLat { get; set; }

        public SyrianGovernorate Governorate { get; set; }
        public List<AgencyNoteDto> AgencyNotes { get; set; } = new();
        public List<AdditionalInfoRequestDto> AdditionalInfoRequests { get; set; } = new();
        public List<AttachmentDto> Attachments { get; set; } = new();

        public Guid? LockedBy { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }

}
