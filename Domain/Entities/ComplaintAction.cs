using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{
    public class ComplaintAction : BaseEntity
    {
        public Guid ComplaintId { get; set; }
        public Complaint Complaint { get; set; } = default!;

        public ActionType ActionType { get; set; }
        public Guid IssuerId { get; set; }
        public string? Description { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
