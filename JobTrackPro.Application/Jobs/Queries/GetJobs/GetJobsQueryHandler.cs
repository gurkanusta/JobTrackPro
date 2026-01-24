using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Application.Jobs.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobTrackPro.Application.Jobs.Queries.GetJobs;

public sealed class GetJobsQueryHandler : IRequestHandler<GetJobsQuery, List<JobDto>>
{
    private readonly IAppDbContext _db;

    public GetJobsQueryHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<List<JobDto>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
    {
        var jobs = await _db.JobApplications
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.ApplicationDate)

            .Select(x => new JobDto(
                x.Id,

                x.CompanyName,
                x.Position,
                x.Status,
                x.ApplicationDate
            ))
            .ToListAsync(cancellationToken);

        return jobs;
    }
}
