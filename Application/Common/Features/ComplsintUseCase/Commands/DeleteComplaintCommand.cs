using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Common.Features.ComplsintUseCase.Commands
{
    public record DeleteComplaintCommand(Guid Id) : IRequest<Unit>;

    public class DeleteComplaintCommandHandler : IRequestHandler<DeleteComplaintCommand, Unit>
    {
        private readonly IRepository<Complaint> _repository;

        public DeleteComplaintCommandHandler(IRepository<Complaint> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteComplaintCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.FirstOrDefaultAsync(c => c.Id == request.Id);
            if (entity != null)
            {
                await _repository.DeleteAsync(entity);
            }
            return Unit.Value;
        }
    }
}
