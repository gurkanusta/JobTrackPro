using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Application.Common.Models;
using JobTrackPro.Application.Jobs.DTOs;
using JobTrackPro.Application.Common.Models;
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace JobTrackPro.Application.Jobs.Queries.GetJobs;

public sealed class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, PagedResult<JobDto>>
{
    private readonly IAppDbContext _db;

    public GetJobsQueryHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<JobDto>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.JobApplications.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();

            query = query.Where(x =>
                x.CompanyName.ToLower().Contains(search) ||
                x.Position.ToLower().Contains(search));
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.ApplicationDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new JobDto(
                x.Id,
                x.CompanyName,
                x.Position,
                x.Status,
                x.ApplicationDate
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<JobDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}