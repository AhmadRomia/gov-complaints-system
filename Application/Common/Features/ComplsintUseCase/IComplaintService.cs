using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Models;
using Domain.Entities;


namespace Application.Common.Features.ComplsintUseCase
{
    public interface IComplaintService
    : IGenericService<Complaint, ComplaintCreateDto, ComplaintUpdateDto, ComplaintListDto, ComplaintDetailsDto>
    {
    }
}
