

using Domain.Enums;

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ComplaintStatus Status { get; set; }
        public int Severity { get; set; }
        public string? GovernmentEntityName { get; set; }

        public string? CitizenName { get; set; }

        public string? CitizenEmail { get; set; }
        public string? CitizenPhoneNumber { get; set; }

        public SyrianGovernorate Governorate { get; set; }
        public decimal LocationLong { get; set; }
        public decimal LocationLat { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? LockedBy { get; set; }
    }


}
