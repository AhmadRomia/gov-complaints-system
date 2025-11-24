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

        public CreateAgencyHandler(IAgencyService service)
        {
            _service = service;
        }

        public async Task<AgencyDto> Handle(CreateAgencyCommand request, CancellationToken cancellationToken)
        {
            var name = request.Dto?.Name?.Trim();
            if (string.IsNullOrEmpty(name))
                throw new BadRequestException("Agency name is required");

            var exists = await _service.AnyAsync(x => x.Name == name);
            if (exists)
                throw new BadRequestException("Agency with the same name already exists");

            return await _service.CreateAsync(new AgencyCreateDto { Name = name });
        }
    }
}
