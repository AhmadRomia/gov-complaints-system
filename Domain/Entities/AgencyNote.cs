using Domain.Common;

namespace Domain.Entities
{
    public class AgencyNote : BaseEntity
    {
        public Guid ComplaintId { get; set; }
        public required string Note { get; set; }
        public Complaint? Complaint { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
