using Application.Common.Features.ComplsintUseCase.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin,Agency")]
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("complaints")]
        public async Task<IActionResult> ExportComplaints([FromQuery] Domain.Enums.ExportPeriod? period)
        {
            var relativePath = await _mediator.Send(new ExportComplaintsQuery { Period = period });
            
            if (string.IsNullOrEmpty(relativePath))
            {
                return BadRequest("Could not generate export file.");
            }

            return Ok(relativePath);
        }
    }
}
