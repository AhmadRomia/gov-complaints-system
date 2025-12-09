using Domain.Enums;

namespace WebApi.Controllers
{
    public class SetComplaintStatusDto
    {
        public Guid Id { get; set; }
        public ComplaintStatus Status { get; set; } = default!;
        public string? AgencyNotes { get; set; }
        public string? AdditionalInfoRequest { get; set; }
    }
}
