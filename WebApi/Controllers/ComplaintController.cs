using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Features.ComplsintUseCase.Commands;
using Application.Common.Features.ComplsintUseCase.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Data;
using Domain.Enums;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ComplaintController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency,Admin")]
        public async Task<IActionResult> GetAllForAgency() =>
            Ok(await _mediator.Send(new GetAllComplaintsQuery()));



        

        [HttpGet("by-entity/{entityId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Citizen")]
        public async Task<IActionResult> GetByEntityForAdmin(Guid entityId) =>
            Ok(await _mediator.Send(new GetComplaintsByEntityQuery(entityId)));

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetById(Guid id) =>
            Ok(await _mediator.Send(new GetComplaintByIdQuery(id)));

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Citizen")]
        [RequestSizeLimit(104_857_600)] 
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ComplaintCreateDto dto, [FromForm] List<IFormFile>? attachments)
        {
            var result = await _mediator.Send(new CreateComplaintCommand(dto, attachments));
            return Ok(result);
        }

        [HttpGet("my")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Citizen")]
        public async Task<IActionResult> GetMyComplaints()
        {
            var result = await _mediator.Send(new GetMyComplaintsQuery());
            return Ok(result);
        }

        [HttpGet("track/{reference}")]
        [AllowAnonymous]
        public async Task<IActionResult> TrackByReference(string reference)
        {
            var result = await _mediator.Send(new GetComplaintByReferenceQuery(reference));
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Citizen")]
        public async Task<IActionResult> Update([FromForm]ComplaintUpdateDto dto ,[FromForm] List<IFormFile>? attachments)
        {
            var r = await _mediator.Send(new UpdateMyComplaintCommand(dto,attachments));
            return Ok();
        }

        [HttpPut("status")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency,Admin")]
        public async Task<IActionResult> SetStatus([FromBody] SetComplaintStatusDto dto)
        {
            var result = await _mediator.Send(new SetComplaintStatusCommand(dto.Id, dto.Status, dto.AgencyNotes, dto.AdditionalInfoRequest));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        //[Authorize(Policy = "AdminPolicy")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var r = await _mediator.Send(new DeleteComplaintCommand(id));
            return Ok();
        }

        [HttpGet("paged")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency,Admin")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, int size = 10, [FromQuery] Guid? agencyId = null, [FromQuery] ComplaintStatus? complaintStatus = null) =>
            Ok(await _mediator.Send(new GetPagedComplaintsQuery(page, size, agencyId, complaintStatus)));

        // New endpoints for taking and releasing ownership
        [HttpPut("take-ownership/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency")]
        public async Task<IActionResult> TakeOwnership(Guid id)
        {
            var result = await _mediator.Send(new TakeOwnerShipCommand(id));
            return Ok(result);
        }

        [HttpPut("release-ownership/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency")]
        public async Task<IActionResult> ReleaseOwnership(Guid id)
        {
            var result = await _mediator.Send(new ReleaseOwnerShipCommand(id));
            return Ok(result);
        }

        [HttpPost("{id}/notes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency")]
        public async Task<IActionResult> AddAgencyNote(Guid id, [FromBody] AddAgencyNoteRequestDto dto)
        {
            var result = await _mediator.Send(new AddAgencyNoteCommand(id, dto.Note));
            return Ok(result);
        }

        [HttpPost("{id}/request-info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency")]
        public async Task<IActionResult> RequestAdditionalInfo(Guid id, [FromBody] RequestAdditionalInfoRequestDto dto)
        {
            var result = await _mediator.Send(new RequestAdditionalInfoCommand(id, dto.RequestMessage));
            return Ok(result);
        }

    }
}
