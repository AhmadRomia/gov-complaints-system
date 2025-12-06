using Application.Common.Features.Admin;
using Application.Common.Features.Admin.Commands;
using MediatR;

namespace Application.Common.Features.Admin.Handlers
{
    public class DeleteAgencyHandler : IRequestHandler<DeleteAgencyCommand, bool>
    {
        private readonly IAgencyService _service;

        public DeleteAgencyHandler(IAgencyService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(DeleteAgencyCommand request, CancellationToken cancellationToken)
        {
            var existing = await _service.GetByIdAsync(request.Id);
            if (existing == null)
                return false;

            await _service.DeleteAsync(request.Id);
            return true;
        }
    }
}
