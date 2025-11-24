using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Common.Features.ComplsintUseCase.Commands
{
    public record UpdateComplaintCommand(ComplaintUpdateDto Dto) : IRequest<Unit>;

    public class UpdateComplaintCommandHandler : IRequestHandler<UpdateComplaintCommand, Unit>
    {
        private readonly IRepository<Complaint> _repository;
        private readonly IMapper _mapper;

        public UpdateComplaintCommandHandler(IRepository<Complaint> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateComplaintCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Complaint>(request.Dto);
            await _repository.UpdateAsync(entity);
            return Unit.Value;
        }
    }
}
