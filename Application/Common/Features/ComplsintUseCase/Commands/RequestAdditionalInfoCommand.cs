using Application.Common.Exceptions;
using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Features.ComplsintUseCase.Commands
{
    public record RequestAdditionalInfoCommand(Guid ComplaintId, string InfoRequest) : IRequest<ComplaintDetailsDto>;

    public class RequestAdditionalInfoCommandHandler : IRequestHandler<RequestAdditionalInfoCommand, ComplaintDetailsDto>
    {
        private readonly IComplaintService _service;
        private readonly ICurrentUserService _currentUser;

        public RequestAdditionalInfoCommandHandler(IComplaintService service, ICurrentUserService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        public async Task<ComplaintDetailsDto> Handle(RequestAdditionalInfoCommand request, CancellationToken cancellationToken)
        {
             if (!(_currentUser.IsAuthenticated && await _currentUser.IsInRoleAsync("Agency")))
                throw new BadRequestException("Only agency users can request info");

            var userId = _currentUser.UserId ?? throw new BadRequestException("User ID not found");
            return await _service.RequestAdditionalInfo(request.ComplaintId, userId, request.InfoRequest);
        }
    }
}
