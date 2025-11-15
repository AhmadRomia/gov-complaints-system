namespace Application.Common.Features.Auth.DTOs
{
    public class ConfirmOtpDto
    {
        // Accept either phone or email for flexibility
        public string Identifier { get; set; } // phone or email
        public string Code { get; set; }
    }
}