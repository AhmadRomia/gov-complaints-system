using Microsoft.AspNetCore.Http;


namespace Application.Common.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveAsync(IFormFile file, string folder = "attachments");
        Task DeleteAsync(string fileUrl);
        bool Exists(string fileUrl);
    }
}
