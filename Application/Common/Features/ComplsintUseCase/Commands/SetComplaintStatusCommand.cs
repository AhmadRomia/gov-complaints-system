using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Features.ComplsintUseCase;
using MediatR;

namespace Application.Common.Features.ComplsintUseCase.Commands
{
    public record SetComplaintStatusCommand(Guid Id, string Status, string? AgencyNotes, string? AdditionalInfoRequest) : IRequest<ComplaintDetailsDto>;

    public class SetComplaintStatusCommandHandler : IRequestHandler<SetComplaintStatusCommand, ComplaintDetailsDto>
    {
        private readonly IComplaintService _service;

        public SetComplaintStatusCommandHandler(IComplaintService service)
        {
            _service = service;
        }

        public async Task<ComplaintDetailsDto> Handle(SetComplaintStatusCommand request, CancellationToken cancellationToken)
        {
            return await _service.SetStatusAsync(request.Id, request.Status, request.AgencyNotes, request.AdditionalInfoRequest);
        }
    }
}
