using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using MediatR;
using Domain.Entities;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetComplaintStatusCountsQuery(Guid? GovernmentEntityId) : IRequest<List<ComplaintStatusCountDto>>;

    public class GetComplaintStatusCountsQueryHandler : IRequestHandler<GetComplaintStatusCountsQuery, List<ComplaintStatusCountDto>>
    {
        private readonly IRepository<Complaint> _complaintRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetComplaintStatusCountsQueryHandler(IRepository<Complaint> complaintRepo, ICurrentUserService currentUser, UserManager<ApplicationUser> userManager)
        {
            _complaintRepo = complaintRepo;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<List<ComplaintStatusCountDto>> Handle(GetComplaintStatusCountsQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                throw new BadRequestException("User not authenticated");

            var userId = _currentUser.UserId ?? throw new BadRequestException("User context missing");
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new BadRequestException("User not found");

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                // Admin: always return global counts (ignore any supplied GovernmentEntityId)
                request = new GetComplaintStatusCountsQuery(null);
            }
            else if (await _userManager.IsInRoleAsync(user, "Agency"))
            {
                if (user.GovernmentEntityId == null)
                    throw new BadRequestException("User has no associated government entity");

                // Agency: restrict to their government entity
                request = new GetComplaintStatusCountsQuery(user.GovernmentEntityId);
            }
            else
            {
                throw new BadRequestException("Access denied");
            }

            // build counts
            var list = new List<ComplaintStatusCountDto>();
            foreach (Domain.Enums.ComplaintStatus s in Enum.GetValues(typeof(Domain.Enums.ComplaintStatus)))
            {
                var cnt = request.GovernmentEntityId == null
                    ? await _complaintRepo.CountAsync(c => c.Status == s)
                    : await _complaintRepo.CountAsync(c => c.Status == s && c.GovernmentEntityId == request.GovernmentEntityId);

                list.Add(new ComplaintStatusCountDto { Status = s, Count = cnt });
            }

            return list;
        }
    }
}
