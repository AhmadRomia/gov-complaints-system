using Application.Common.Exceptions;
using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetPagedComplaintsQuery(int Page, int Size,Guid? agencyId, ComplaintStatus? ComplaintStatus) : IRequest<PageResultResponseDto<ComplaintListDto>>;

    public class GetPagedComplaintsQueryHandler : IRequestHandler<GetPagedComplaintsQuery, PageResultResponseDto<ComplaintListDto>>
    {
     
        private readonly IMapper _mapper;
        private readonly IComplaintService _complaintService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUser;

       

        public GetPagedComplaintsQueryHandler(
            IComplaintService complaintService,
            IRepository<Complaint> repository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ICurrentUserService currentUser) 
        {
            _complaintService = complaintService;
            _userManager = userManager;
            _mapper = mapper;
            _currentUser = currentUser;
        }


        public async Task<PageResultResponseDto<ComplaintListDto>> Handle(GetPagedComplaintsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId
                       ?? throw new BadRequestException("User not authenticated");

            var userGuid = userId;

            var user = await _userManager.FindByIdAsync(userGuid.ToString())
                       ?? throw new BadRequestException("User not found");

            if (!await _userManager.IsInRoleAsync(user, "Agency") && !await _userManager.IsInRoleAsync(user, "Admin"))
                throw new BadRequestException("Only agency users can list complaints");

            if (user.GovernmentEntityId == null && !await _userManager.IsInRoleAsync(user, "Admin"))
                throw new BadRequestException("User has no associated government entity");

          
            PageResultResponseDto<ComplaintListDto> page =new();
            if (await _userManager.IsInRoleAsync(user, "Agency"))
            {
                var govEntityId = user.GovernmentEntityId.Value;
                // Filter by agency's government entity and optionally by status
                Expression<Func<Complaint, bool>> filter = a => a.GovernmentEntityId == govEntityId;
                if (request.ComplaintStatus != null)
                {
                    var status = request.ComplaintStatus.Value;
                    filter = a => a.GovernmentEntityId == govEntityId && a.Status == status;
                }

                page = await _complaintService.GetPagedAsync(request.Page, request.Size, a => a.OrderByDescending(c => c.CreatedAt), filter, c => c.GovernmentEntity, c => c.Citizen, c => c.AgencyNotes, c => c.AdditionalInfoRequests);
            }
            else
            {
                var filter = (Expression<Func<Complaint, bool>>?)null;

                if (request.agencyId != null && request.ComplaintStatus != null)
                {
                    var agencyId = request.agencyId.Value;
                    var status = request.ComplaintStatus.Value;
                    filter = a => a.GovernmentEntityId == agencyId && a.Status == status;
                }
                else if (request.agencyId != null)
                {
                    var agencyId = request.agencyId.Value;
                    filter = a => a.GovernmentEntityId == agencyId;
                }
                else if (request.ComplaintStatus != null)
                {
                    var status = request.ComplaintStatus.Value;
                    filter = a => a.Status == status;
                }

                page = await _complaintService.GetPagedAsync(request.Page, request.Size, a => a.OrderByDescending(c => c.CreatedAt), filter, c => c.GovernmentEntity, c => c.Citizen, c => c.AgencyNotes, c => c.AdditionalInfoRequests);
            }
            return page; 
        }
    }
}
