

namespace Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Email { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
        Guid? GovernmentEntityId { get; }
        Task<bool> IsInRoleAsync(string role);
    }

}
