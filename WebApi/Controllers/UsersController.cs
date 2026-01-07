using Application.Common.Features.Users.Commands;
using Application.Common.Features.Users.DTOs;
using Application.Common.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await _mediator.Send(new GetMyProfileQuery());
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var r = await _mediator.Send(new UpdateProfileCommand(dto));
            return r ? Ok() : BadRequest();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string? searchQuery = null)
        {
            var users = await _mediator.Send(new GetAllUsersQuery { Page = page, Size = size, SearchQuery = searchQuery });
            return Ok(users);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));
            return user != null ? Ok(user) : NotFound();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var r = await _mediator.Send(new DeactivateUserCommand(id));
            return r ? Ok() : NotFound();
        }
    }
}
