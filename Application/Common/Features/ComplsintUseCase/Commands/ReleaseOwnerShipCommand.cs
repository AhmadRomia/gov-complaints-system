using Application.Common.Exceptions;
using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Features.ComplsintUseCase.Commands
{
    public record ReleaseOwnerShipCommand(Guid Id) : IRequest<ComplaintDetailsDto>;

    public class ReleaseOwnerShipCommandHandler(IComplaintService _service, ICurrentUserService _currentUser) : IRequestHandler<ReleaseOwnerShipCommand, ComplaintDetailsDto>
    {

        public async Task<ComplaintDetailsDto> Handle(ReleaseOwnerShipCommand request, CancellationToken cancellationToken)
        {
            if (!(_currentUser.IsAuthenticated && await _currentUser.IsInRoleAsync("Agency")))
                throw new BadRequestException("Only agency users can set status");

            var userId = _currentUser.UserId;
            return await _service.ReleasOwnerShip(request.Id, userId.Value);
        }
    }
}
