namespace Application.Common.Features.ComplsintUseCase.DTOs
{
    public class AttachmentDto
    {
        public string FileName { get; set; } = default!;
        public string FileUrl { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public long FileSize { get; set; }
    }
}
