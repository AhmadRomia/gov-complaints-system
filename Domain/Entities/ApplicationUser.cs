using Microsoft.AspNetCore.Identity;
using Domain.Enums;
namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public required string FullName { get; set; }
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiresAt { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; } = true;

        public Guid? GovernmentEntityId { get; set; }
        public GovernmentEntity? GovernmentEntity { get; set; }
        
        public  UserRoleEnum  UserRole { get; set; }

    }
}
