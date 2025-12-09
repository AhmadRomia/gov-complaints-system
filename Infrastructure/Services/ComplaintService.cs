using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Features.ComplsintUseCase;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;


namespace Infrastructure.Services
{
    public class ComplaintService
        : GenericService<Complaint, ComplaintCreateDto, ComplaintUpdateDto, ComplaintListDto, ComplaintDetailsDto>,
          IComplaintService
    {
        private readonly IFileService _fileService;

        public ComplaintService(IRepository<Complaint> repository, IMapper mapper, IFileService fileService)
            : base(repository, mapper)
        {
            _fileService = fileService;
        }
        public async Task<ComplaintDetailsDto> UpdateWithFilesAsync(
    Complaint complaint,
    ComplaintUpdateDto dto,
    List<IFormFile>? newFiles)
        {
            complaint.Title = dto.Title;
            complaint.Description = dto.Description;
            complaint.Severity = dto.Severity;
            complaint.Location = dto.Location;
            complaint.Type = dto.Type;
            complaint.GovernmentEntityId = dto.GovernmentEntityId;

            if (newFiles != null && newFiles.Count > 0)
            {
                foreach (var file in newFiles)
                {
                    if (file.Length <= 0) continue;

                    var url = await _fileService.SaveAsync(file);

                    complaint.Attachments.Add(new Attachment
                    {
                        FileName = file.FileName,
                        FileUrl = url,
                        ContentType = file.ContentType ?? string.Empty,
                        FileSize = file.Length
                    });
                }
            }

            await _repository.UpdateAsync(complaint);

            return _mapper.Map<ComplaintDetailsDto>(complaint);
        }
        public async Task<ComplaintDetailsDto> CreateWithFilesAsync(ComplaintCreateDto dto, List<IFormFile>? attachments)
        {
            var entity = _mapper.Map<Complaint>(dto);
            entity.Status = ComplaintStatus.New;
            entity.ReferenceNumber = GenerateReference();

            if (attachments != null && attachments.Count > 0)
            {
                foreach (var file in attachments)
                {
                    if (file.Length <= 0) continue;
                    var url = await _fileService.SaveAsync(file);
                    entity.Attachments.Add(new Attachment
                    {
                        FileName = file.FileName,
                        FileUrl = url,
                        ContentType = file.ContentType ?? string.Empty,
                        FileSize = file.Length
                    });
                }
            }

            var created = await _repository.AddAsync(entity);
            return _mapper.Map<ComplaintDetailsDto>(created);
        }

        public async Task<ComplaintDetailsDto?> GetByReferenceAsync(string referenceNumber)
        {
            var entity = await _repository.FirstOrDefaultAsync(c => c.ReferenceNumber == referenceNumber);
            return entity is null ? null : _mapper.Map<ComplaintDetailsDto>(entity);
        }

        public async Task<ComplaintDetailsDto> SetStatusAsync(Guid id, ComplaintStatus status, string? agencyNotes, string? additionalInfoRequest)
        {


            var entity = await _repository.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("Complaint not found");

            if (!AllowedTransitions.TryGetValue(entity.Status, out var allowed) || !allowed.Contains(status))
                throw new Exception("Invalid status transition");

            entity.Status = status;
            entity.AgencyNotes = agencyNotes ?? entity.AgencyNotes;
            entity.AdditionalInfoRequest = additionalInfoRequest ?? entity.AdditionalInfoRequest;

            await _repository.UpdateAsync(entity);
            return _mapper.Map<ComplaintDetailsDto>(entity);
        }

        private static readonly Dictionary<ComplaintStatus, ComplaintStatus[]> AllowedTransitions = new()
        {
            { ComplaintStatus.New, new[] { ComplaintStatus.InProgress, ComplaintStatus.Rejected } },
            { ComplaintStatus.InProgress, new[] { ComplaintStatus.Completed, ComplaintStatus.Rejected } },
            { ComplaintStatus.Completed, Array.Empty<ComplaintStatus>() },
            { ComplaintStatus.Rejected, Array.Empty<ComplaintStatus>() }
        };

        private string GenerateReference()
        {
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var rand = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("+", "")
                .Replace("/", "")
                .Replace("=", "")
                .Substring(0, 6)
                .ToUpperInvariant();
            return $"CMP-{date}-{rand}";
        }
    }
}
