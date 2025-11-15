

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintDetailsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Severity { get; set; }
        public string Status { get; set; }
        public Guid CitizenId { get; set; }
    }

}
