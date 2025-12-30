

using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class ComplaintCreateDto
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Severity { get; set; }
        public ComplaintType Type { get; set; }
        public Guid? GovernmentEntityId { get; set; }
         public SyrianGovernorate Governorate { get; set; }
        public decimal LocationLong { get; set; }
        public decimal LocationLat { get; set; }
        [JsonIgnore]
        public Guid? CitizenId { get; set; }
    }


}
