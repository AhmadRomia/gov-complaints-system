using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Domain.Entities;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetComplaintHistoryQuery(Guid ComplaintId) : IRequest<List<ComplaintActionDto>>;

    public class GetComplaintHistoryQueryHandler : IRequestHandler<GetComplaintHistoryQuery, List<ComplaintActionDto>>
    {
        private readonly IRepository<Complaint> _complaintRepo;
        private readonly IRepository<ComplaintAction> _actionRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public GetComplaintHistoryQueryHandler(
            IRepository<Complaint> complaintRepo,
            IRepository<ComplaintAction> actionRepo,
            ICurrentUserService currentUser,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _complaintRepo = complaintRepo;
            _actionRepo = actionRepo;
            _currentUser = currentUser;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<ComplaintActionDto>> Handle(GetComplaintHistoryQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                throw new BadRequestException("User not authenticated");

            var complaint = await _complaintRepo.FirstOrDefaultAsync(c => c.Id == request.ComplaintId)
                ?? throw new BadRequestException("Complaint not found");

            var userId = _currentUser.UserId ?? throw new BadRequestException("User context missing");

            var user = await _userManager.FindByIdAsync(userId.ToString())
                       ?? throw new BadRequestException("User not found");

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                // admin can view any history
            }
            else if (await _userManager.IsInRoleAsync(user, "Agency"))
            {
                if (user.GovernmentEntityId == null || complaint.GovernmentEntityId != user.GovernmentEntityId)
                    throw new BadRequestException("Access denied");
            }
            else if (await _userManager.IsInRoleAsync(user, "Citizen"))
            {
                if (complaint.CitizenId != userId)
                    throw new BadRequestException("Access denied");
            }
            else
            {
                throw new BadRequestException("Access denied");
            }

            var actions = await _actionRepo.GetAllAsync(a => a.ComplaintId == request.ComplaintId, orderBy: q => q.OrderByDescending(a => a.OccurredAt));
            return _mapper.Map<List<ComplaintActionDto>>(actions);
        }
    }
}
