using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Features.ComplsintUseCase.Commands
{

    public record UpdateMyComplaintCommand(ComplaintUpdateDto Dto, List<IFormFile>? Attachments) : IRequest<Unit>;

    public class UpdateMyComplaintCommandHandler
        : IRequestHandler<UpdateMyComplaintCommand, Unit>
    {
        private readonly IComplaintService _complaintService;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        private readonly IRepository<Complaint> _repository;

        public UpdateMyComplaintCommandHandler(
            IComplaintService complaintService,
            ICurrentUserService currentUser,
            IMapper mapper,
            IRepository<Complaint> repository   )
        {
            _complaintService = complaintService;
            _currentUser = currentUser;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateMyComplaintCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                throw new BadRequestException("User not authenticated");

            if (!await _currentUser.IsInRoleAsync("Citizen"))
                throw new BadRequestException("Only citizens can update their complaints");

            var userGuid = _currentUser.UserId ?? throw new BadRequestException("User context is missing");

            var complaint = await _repository.FirstOrDefaultAsync(x=>x.Id == request.Dto.Id)
                            ?? throw new BadRequestException("Complaint not found");



            if (complaint.CitizenId != userGuid)
                throw new BadRequestException("Access denied to this complaint");

            if (complaint.Status != ComplaintStatus.New)
                throw new BadRequestException("Cannot update complaint after it has been assigned or processed");

            await _complaintService.UpdateWithFilesAsync(
                complaint,
                request.Dto,
                request.Attachments
            );

            return Unit.Value;
        }
    }

}
