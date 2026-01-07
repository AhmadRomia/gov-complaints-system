

namespace Application.Common.Features.Users.DTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool IsVerified { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }

        public bool IsActive { get; set; }

    }
}
