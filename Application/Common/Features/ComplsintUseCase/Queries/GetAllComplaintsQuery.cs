using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetAllComplaintsQuery() : IRequest<List<ComplaintListDto>>;

    public class GetAllComplaintsQueryHandler
    : IRequestHandler<GetAllComplaintsQuery, List<ComplaintListDto>>
    {
        private readonly IComplaintService _complaintService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetAllComplaintsQueryHandler(
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

        public async Task<List<ComplaintListDto>> Handle(GetAllComplaintsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId
                         ?? throw new BadRequestException("User not authenticated");

            var userGuid = userId;

            var user = await _userManager.FindByIdAsync(userGuid.ToString())
                       ?? throw new BadRequestException("User not found");

            if(!await _userManager.IsInRoleAsync(user, "Agency") && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                throw new BadRequestException("Only agency users and admin can list complaints");

            }

            List<ComplaintListDto> complaints=new();
            if (await _userManager.IsInRoleAsync(user, "Agency"))
            {
                if (user.GovernmentEntityId == null)
                    throw new BadRequestException("User has no associated government entity");
                var govEntityId = user.GovernmentEntityId.Value;

                complaints = await _complaintService.GetAllAsync( c => c.GovernmentEntityId == govEntityId, orderBy: q => q.OrderByDescending(c => c.CreatedAt), includes: "GovernmentEntity,Citizen");
            }
             if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                complaints = await _complaintService.GetAllAsync(orderBy: q => q.OrderByDescending(c => c.CreatedAt), includes: "GovernmentEntity,Citizen");
            }

           
            return _mapper.Map<List<ComplaintListDto>>(complaints);
        }
    }


}
