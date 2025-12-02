using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetComplaintByIdQuery(Guid Id) : IRequest<ComplaintDetailsDto>;

    public class GetComplaintByIdQueryHandler : IRequestHandler<GetComplaintByIdQuery, ComplaintDetailsDto>
    {
        private readonly IRepository<Complaint> _repository;
        private readonly IMapper _mapper;

        public GetComplaintByIdQueryHandler(IRepository<Complaint> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ComplaintDetailsDto> Handle(GetComplaintByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.FirstOrDefaultAsync(c => c.Id == request.Id);
            return _mapper.Map<ComplaintDetailsDto>(entity);
        }
    }
}
