using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net;

namespace Infrastructure.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly HttpClient _httpClient;
        private readonly TelegramSettings _settings;
        private readonly ILogger<TelegramService> _logger;

        public TelegramService(HttpClient httpClient, IOptions<TelegramSettings> settings, ILogger<TelegramService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendMessageAsync(string message)
        {
            if (string.IsNullOrEmpty(_settings.BotToken) || string.IsNullOrEmpty(_settings.ChatId))
            {
                return;
            }

            var url = $"https://api.telegram.org/bot{_settings.BotToken}/sendMessage";
            var payload = new
            {
                chat_id = _settings.ChatId,
                text = message,                parse_mode = "HTML"
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
            
            try
            {
                var response = await _httpClient.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to send Telegram message. Status: {StatusCode}, Error: {Error}", response.StatusCode, error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while sending Telegram message");
            }
        }

        public async Task SendExceptionAsync(Exception ex, string? additionalInfo = null)
        {
            var message = $"<b>🚨 Unhandled Exception</b>\n\n" +
                          $"<b>Message:</b> <code>{WebUtility.HtmlEncode(ex.Message)}</code>\n" +
                          $"<b>Type:</b> <code>{ex.GetType().Name}</code>\n";

            if (!string.IsNullOrEmpty(additionalInfo))
            {
                message += $"\n<b>📍 Context:</b>\n{WebUtility.HtmlEncode(additionalInfo)}\n";
            }

            var stackTrace = ex.StackTrace ?? string.Empty;
            if (stackTrace.Length > 500)
            {
                stackTrace = stackTrace.Substring(0, 500) + "...";
            }

            message += $"\n<b>🔍 Stack Trace (Snippet):</b>\n<code>{WebUtility.HtmlEncode(stackTrace)}</code>";

            await SendMessageAsync(message);
        }
    }
}
