using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Features.ComplsintUseCase;
using MediatR;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetComplaintByReferenceQuery(string ReferenceNumber) : IRequest<ComplaintDetailsDto?>;

    public class GetComplaintByReferenceQueryHandler : IRequestHandler<GetComplaintByReferenceQuery, ComplaintDetailsDto?>
    {
        private readonly IComplaintService _service;

        public GetComplaintByReferenceQueryHandler(IComplaintService service)
        {
            _service = service;
        }

        public async Task<ComplaintDetailsDto?> Handle(GetComplaintByReferenceQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetByReferenceAsync(request.ReferenceNumber);
        }
    }
}
