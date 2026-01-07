using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Features.ComplsintUseCase.Commands
{
    public record CreateComplaintCommand(ComplaintCreateDto Dto, List<IFormFile>? Attachments) : IRequest<ComplaintDetailsDto>;

    public class CreateComplaintCommandHandler : IRequestHandler<CreateComplaintCommand, ComplaintDetailsDto>
    {
        private readonly IComplaintService _service;
        private readonly ICurrentUserService _currentUser;

        public CreateComplaintCommandHandler(IComplaintService service, ICurrentUserService currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        public async Task<ComplaintDetailsDto> Handle(CreateComplaintCommand request, CancellationToken cancellationToken)
        {
            try 
            {
                if (!(_currentUser.IsAuthenticated && await _currentUser.IsInRoleAsync("Citizen")))
                    throw new BadRequestException("Only citizens can create complaints");

                var dto = request.Dto;
                var userId = _currentUser.UserId ?? throw new BadRequestException("User context is missing");

                dto.CitizenId = userId;

                if (dto.GovernmentEntityId == null)
                    throw new BadRequestException("Government entity is required");


                if (request.Attachments?.Count > 10)
                    throw new BadRequestException("Maximum 10 files allowed");

                foreach (var file in request.Attachments ?? new List<IFormFile>())
                {
                    if (file.Length > 10 * 1024 * 1024)
                        throw new BadRequestException("Each file must be under 10MB");

                    var ext = Path.GetExtension(file.FileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
                    if (!allowedExtensions.Contains(ext))
                        throw new BadRequestException($"File type '{ext}' not allowed. Allowed types: jpg, jpeg, png, pdf");
                }

                return await _service.CreateWithFilesAsync(dto, request.Attachments);
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"DEBUG INFO: {ex.Message} \n {ex.StackTrace}");
            }
        }

    }
}
