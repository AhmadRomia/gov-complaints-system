using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public class ExportComplaintsQuery : IRequest<string>
    {
        public Domain.Enums.ExportPeriod? Period { get; set; }
    }

    public class ExportComplaintsQueryHandler : IRequestHandler<ExportComplaintsQuery, string>
    {
        private readonly IRepository<Complaint> _repository;
        private readonly ICurrentUserService _currentUser;
        private readonly IComplaintExportService _exportService;

        public ExportComplaintsQueryHandler(
            IRepository<Complaint> repository,
            ICurrentUserService currentUser,
            IComplaintExportService exportService)
        {
            _repository = repository;
            _currentUser = currentUser;
            _exportService = exportService;
        }

        public async Task<string> Handle(ExportComplaintsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Complaint> complaints;
            var startDate = DateTime.MinValue;

            if (request.Period.HasValue)
            {
                startDate = request.Period.Value switch
                {
                    Domain.Enums.ExportPeriod.Day => DateTime.UtcNow.Date,
                    Domain.Enums.ExportPeriod.Week => DateTime.UtcNow.Date.AddDays(-7),
                    Domain.Enums.ExportPeriod.Month => DateTime.UtcNow.Date.AddMonths(-1),
                    Domain.Enums.ExportPeriod.Year => DateTime.UtcNow.Date.AddYears(-1),
                    _ => DateTime.MinValue
                };
            }

            if (await _currentUser.IsInRoleAsync("Admin"))
            {
                complaints = await _repository.GetAllAsync(c => c.CreatedAt >= startDate);
            }
            else if (await _currentUser.IsInRoleAsync("Agency"))
            {
                var agencyId = _currentUser.GovernmentEntityId;
                if (agencyId == null)
                {
                    return string.Empty; // Or throw exception
                }
                complaints = await _repository.GetAllAsync(c => c.GovernmentEntityId == agencyId && c.CreatedAt >= startDate);
            }
            else
            {
                throw new System.UnauthorizedAccessException("Only Admins and Agency users can export complaints.");
            }

            return await _exportService.ExportToExcelAsync(complaints);
        }
    }
}
