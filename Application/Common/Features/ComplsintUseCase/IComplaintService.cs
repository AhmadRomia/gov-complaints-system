using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Models;
using Domain.Entities;


namespace Application.Common.Features.ComplsintUseCase
{
    public interface IComplaintService
    : IGenericService<Complaint, ComplaintCreateDto, ComplaintUpdateDto, ComplaintListDto, ComplaintDetailsDto>
    {
        Task<ComplaintDetailsDto> CreateWithFilesAsync(ComplaintCreateDto dto, List<Microsoft.AspNetCore.Http.IFormFile>? attachments);
        Task<ComplaintDetailsDto?> GetByReferenceAsync(string referenceNumber);
        Task<ComplaintDetailsDto> SetStatusAsync(Guid id, string status, string? agencyNotes, string? additionalInfoRequest);
    }
}
