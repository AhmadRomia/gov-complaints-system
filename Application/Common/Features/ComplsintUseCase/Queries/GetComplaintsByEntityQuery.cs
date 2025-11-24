using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Features.ComplsintUseCase;
using MediatR;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetComplaintsByEntityQuery(Guid GovernmentEntityId) : IRequest<List<ComplaintListDto>>;

    public class GetComplaintsByEntityQueryHandler : IRequestHandler<GetComplaintsByEntityQuery, List<ComplaintListDto>>
    {
        private readonly IComplaintService _service;
        public GetComplaintsByEntityQueryHandler(IComplaintService service)
        {
            _service = service;
        }

        public async Task<List<ComplaintListDto>> Handle(GetComplaintsByEntityQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(c => c.GovernmentEntityId == request.GovernmentEntityId);
        }
    }
}
