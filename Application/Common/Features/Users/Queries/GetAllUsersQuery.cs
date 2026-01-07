using Application.Common.Features.Users.DTOs;
using Application.Common.Models;
using MediatR;


namespace Application.Common.Features.Users.Queries
{
    public class GetAllUsersQuery : IRequest<PagingResult<UserProfileDto>>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? SearchQuery { get; set; }
    }
}
