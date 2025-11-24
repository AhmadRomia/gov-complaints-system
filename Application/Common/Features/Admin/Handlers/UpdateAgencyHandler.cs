using Application.Common.Exceptions;
using Application.Common.Features.Admin;
using Application.Common.Features.Admin.Commands;
using Application.Common.Features.Admin.Dtos;
using MediatR;

namespace Application.Common.Features.Admin.Handlers
{
    public class UpdateAgencyHandler : IRequestHandler<UpdateAgencyCommand, bool>
    {
        private readonly IAgencyService _service;

        public UpdateAgencyHandler(IAgencyService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(UpdateAgencyCommand request, CancellationToken cancellationToken)
        {
            var id = request.Dto.Id;
            var name = request.Dto?.Name?.Trim();
            if (id == Guid.Empty)
                throw new BadRequestException("Invalid agency id");
            if (string.IsNullOrEmpty(name))
                throw new BadRequestException("Agency name is required");

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return false;

            var duplicate = await _service.AnyAsync(x => x.Name == name && x.Id != id);
            if (duplicate)
                throw new BadRequestException("Another agency with the same name exists");

            await _service.UpdateAsync(new AgencyUpdateDto { Id = id, Name = name });
            return true;
        }
    }
}
