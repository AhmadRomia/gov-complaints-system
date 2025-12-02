using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetAllComplaintsQuery() : IRequest<List<ComplaintListDto>>;

    public class GetAllComplaintsQueryHandler : IRequestHandler<GetAllComplaintsQuery, List<ComplaintListDto>>
    {
        private readonly IRepository<Complaint> _repository;
        private readonly IMapper _mapper;

        public GetAllComplaintsQueryHandler(IRepository<Complaint> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ComplaintListDto>> Handle(GetAllComplaintsQuery request, CancellationToken cancellationToken)
        {
            var all = await _repository.GetAllAsync();
            return _mapper.Map<List<ComplaintListDto>>(all);
        }
    }
}
