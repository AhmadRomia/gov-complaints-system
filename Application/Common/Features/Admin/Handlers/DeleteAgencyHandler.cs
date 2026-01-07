using Application.Common.Features.Admin;
using Application.Common.Features.Admin.Commands;
using MediatR;

namespace Application.Common.Features.Admin.Handlers
{
    public class DeleteAgencyHandler : IRequestHandler<DeleteAgencyCommand, bool>
    {
        private readonly IAgencyService _service;
        private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;
        private const string CacheKey = "Agencies_GetAll";

        public DeleteAgencyHandler(IAgencyService service, Microsoft.Extensions.Caching.Memory.IMemoryCache cache)
        {
            _service = service;
            _cache = cache;
        }

        public async Task<bool> Handle(DeleteAgencyCommand request, CancellationToken cancellationToken)
        {
            var existing = await _service.GetByIdAsync(request.Id);
            if (existing == null)
                return false;

            await _service.DeleteAsync(request.Id);
            _cache.Remove(CacheKey);
            return true;
        }
    }
}
