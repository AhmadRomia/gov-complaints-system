using Domain.Enums;

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintActionDto
    {
        public Guid Id { get; set; }
        public ActionType ActionType { get; set; }
        public Guid IssuerId { get; set; }
        public string? Description { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
