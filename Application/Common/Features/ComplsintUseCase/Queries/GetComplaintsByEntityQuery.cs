using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Features.ComplsintUseCase;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using AutoMapper;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetComplaintsByEntityQuery(Guid GovernmentEntityId) : IRequest<List<ComplaintListDto>>;

    public class GetComplaintsByEntityQueryHandler
        : IRequestHandler<GetComplaintsByEntityQuery, List<ComplaintListDto>>
    {
        private readonly IComplaintService _complaintService;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public GetComplaintsByEntityQueryHandler(
            IComplaintService complaintService,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _complaintService = complaintService;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<List<ComplaintListDto>> Handle(GetComplaintsByEntityQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                throw new BadRequestException("User not authenticated");

            if (!await _currentUser.IsInRoleAsync("Admin"))
                throw new BadRequestException("Only Admin can list complaints by entity");

            if (request.GovernmentEntityId == Guid.Empty)
                throw new BadRequestException("Government entity ID is required");

            var complaints = await _complaintService.GetAllAsync(
                c => c.GovernmentEntityId == request.GovernmentEntityId);

            return _mapper.Map<List<ComplaintListDto>>(complaints);
        }
    }

}
