using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetComplaintByIdQuery(Guid Id) : IRequest<ComplaintDetailsDto>;

    public class GetComplaintByIdQueryHandler
        : IRequestHandler<GetComplaintByIdQuery, ComplaintDetailsDto>
    {
        private readonly IComplaintService _complaintService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetComplaintByIdQueryHandler(
            IComplaintService complaintService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ICurrentUserService currentUser)
        {
            _complaintService = complaintService;
            _userManager = userManager;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<ComplaintDetailsDto> Handle(GetComplaintByIdQuery request, CancellationToken cancellationToken)
        {
            //if (!_currentUser.IsAuthenticated)
            //    throw new BadRequestException("User not authenticated");

            var userGuid = _currentUser.UserId ?? throw new BadRequestException("User context is missing");

            var entity = await _complaintService.GetByIdAsync(request.Id, "AgencyNotes,AdditionalInfoRequests")
                         ?? throw new BadRequestException("Complaint not found");

            if (await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(userGuid.ToString()), "Citizen"))
            {
                if (entity.CitizenId != userGuid)
                    throw new BadRequestException("Access denied to this complaint");
            }
            else if (await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(userGuid.ToString()), "Agency"))
            {
                var user = await _userManager.FindByIdAsync(userGuid.ToString());
                if (user == null || user.GovernmentEntityId == null)
                    throw new BadRequestException("Access denied");

                if (entity.GovernmentEntityId != user.GovernmentEntityId)
                    throw new BadRequestException("Access denied to this complaint");
            }
           

            return _mapper.Map<ComplaintDetailsDto>(entity);
        }
    }


}
