namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class AdditionalInfoRequestDto
    {
        public string RequestMessage { get; set; } = default!;
        public DateTime? CreatedAt { get; set; }
    }
}
