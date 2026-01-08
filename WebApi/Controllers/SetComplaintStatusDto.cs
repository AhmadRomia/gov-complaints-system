using Domain.Enums;

namespace WebApi.Controllers
{
    public class SetComplaintStatusDto
    {
        public Guid Id { get; set; }
        public ComplaintStatus Status { get; set; } = default!;
       
    }
}
