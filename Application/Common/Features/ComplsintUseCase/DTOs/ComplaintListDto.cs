

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public int Severity { get; set; }
    }

}
