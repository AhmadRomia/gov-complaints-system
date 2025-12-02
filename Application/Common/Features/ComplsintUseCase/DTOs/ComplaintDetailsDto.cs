

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Severity { get; set; }
        public string Status { get; set; }
        public Guid CitizenId { get; set; }
        public string ReferenceNumber { get; set; }
        public string Type { get; set; } = default!;
        public Guid? GovernmentEntityId { get; set; }
        public string Location { get; set; } = default!;
        public string? AgencyNotes { get; set; }
        public string? AdditionalInfoRequest { get; set; }
        public List<AttachmentDto> Attachments { get; set; } = new();
    }

}
