

using Domain.Enums;

namespace Application.Common.Features.Auth.DTOs
{
    public class RegisterDto
    {
        public required string FullName { get; set; }
        public  string? Email { get; set; }

        public  string? Phone { get; set; }
        public required string Password { get; set; }

        public UserRoleEnum UserRole { get; set; }

    }

}
