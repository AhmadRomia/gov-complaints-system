using Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveAsync(IFormFile file, string folder = "attachments")
        {
            if (_env.WebRootPath is null)
                throw new Exception("WebRootPath not found");

            if (file.Length > 20 * 1024 * 1024)
                throw new Exception("File too large");

            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!_allowedExtensions.Contains(ext))
                throw new Exception("File type not allowed");

            var rootPath = Path.Combine(_env.WebRootPath, folder);

            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var fileName = Path.GetRandomFileName() + ext;
            var filePath = Path.Combine(rootPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folder}/{fileName}";
        }

        public async Task DeleteAsync(string fileUrl)
        {
            var fullPath = Path.Combine(_env.WebRootPath, fileUrl.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            await Task.CompletedTask;
        }

        public bool Exists(string fileUrl)
        {
            var fullPath = Path.Combine(_env.WebRootPath, fileUrl.TrimStart('/'));
            return File.Exists(fullPath);
        }
    }

}
