using Application.Common.Features.Admin.Dtos;
using Application.Common.Features.Admin.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers
{
    [Route("api/admin/agencies")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminAgenciesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileService _fileService;

        public AdminAgenciesController(IMediator mediator, IFileService fileService)
        {
            _mediator = mediator;
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AgencyCreateDto dto)
        {
            if (dto.Logo != null)
            {
                var url = await _fileService.SaveAsync(dto.Logo, "logos");
                dto.LogoUrl = url;
            }

            var created = await _mediator.Send(new CreateAgencyCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var agency = await _mediator.Send(new Application.Common.Features.Admin.Queries.GetAgencyByIdQuery(id));
            if (agency == null) return NotFound();
            return Ok(agency);
        }
    }
}
