using Application.Common.Features.Users.DTOs;
using MediatR;


namespace Application.Common.Features.Users.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserProfileDto>>
    {
    }
}
