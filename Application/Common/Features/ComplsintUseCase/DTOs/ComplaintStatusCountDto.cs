using Domain.Enums;

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintStatusCountDto
    {
        public ComplaintStatus Status { get; set; }
        public int Count { get; set; }
    }
}
