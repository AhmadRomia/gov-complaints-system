using Application.Common.Features.Admin.Dtos;
using Application.Common.Features.Admin.Queries;
using Application.Common.Features.Admin;
using MediatR;

namespace Application.Common.Features.Admin.Handlers
{
    public class GetAgencyByIdHandler : IRequestHandler<GetAgencyByIdQuery, AgencyDto?>
    {
        private readonly IAgencyService _service;

        public GetAgencyByIdHandler(IAgencyService service)
        {
            _service = service;
        }

        public async Task<AgencyDto?> Handle(GetAgencyByIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(request.Id);
        }
    }
}
