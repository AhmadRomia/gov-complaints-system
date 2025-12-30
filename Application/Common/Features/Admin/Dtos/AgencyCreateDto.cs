using Microsoft.AspNetCore.Http;

namespace Application.Common.Features.Admin.Dtos
{
    public class AgencyCreateDto
    {
        public string Name { get; set; }
        public string? LogoUrl { get; set; }
        public IFormFile? Logo { get; set; }
    }
}
