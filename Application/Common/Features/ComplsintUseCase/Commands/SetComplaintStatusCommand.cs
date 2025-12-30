using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Features.ComplsintUseCase;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Domain.Enums;

namespace Application.Common.Features.ComplsintUseCase.Commands
{
    public record SetComplaintStatusCommand(Guid Id, ComplaintStatus Status, string? AgencyNotes, string? AdditionalInfoRequest) : IRequest<ComplaintDetailsDto>;

    public class SetComplaintStatusCommandHandler : IRequestHandler<SetComplaintStatusCommand, ComplaintDetailsDto>
    {
        private readonly IComplaintService _service;
        private readonly ICurrentUserService _currentUser;

        public SetComplaintStatusCommandHandler(IComplaintService service, ICurrentUserService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        public async Task<ComplaintDetailsDto> Handle(SetComplaintStatusCommand request, CancellationToken cancellationToken)
        {
            if (!(_currentUser.IsAuthenticated && await _currentUser.IsInRoleAsync("Agency")) && !(_currentUser.IsAuthenticated && await _currentUser.IsInRoleAsync("Admin")))
                throw new BadRequestException("Only agency users can set status");

            var userId = _currentUser.UserId;
            return await _service.SetStatusAsync(request.Id,userId.Value, request.Status, request.AgencyNotes, request.AdditionalInfoRequest);
        }
    }
}
