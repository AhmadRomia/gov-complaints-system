using Application.Common.Features.Admin.Dtos;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Common.Features.Admin.Queries
{
    //public class GetAllComplaintsForAdminQueryHandler
    //    : IRequestHandler<GetAllComplaintsForAdminQuery, List<AdminComplaintDto>>
    //{
    //    private readonly IApplicationDbContext _context;

    //    public GetAllComplaintsForAdminQueryHandler(IApplicationDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<List<AdminComplaintDto>> Handle(
    //        GetAllComplaintsForAdminQuery request,
    //        CancellationToken cancellationToken)
    //    {
    //        var query = _context.Complaints
    //            .Include(c => c.GovernmentEntity)
    //            .AsQueryable();

    //        if (!string.IsNullOrWhiteSpace(request.Search))
    //        {
    //            query = query.Where(c =>
    //                c.Title.Contains(request.Search) ||
    //                c.ReferenceNumber!.Contains(request.Search));
    //        }

    //        if (request.Status.HasValue)
    //        {
    //            query = query.Where(c => c.Status == request.Status.Value);
    //        }

    //        if (!string.IsNullOrWhiteSpace(request.Agency))
    //        {
    //            query = query.Where(c => c.Agency == request.Agency);
    //        }

    //        return await query
    //            .OrderByDescending(c => c.CreatedAt)
    //            .Select(c => new AdminComplaintDto
    //            {
    //                Id = c.Id,
    //                Title = c.Title,
    //                Status = c.Status,
    //                Agency = c.Agency,
    //                ReferenceNumber = c.ReferenceNumber,
    //                CreatedAt = c.CreatedAt.Value,
    //                CitizenName = "", 
    //            })
    //            .ToListAsync(cancellationToken);
    //    }
    //}
}
