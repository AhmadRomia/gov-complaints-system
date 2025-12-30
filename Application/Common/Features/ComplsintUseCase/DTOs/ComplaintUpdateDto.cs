

using Domain.Enums;

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintUpdateDto
    {
        public Guid Id { get; set; }             
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Severity { get; set; }
        public decimal LocationLong { get; set; }
        public decimal LocationLat { get; set; }
        public ComplaintType Type { get; set; }
        public Guid? GovernmentEntityId { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }


}
