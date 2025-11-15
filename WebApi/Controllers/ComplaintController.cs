using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Features.ComplsintUseCase;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _service;

        public ComplaintController(IComplaintService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) =>
            Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(ComplaintCreateDto dto) =>
            Ok(await _service.CreateAsync(dto));

        [HttpPut]
        public async Task<IActionResult> Update(ComplaintUpdateDto dto)
        {
            await _service.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, int size = 10) =>
            Ok(await _service.GetPagedAsync(page, size));
    }
}
