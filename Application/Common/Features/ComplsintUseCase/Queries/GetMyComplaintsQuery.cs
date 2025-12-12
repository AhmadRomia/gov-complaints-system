using Application.Common.Features.ComplsintUseCase.DTOs;
using MediatR;


namespace Application.Common.Features.ComplsintUseCase.Queries
{
    public record GetMyComplaintsQuery() : IRequest<List<ComplaintListDto>>;

}
