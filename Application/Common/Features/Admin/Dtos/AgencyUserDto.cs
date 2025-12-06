namespace Application.Common.Features.Admin.Dtos
{
    public class AgencyUserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Guid? GovernmentEntityId { get; set; }
    }
}
