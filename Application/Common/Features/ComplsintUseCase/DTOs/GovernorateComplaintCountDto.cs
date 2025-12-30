using Domain.Enums;

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class GovernorateComplaintCountDto
    {
        public SyrianGovernorate Governorate { get; set; }
        public int Count { get; set; }
    }
}
