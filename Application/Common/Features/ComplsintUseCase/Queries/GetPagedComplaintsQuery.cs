using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetPagedComplaintsQuery(int Page, int Size) : IRequest<List<ComplaintListDto>>;

    public class GetPagedComplaintsQueryHandler : IRequestHandler<GetPagedComplaintsQuery, List<ComplaintListDto>>
    {
        private readonly IRepository<Complaint> _repository;
        private readonly IMapper _mapper;

        public GetPagedComplaintsQueryHandler(IRepository<Complaint> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ComplaintListDto>> Handle(GetPagedComplaintsQuery request, CancellationToken cancellationToken)
        {
            var page = await _repository.GetPagedAsync(request.Page, request.Size);
            return _mapper.Map<List<ComplaintListDto>>(page);
        }
    }
}
