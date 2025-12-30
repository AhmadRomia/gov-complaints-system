using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using MediatR;
using Domain.Entities;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetComplaintCountsByGovernorateQuery() : IRequest<List<GovernorateComplaintCountDto>>;

    public class GetComplaintCountsByGovernorateQueryHandler : IRequestHandler<GetComplaintCountsByGovernorateQuery, List<GovernorateComplaintCountDto>>
    {
        private readonly IRepository<Complaint> _complaintRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetComplaintCountsByGovernorateQueryHandler(IRepository<Complaint> complaintRepo, ICurrentUserService currentUser, UserManager<ApplicationUser> userManager)
        {
            _complaintRepo = complaintRepo;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<List<GovernorateComplaintCountDto>> Handle(GetComplaintCountsByGovernorateQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                throw new BadRequestException("User not authenticated");

            var userId = _currentUser.UserId ?? throw new BadRequestException("User context missing");
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new BadRequestException("User not found");

            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            bool isAgency = await _userManager.IsInRoleAsync(user, "Agency");

            if (!isAdmin && !isAgency)
                throw new BadRequestException("Access denied");

            // if agency, restrict to their own government (no filter for governorate)
            Guid? filterGovId = null;
            if (isAgency)
            {
                if (user.GovernmentEntityId == null)
                    throw new BadRequestException("User has no associated government entity");
                filterGovId = user.GovernmentEntityId;
            }

            var result = new List<GovernorateComplaintCountDto>();

            foreach (Domain.Enums.SyrianGovernorate gov in Enum.GetValues(typeof(Domain.Enums.SyrianGovernorate)))
            {
                var cnt = await _complaintRepo.CountAsync(c => c.Governorate == gov && (filterGovId == null || c.GovernmentEntityId == filterGovId));
                result.Add(new GovernorateComplaintCountDto { Governorate = gov, Count = cnt });
            }

            return result.OrderByDescending(r => r.Count).ToList();
        }
    }
}
