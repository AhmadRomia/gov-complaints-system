using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Features.ComplsintUseCase.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnalyticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("status-counts")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency,Admin")]
        public async Task<IActionResult> GetStatusCounts([FromQuery] Guid? governmentEntityId = null)
        {
            var result = await _mediator.Send(new GetComplaintStatusCountsQuery(governmentEntityId));
            // return as list of objects (Status and Count)
            return Ok(result);
        }

        [HttpGet("by-agency")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency,Admin")]
        public async Task<IActionResult> GetCountsByAgency()
        {
            var result = await _mediator.Send(new GetComplaintCountsByAgencyQuery());
            return Ok(result);
        }

        [HttpGet("by-governorate")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency,Admin")]
        public async Task<IActionResult> GetCountsByGovernorate()
        {
            var result = await _mediator.Send(new GetComplaintCountsByGovernorateQuery());
            return Ok(result);
        }
    }
}
