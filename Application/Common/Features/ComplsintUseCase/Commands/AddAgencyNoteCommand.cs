using Application.Common.Exceptions;
using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Features.ComplsintUseCase.Commands
{
    public record AddAgencyNoteCommand(Guid ComplaintId, string Note) : IRequest<ComplaintDetailsDto>;

    public class AddAgencyNoteCommandHandler : IRequestHandler<AddAgencyNoteCommand, ComplaintDetailsDto>
    {
        private readonly IComplaintService _service;
        private readonly ICurrentUserService _currentUser;

        public AddAgencyNoteCommandHandler(IComplaintService service, ICurrentUserService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        public async Task<ComplaintDetailsDto> Handle(AddAgencyNoteCommand request, CancellationToken cancellationToken)
        {
            if (!(_currentUser.IsAuthenticated && await _currentUser.IsInRoleAsync("Agency")))
                throw new BadRequestException("Only agency users can add notes");

            var userId = _currentUser.UserId ?? throw new BadRequestException("User ID not found");
            return await _service.AddAgencyNote(request.ComplaintId, userId, request.Note);
        }
    }
}
