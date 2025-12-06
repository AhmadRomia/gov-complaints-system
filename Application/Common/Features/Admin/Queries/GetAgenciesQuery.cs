using Application.Common.Features.Admin.Dtos;
using MediatR;

namespace Application.Common.Features.Admin.Queries
{
    public class GetAgenciesQuery : IRequest<List<AgencyDto>>
    {
    }
}
