using Application.Common.Exceptions;
using Application.Common.Features.Admin;
using Application.Common.Features.Admin.Commands;
using Application.Common.Features.Admin.Dtos;
using MediatR;

namespace Application.Common.Features.Admin.Handlers
{
    public class CreateAgencyHandler : IRequestHandler<CreateAgencyCommand, AgencyDto>
    {
        private readonly IAgencyService _service;
        private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;
        private const string CacheKey = "Agencies_GetAll";

        public CreateAgencyHandler(IAgencyService service, Microsoft.Extensions.Caching.Memory.IMemoryCache cache)
        {
            _service = service;
            _cache = cache;
        }

        public async Task<AgencyDto> Handle(CreateAgencyCommand request, CancellationToken cancellationToken)
        {
            var name = request.Dto?.Name?.Trim();
            if (string.IsNullOrEmpty(name))
                throw new BadRequestException("Agency name is required");

            var exists = await _service.AnyAsync(x => x.Name == name);
            if (exists)
                throw new BadRequestException("Agency with the same name already exists");

            var createDto = new AgencyCreateDto { Name = name, LogoUrl = request.Dto.LogoUrl };
            var result = await _service.CreateAsync(createDto);
            
            _cache.Remove(CacheKey);
            
            return result;
        }
    }
}
