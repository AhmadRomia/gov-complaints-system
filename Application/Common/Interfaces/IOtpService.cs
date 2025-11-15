using Domain.Entities;


namespace Application.Common.Interfaces
{
    public interface IOtpService
    {
        Task GenerateOtpAsync(ApplicationUser user);
    }
}
