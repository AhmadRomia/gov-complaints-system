using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using MediatR;
using Domain.Entities;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetComplaintCountsByAgencyQuery() : IRequest<List<AgencyComplaintCountDto>>;

    public class GetComplaintCountsByAgencyQueryHandler : IRequestHandler<GetComplaintCountsByAgencyQuery, List<AgencyComplaintCountDto>>
    {
        private readonly IRepository<Complaint> _complaintRepo;
        private readonly IRepository<GovernmentEntity> _govRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetComplaintCountsByAgencyQueryHandler(IRepository<Complaint> complaintRepo, IRepository<GovernmentEntity> govRepo, ICurrentUserService currentUser, UserManager<ApplicationUser> userManager)
        {
            _complaintRepo = complaintRepo;
            _govRepo = govRepo;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<List<AgencyComplaintCountDto>> Handle(GetComplaintCountsByAgencyQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                throw new BadRequestException("User not authenticated");

            var userId = _currentUser.UserId ?? throw new BadRequestException("User context missing");
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new BadRequestException("User not found");

            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            bool isAgency = await _userManager.IsInRoleAsync(user, "Agency");

            if (!isAdmin && !isAgency)
                throw new BadRequestException("Access denied");

            // if agency, restrict to their own government
            Guid? filterGovId = null;
            if (isAgency)
            {
                if (user.GovernmentEntityId == null)
                    throw new BadRequestException("User has no associated government entity");
                filterGovId = user.GovernmentEntityId;
            }

            var govs = await _govRepo.GetAllAsync();

            var result = new List<AgencyComplaintCountDto>();

            foreach (var g in govs)
            {
                if (filterGovId != null && g.Id != filterGovId) continue;
                var cnt = await _complaintRepo.CountAsync(c => c.GovernmentEntityId == g.Id);
                result.Add(new AgencyComplaintCountDto { GovernmentEntityId = g.Id, GovernmentEntityName = g.Name, Count = cnt });
            }

            return result.OrderByDescending(r => r.Count).ToList();
        }
    }
}
