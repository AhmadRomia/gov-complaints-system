using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    [Route("api/admin")]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("users/{id}/activate")]
        public async Task<ActionResult<bool>> ActivateUser([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new Application.Common.Features.Users.Commands.ActivateUserCommand(id));
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("users/{id}/deactivate")]
        public async Task<ActionResult<bool>> DeactivateUser([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new Application.Common.Features.Users.Commands.DeactivateUserCommand(id));
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("agency-users/create")]
        public async Task<ActionResult<Application.Common.Features.Admin.Dtos.AgencyUserDto>> CreateAgencyUser([FromBody] Application.Common.Features.Admin.Dtos.AgencyUserCreateDto dto)
        {
            var created = await _mediator.Send(new Application.Common.Features.Admin.Commands.CreateAgencyUserCommand(dto));
            return Created($"api/admin/agency-users/{created.Id}", created);
        }

        [HttpGet("agency-users")]
        public async Task<ActionResult<List<Application.Common.Features.Admin.Dtos.AgencyUserDto>>> GetAgencyUsers([FromQuery] Guid? governmentEntityId)
        {
            var users = await _mediator.Send(new Application.Common.Features.Admin.Queries.GetAgencyUsersQuery(governmentEntityId));
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }

    }
}
