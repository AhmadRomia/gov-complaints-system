using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class ComplaintVersion : BaseEntity
    {
        public Guid ComplaintId { get; set; }
        public Complaint Complaint { get; set; } = null!;
        public int VersionNumber { get; set; }

        public required string Title { get; set; }
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
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
