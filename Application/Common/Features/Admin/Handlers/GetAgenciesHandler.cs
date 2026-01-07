using Application.Common.Features.Admin.Dtos;
using Application.Common.Features.Admin.Queries;
using Application.Common.Features.Admin;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Common.Features.Admin.Handlers
{
    public class GetAgenciesHandler : IRequestHandler<GetAgenciesQuery, List<AgencyDto>>
    {
        private readonly IAgencyService _service;
        private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;
        private const string CacheKey = "Agencies_GetAll";

        public GetAgenciesHandler(IAgencyService service, Microsoft.Extensions.Caching.Memory.IMemoryCache cache)
        {
            _service = service;
            _cache = cache;
        }

        public async Task<List<AgencyDto>> Handle(GetAgenciesQuery request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(CacheKey, out List<AgencyDto> agencies))
            {
                agencies = await _service.GetAllAsync();
                
                var cacheEntryOptions = new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions();
                     // 30 minutes sliding expiration

                _cache.Set(CacheKey, agencies, cacheEntryOptions);
            }

            return agencies;
        }
    }
}
