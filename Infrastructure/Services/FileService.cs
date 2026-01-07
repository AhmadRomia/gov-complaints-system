using Application.Common.Interfaces;
using Application.Common.Exceptions;
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
            var webRootPath = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");

            if (file.Length > 20 * 1024 * 1024)
                throw new BadRequestException("File too large");

            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!_allowedExtensions.Contains(ext))
                throw new BadRequestException("File type not allowed. Allowed types: jpg, jpeg, png, pdf");

            var rootPath = Path.Combine(webRootPath, folder);

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
            var webRootPath = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var fullPath = Path.Combine(webRootPath, fileUrl.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            await Task.CompletedTask;
        }

        public bool Exists(string fileUrl)
        {
            var webRootPath = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var fullPath = Path.Combine(webRootPath, fileUrl.TrimStart('/'));
            return File.Exists(fullPath);
        }
    }

}
