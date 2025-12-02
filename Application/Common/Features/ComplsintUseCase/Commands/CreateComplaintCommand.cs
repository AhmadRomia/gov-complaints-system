using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Features.ComplsintUseCase;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Features.ComplsintUseCase.Commands
{
    public record CreateComplaintCommand(ComplaintCreateDto Dto, List<IFormFile>? Attachments) : IRequest<ComplaintDetailsDto>;

    public class CreateComplaintCommandHandler : IRequestHandler<CreateComplaintCommand, ComplaintDetailsDto>
    {
        private readonly IComplaintService _service;

        public CreateComplaintCommandHandler(IComplaintService service)
        {
            _service = service;
        }

        public async Task<ComplaintDetailsDto> Handle(CreateComplaintCommand request, CancellationToken cancellationToken)
        {
            return await _service.CreateWithFilesAsync(request.Dto, request.Attachments);
        }
    }
}
