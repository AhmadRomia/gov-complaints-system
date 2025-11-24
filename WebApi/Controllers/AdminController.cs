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
        [HttpGet("agencies")]
        public async Task<ActionResult<List<Application.Common.Features.Admin.Dtos.AgencyDto>>> GetAgencies()
        {
            var agencies = await _mediator.Send(new Application.Common.Features.Admin.Queries.GetAgenciesQuery());
            return Ok(agencies);
        }

        [HttpGet("agencies/{id}")]
        public async Task<ActionResult<Application.Common.Features.Admin.Dtos.AgencyDto>> GetAgencyById([FromRoute] Guid id)
        {
            var agency = await _mediator.Send(new Application.Common.Features.Admin.Queries.GetAgencyByIdQuery(id));
            if (agency == null) return NotFound();
            return Ok(agency);
        }

        [HttpPost("agencies")]
        public async Task<ActionResult<Application.Common.Features.Admin.Dtos.AgencyDto>> CreateAgency([FromBody] Application.Common.Features.Admin.Dtos.AgencyCreateDto dto)
        {
            var created = await _mediator.Send(new Application.Common.Features.Admin.Commands.CreateAgencyCommand(dto));
            return Created($"api/admin/agencies/{created.Id}", created);
        }

        [HttpPut("agencies/{id}")]
        public async Task<IActionResult> UpdateAgency( [FromBody] Application.Common.Features.Admin.Dtos.AgencyUpdateDto dto)
        {
            var result = await _mediator.Send(new Application.Common.Features.Admin.Commands.UpdateAgencyCommand( dto));
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("agencies/{id}")]
        public async Task<IActionResult> DeleteAgency([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new Application.Common.Features.Admin.Commands.DeleteAgencyCommand(id));
            if (!result) return NotFound();
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }

    }
}
