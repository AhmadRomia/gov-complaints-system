

using Domain.Enums;

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ComplaintStatus Status { get; set; }
        public int Severity { get; set; }
        public string? GovernmentEntityName { get; set; }
        public DateTime CreatedOn { get; set; }
    }


}
