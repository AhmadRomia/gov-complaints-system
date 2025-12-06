using Application.Common.Features.Admin.Dtos;
using MediatR;

namespace Application.Common.Features.Admin.Queries
{
    public class GetAgencyByIdQuery : IRequest<AgencyDto?>
    {
        public Guid Id { get; }

        public GetAgencyByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
