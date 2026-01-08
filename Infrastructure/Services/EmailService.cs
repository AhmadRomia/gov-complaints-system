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
            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("Recipient email 'to' must be provided", nameof(to));
            if (string.IsNullOrWhiteSpace(_settings.From))
                throw new InvalidOperationException("Email 'From' address is not configured");

            var message = new MimeMessage();
            // Parse addresses to validate format and avoid nulls
            var fromAddress = _settings.From;
            message.From.Add(new MailboxAddress("Government System", fromAddress));

            message.To.Add(new MailboxAddress(to, to));

            message.Subject = subject ?? string.Empty;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();

            var socketOptions = _settings.Port switch
            {
                587 => MailKit.Security.SecureSocketOptions.StartTls,
                465 => MailKit.Security.SecureSocketOptions.SslOnConnect,
                _ => _settings.EnableSsl
                        ? MailKit.Security.SecureSocketOptions.Auto
                        : MailKit.Security.SecureSocketOptions.None
            };

            await client.ConnectAsync(_settings.Host, _settings.Port, socketOptions);

            if (!string.IsNullOrWhiteSpace(_settings.UserName) && !string.IsNullOrWhiteSpace(_settings.Password))
            {
                await client.AuthenticateAsync(_settings.UserName, _settings.Password);
            }

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}
