using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;


namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Government System", _settings.From));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(_settings.Host, _settings.Port, _settings.EnableSsl);

            await client.AuthenticateAsync(_settings.UserName, _settings.Password);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}
