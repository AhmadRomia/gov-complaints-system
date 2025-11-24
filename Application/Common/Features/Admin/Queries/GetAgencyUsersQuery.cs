using Application.Common.Features.Admin.Dtos;
using MediatR;

namespace Application.Common.Features.Admin.Queries
{
    public class GetAgencyUsersQuery : IRequest<List<AgencyUserDto>>
    {
        public Guid? GovernmentEntityId { get; set; }

        public GetAgencyUsersQuery(Guid? governmentEntityId = null)
        {
            GovernmentEntityId = governmentEntityId;
        }
    }
}
