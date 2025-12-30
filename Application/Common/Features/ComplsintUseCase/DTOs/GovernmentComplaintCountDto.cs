namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class AgencyComplaintCountDto
    {
        public Guid GovernmentEntityId { get; set; }
        public string? GovernmentEntityName { get; set; }
        public int Count { get; set; }
    }
}
