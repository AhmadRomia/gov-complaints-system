using Domain.Enums;

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintVersionDto
    {
        public Guid Id { get; set; }
        public Guid ComplaintId { get; set; }
        public int VersionNumber { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Severity { get; set; }
        public ComplaintStatus Status { get; set; }
        public Guid CitizenId { get; set; }
        public string? ReferenceNumber { get; set; }
        public ComplaintType Type { get; set; }
        public decimal LocationLong { get; set; }
        public decimal LocationLat { get; set; }
        public Guid? GovernmentEntityId { get; set; }
        public SyrianGovernorate Governorate { get; set; }
        public Guid? LockedBy { get; set; }

        public string? ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
