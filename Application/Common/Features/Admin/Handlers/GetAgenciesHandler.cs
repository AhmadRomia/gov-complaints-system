using Application.Common.Features.Admin.Dtos;
using Application.Common.Features.Admin.Queries;
using Application.Common.Features.Admin;
using MediatR;

namespace Application.Common.Features.Admin.Handlers
{
    public class GetAgenciesHandler : IRequestHandler<GetAgenciesQuery, List<AgencyDto>>
    {
        private readonly IAgencyService _service;

        public GetAgenciesHandler(IAgencyService service)
        {
            _service = service;
        }

        public async Task<List<AgencyDto>> Handle(GetAgenciesQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync();
        }
    }
}
