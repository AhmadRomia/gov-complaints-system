namespace Application.Common.Interfaces
{
    public interface ITelegramService
    {
        Task SendMessageAsync(string message);
        Task SendExceptionAsync(Exception ex, string? additionalInfo = null);
    }
}
