

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Severity { get; set; }
        public Guid CitizenId { get; set; }
    }

}
