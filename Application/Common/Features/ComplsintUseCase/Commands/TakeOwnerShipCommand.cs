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
    public record TakeOwnerShipCommand(Guid Id) : IRequest<ComplaintDetailsDto>;

    public class TakeOwnerShipCommandHandler(IComplaintService _service, ICurrentUserService _currentUser) : IRequestHandler<TakeOwnerShipCommand, ComplaintDetailsDto>
    {
     
        public async Task<ComplaintDetailsDto> Handle(TakeOwnerShipCommand request, CancellationToken cancellationToken)
        {
            if (!(_currentUser.IsAuthenticated && await _currentUser.IsInRoleAsync("Agency")))
                throw new BadRequestException("Only agency users can set status");

            var userId = _currentUser.UserId;
            return await _service.TakeOwnerShip(request.Id,userId.Value);
        }
    }
}
