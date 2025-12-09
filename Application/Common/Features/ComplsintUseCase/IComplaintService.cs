using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;


namespace Application.Common.Features.ComplsintUseCase
{
    public interface IComplaintService
    : IGenericService<Complaint, ComplaintCreateDto, ComplaintUpdateDto, ComplaintListDto, ComplaintDetailsDto>
    {
        Task<ComplaintDetailsDto> CreateWithFilesAsync(ComplaintCreateDto dto, List<Microsoft.AspNetCore.Http.IFormFile>? attachments);
        Task<ComplaintDetailsDto?> GetByReferenceAsync(string referenceNumber);
        Task<ComplaintDetailsDto> SetStatusAsync(Guid id, ComplaintStatus status, string? agencyNotes, string? additionalInfoRequest);
        Task<ComplaintDetailsDto> UpdateWithFilesAsync(
    Complaint complaint,
    ComplaintUpdateDto dto,
    List<IFormFile>? newFiles);
    }
}
