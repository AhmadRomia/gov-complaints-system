

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintCreateDto
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Severity { get; set; }
        public Guid CitizenId { get; set; }
        // Enum name string (ComplaintType) e.g. Infrastructure, Service, Other
        public string Type { get; set; } = default!;
        // Selected government entity id
        public Guid? GovernmentEntityId { get; set; }
        public string Location { get; set; } = default!;
    }

}
