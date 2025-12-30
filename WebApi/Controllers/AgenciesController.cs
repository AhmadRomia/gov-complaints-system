using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgenciesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AgenciesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Citizen")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new Application.Common.Features.Admin.Queries.GetAgenciesQuery());
            return Ok(result);
        }
    }
}
