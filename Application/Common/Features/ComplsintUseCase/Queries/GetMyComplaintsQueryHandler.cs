using Application.Common.Exceptions;
using Application.Common.Features.ComplsintUseCase.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;


namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public class GetMyComplaintsQueryHandler
    : IRequestHandler<GetMyComplaintsQuery, List<ComplaintListDto>>
    {
        private readonly IComplaintService _complaintService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetMyComplaintsQueryHandler(
            IComplaintService complaintService,
            IMapper mapper,
            ICurrentUserService currentUser)
        {
            _complaintService = complaintService;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<List<ComplaintListDto>> Handle(GetMyComplaintsQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                throw new BadRequestException("User not authenticated");

            if (!await _currentUser.IsInRoleAsync("Citizen"))
                throw new BadRequestException("Only citizens can view their complaints");

            var userGuid = _currentUser.UserId ?? throw new BadRequestException("User context is missing");

            var complaints = await _complaintService.GetAllAsync(
                c => c.CitizenId == userGuid ,orderBy: c=> c.OrderBy(c => c.CreatedAt), includes : "GovernmentEntity, Citizen");

            return _mapper.Map<List<ComplaintListDto>>(complaints);
        }
    }


}
