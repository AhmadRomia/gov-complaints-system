using Domain.Entities;


namespace Application.Common.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
