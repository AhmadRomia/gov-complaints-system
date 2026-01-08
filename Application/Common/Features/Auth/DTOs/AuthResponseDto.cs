

using Domain.Enums;

namespace Application.Common.Features.Auth.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }


        public UserRoleEnum  UserRole { get; set; }
    }

}
