using Domain.Common;


namespace Domain.Entities
{
    public class GovernmentEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public ICollection<ApplicationUser> Employees { get; set; } = new List<ApplicationUser>();
        public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();
    }
}
