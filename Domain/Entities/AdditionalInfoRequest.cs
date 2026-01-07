using Domain.Common;

namespace Domain.Entities
{
    public class AdditionalInfoRequest : BaseEntity
    {
        public Guid ComplaintId { get; set; }
        public required string RequestMessage { get; set; }
        public Complaint? Complaint { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
