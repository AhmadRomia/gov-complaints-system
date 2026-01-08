using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public OtpService(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task GenerateOtpAsync(ApplicationUser user)
        {
            var otp = new Random().Next(100000, 999999).ToString();

            user.OtpCode = otp;
            user.OtpExpiresAt = DateTime.UtcNow.AddMinutes(5);

            await _userManager.UpdateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                var subject = "Your OTP Code";
                var body = $"<h2>Your OTP is: <b>{otp}</b></h2><p>This code expires in 5 minutes.</p>";
                try 
                {
                    await _emailService.SendEmailAsync(user.Email, subject, body);
                }
                catch
                {
                    // Swallow or log email exception to prevent crashing the OTP process
                }
            }
            else
            {
                Console.WriteLine($"OTP for {user.PhoneNumber}: {otp}");
            }
        }
    }
}
