namespace Application.Common.Features.Admin.Dtos
{
    public class AgencyUserCreateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid GovernmentEntityId { get; set; }
    }
}
