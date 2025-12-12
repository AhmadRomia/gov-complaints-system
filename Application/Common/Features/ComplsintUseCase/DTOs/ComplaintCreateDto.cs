

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
        public string Location { get; set; } = default!;
        [JsonIgnore]
        public Guid? CitizenId { get; set; }
    }


}
