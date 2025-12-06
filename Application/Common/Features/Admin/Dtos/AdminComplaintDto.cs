using Domain.Enums;


namespace Application.Common.Features.Admin.Dtos
{
    public class AdminComplaintDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CitizenName { get; set; }
        public ComplaintStatus Status { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Agency { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
