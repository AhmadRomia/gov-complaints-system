

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintUpdateDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Severity { get; set; }
        public string Status { get; set; } = default!;
        public string? AgencyNotes { get; set; }
        public string? AdditionalInfoRequest { get; set; }
    }

}
