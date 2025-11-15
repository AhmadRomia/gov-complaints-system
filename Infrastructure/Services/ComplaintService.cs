using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Features.ComplsintUseCase;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;


namespace Infrastructure.Services
{
    public class ComplaintService
        : GenericService<Complaint, ComplaintCreateDto, ComplaintUpdateDto, ComplaintListDto, ComplaintDetailsDto>,
          IComplaintService
    {
        public ComplaintService(IRepository<Complaint> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }

        public async Task ApproveComplaint(Guid id)
        {
            var complaint = await _repository.FirstOrDefaultAsync(c => c.Id == id);
            complaint.Status = "Approved";
            await _repository.UpdateAsync(complaint);
        }
    }
}
