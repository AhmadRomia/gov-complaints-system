using Domain.Common;


namespace Domain.Entities
{
    public class Complaint : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Severity { get; set; }
        public string Status { get; set; } = "Pending";

        public Guid CitizenId { get; set; }
    }
}
