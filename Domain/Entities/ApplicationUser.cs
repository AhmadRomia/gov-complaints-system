

using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiresAt { get; set; }
        public bool IsVerified { get; set; }

    }
}
