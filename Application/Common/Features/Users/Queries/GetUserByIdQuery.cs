using Application.Common.Features.Users.DTOs;
using MediatR;

namespace Application.Common.Features.Users.Queries
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<UserProfileDto?>;
}
