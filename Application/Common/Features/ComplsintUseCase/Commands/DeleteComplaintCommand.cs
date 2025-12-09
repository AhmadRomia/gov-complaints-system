using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Common.Features.ComplsintUseCase.Commands
{
    public record DeleteComplaintCommand(Guid Id) : IRequest<Unit>;

    public class DeleteComplaintCommandHandler : IRequestHandler<DeleteComplaintCommand, Unit>
    {
        private readonly IRepository<Complaint> _repository;
        private readonly ICurrentUserService _currentUser;

        public DeleteComplaintCommandHandler(IRepository<Complaint> repository, ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(DeleteComplaintCommand request, CancellationToken cancellationToken)
        {
            if (!(_currentUser.IsAuthenticated && await _currentUser.IsInRoleAsync("Admin")))
                throw new BadRequestException("Only admins can delete complaints");
            var entity = await _repository.FirstOrDefaultAsync(c => c.Id == request.Id);
            if (entity != null)
            {
                await _repository.DeleteAsync(entity);
            }
            return Unit.Value;
        }
    }
}
