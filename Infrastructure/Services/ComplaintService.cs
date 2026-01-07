using Application.Common.Exceptions;
using Application.Common.Features.ComplsintUseCase;
using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using Application.Notifier.Core.Firebase;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Infrastructure.Services
{
    public class ComplaintService
        : GenericService<Complaint, ComplaintCreateDto, ComplaintUpdateDto, ComplaintListDto, ComplaintDetailsDto>,
          IComplaintService
    {
        private readonly IFileService _fileService;
        private readonly IRepository<ComplaintAction> _actionRepository;
        private readonly IRepository<AgencyNote> _agencyNoteRepository;
        private readonly IRepository<AdditionalInfoRequest> _additionalInfoRepository;
        private readonly IFirebaseCoreService _firebaseCoreService;

        public ComplaintService(
            IRepository<Complaint> repository,
            IMapper mapper,
            IFileService fileService,
            IRepository<ComplaintAction> actionRepository,
            IRepository<AgencyNote> agencyNoteRepository,
            IFirebaseCoreService firebaseCoreService,
            IRepository<AdditionalInfoRequest> additionalInfoRepository)
            : base(repository, mapper)
        {
            _fileService = fileService;
            _actionRepository = actionRepository;
            _agencyNoteRepository = agencyNoteRepository;
            _additionalInfoRepository = additionalInfoRepository;
            _firebaseCoreService = firebaseCoreService;
        }
        public async Task<ComplaintDetailsDto> UpdateWithFilesAsync(
    Complaint complaint,
    ComplaintUpdateDto dto,
    List<IFormFile>? newFiles)
        {
            complaint.Title = dto.Title;
            complaint.Description = dto.Description;
            complaint.Severity = dto.Severity;
            complaint.LocationLat = dto.LocationLat;
            complaint.LocationLong = dto.LocationLong;
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

        public async Task<ComplaintDetailsDto> SetStatusAsync(Guid id, Guid userId, ComplaintStatus status, string? agencyNotes, string? additionalInfoRequest)
        {


            var entity = await _repository.FirstOrDefaultAsync(c => c.Id == id, c => c.AgencyNotes, c => c.AdditionalInfoRequests) ?? throw new BadRequestException("Complaint not found");

            if (entity.LockedBy != userId)
                throw new BadRequestException("You do not own the lock on this complaint");

            if (!AllowedTransitions.TryGetValue(entity.Status, out var allowed) || !allowed.Contains(status))
                throw new Exception("Invalid status transition");

            var previousStatus = entity.Status;

            entity.Status = status;



            if (!string.IsNullOrWhiteSpace(agencyNotes))
            {
                await _agencyNoteRepository.AddAsync(new AgencyNote { Note = agencyNotes, ComplaintId = entity.Id });
            }

            if (!string.IsNullOrWhiteSpace(additionalInfoRequest))
            {
                await _additionalInfoRepository.AddAsync(new AdditionalInfoRequest { RequestMessage = additionalInfoRequest, ComplaintId = entity.Id });
            }

            await _repository.UpdateAsync(entity);

            // record action: status changed
            await _actionRepository.AddAsync(new ComplaintAction
            {
                ComplaintId = entity.Id,
                ActionType = ActionType.StatusChanged,
                IssuerId = userId,
                Description = $"Status changed from {previousStatus} to {status}"
            });

            // record agency notes action if provided
            if (!string.IsNullOrWhiteSpace(agencyNotes))
            {
                await _actionRepository.AddAsync(new ComplaintAction
                {
                    ComplaintId = entity.Id,
                    ActionType = ActionType.AgencyNotesAdded,
                    IssuerId = userId,
                    Description = agencyNotes
                });
            }

            // record additional info request action if provided
            if (!string.IsNullOrWhiteSpace(additionalInfoRequest))
            {
                await _actionRepository.AddAsync(new ComplaintAction
                {
                    ComplaintId = entity.Id,
                    ActionType = ActionType.AdditionalInfoRequested,
                    IssuerId = userId,
                    Description = additionalInfoRequest
                });
            }

            var dto = _mapper.Map<ComplaintListDto>(entity);

            var data = new Dictionary<string, string>
                       {
                           { "NewStatus", JsonSerializer.Serialize(dto) }
                        };


            await _firebaseCoreService.SendNotificationAndDataToTopic(
      topic: entity.CitizenId.ToString(),
      title: entity.GovernmentEntity?.Name ?? "Ministry",
      body: "Complaint Status Updated",
      data: data
  );
         

            return _mapper.Map<ComplaintDetailsDto>(entity);
        }


        public async Task<ComplaintDetailsDto> TakeOwnerShip(Guid id, Guid userId)
        {
            var entity = await _repository.FirstOrDefaultAsync(c => c.Id == id, c => c.AgencyNotes, c => c.AdditionalInfoRequests) ?? throw new BadRequestException("Complaint not found");
            if (entity.LockedBy != null)
                throw new BadRequestException("Complaint is already locked");

            entity.LockedBy = userId;
            await _repository.UpdateAsync(entity);

            await _actionRepository.AddAsync(new ComplaintAction
            {
                ComplaintId = entity.Id,
                ActionType = ActionType.TakenOwnership,
                IssuerId = userId,
                Description = "Taken ownership"
            });

            return _mapper.Map<ComplaintDetailsDto>(entity);
        }

        public async Task<ComplaintDetailsDto> ReleasOwnerShip(Guid id, Guid userId)
        {
            var entity = await _repository.FirstOrDefaultAsync(c => c.Id == id, c => c.AgencyNotes, c => c.AdditionalInfoRequests) ?? throw new BadRequestException("Complaint not found");
            if (entity.LockedBy == null)
                throw new BadRequestException("Complaint is not locked");

            if (entity.LockedBy != userId)
                throw new BadRequestException("You do not own the lock on this complaint");

            entity.LockedBy = null;
            await _repository.UpdateAsync(entity);

            await _actionRepository.AddAsync(new ComplaintAction
            {
                ComplaintId = entity.Id,
                ActionType = ActionType.ReleasedOwnership,
                IssuerId = userId,
                Description = "Released ownership"
            });

            return _mapper.Map<ComplaintDetailsDto>(entity);
        }

        public async Task<ComplaintDetailsDto> AddAgencyNote(Guid id, Guid userId, string note)
        {
            var entity = await _repository.FirstOrDefaultAsync(c => c.Id == id, c => c.AgencyNotes, c => c.AdditionalInfoRequests) ?? throw new BadRequestException("Complaint not found");

            if (entity.LockedBy != userId)
                throw new BadRequestException("You do not own the lock on this complaint");

            if (string.IsNullOrWhiteSpace(note))
                throw new BadRequestException("Note cannot be empty");

            await _agencyNoteRepository.AddAsync(new AgencyNote { Note = note, ComplaintId = entity.Id });

            // No need to update the complaint entity graph anymore

            await _actionRepository.AddAsync(new ComplaintAction
            {
                ComplaintId = entity.Id,
                ActionType = ActionType.AgencyNotesAdded,
                IssuerId = userId,
                Description = note
            });

            return _mapper.Map<ComplaintDetailsDto>(entity);
        }

        public async Task<ComplaintDetailsDto> RequestAdditionalInfo(Guid id, Guid userId, string infoRequest)
        {
            var entity = await _repository.FirstOrDefaultAsync(c => c.Id == id, c => c.AgencyNotes, c => c.AdditionalInfoRequests) ?? throw new BadRequestException("Complaint not found");

            if (entity.LockedBy != userId)
                throw new BadRequestException("You do not own the lock on this complaint");

            if (string.IsNullOrWhiteSpace(infoRequest))
                throw new BadRequestException("Request info cannot be empty");

            await _additionalInfoRepository.AddAsync(new AdditionalInfoRequest { RequestMessage = infoRequest, ComplaintId = entity.Id });

            // No need to update the complaint entity graph anymore

            await _actionRepository.AddAsync(new ComplaintAction
            {
                ComplaintId = entity.Id,
                ActionType = ActionType.AdditionalInfoRequested,
                IssuerId = userId,
                Description = infoRequest
            });

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

            var rand = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("+", "")
                .Replace("/", "")
                .Replace("=", "")
                .Substring(0, 6)
                .ToUpperInvariant();
            return $"CMP-{rand}";
        }


    }
}
