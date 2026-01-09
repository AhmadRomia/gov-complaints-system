using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetComplaintVersionsQuery(Guid ComplaintId) : IRequest<List<ComplaintVersionDto>>;

    public class GetComplaintVersionsQueryHandler : IRequestHandler<GetComplaintVersionsQuery, List<ComplaintVersionDto>>
    {
        private readonly IRepository<ComplaintVersion> _repository;
        private readonly IMapper _mapper;

        public GetComplaintVersionsQueryHandler(IRepository<ComplaintVersion> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ComplaintVersionDto>> Handle(GetComplaintVersionsQuery request, CancellationToken cancellationToken)
        {
            var versions = await _repository.GetAllAsNoTrackingAsync(
                filter: v => v.ComplaintId == request.ComplaintId,
                orderBy: q => q.OrderByDescending(v => v.VersionNumber)
            );

            return _mapper.Map<List<ComplaintVersionDto>>(versions);
        }
    }
}
