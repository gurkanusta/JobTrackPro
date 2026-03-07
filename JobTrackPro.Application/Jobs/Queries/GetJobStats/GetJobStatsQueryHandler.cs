using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Application.Jobs.DTOs;
using JobTrackPro.Domain.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace JobTrackPro.Application.Jobs.Queries.GetJobStats;

public class GetJobStatsQueryHandler : IRequestHandler<GetJobStatsQuery, JobStatsDto>
{
    private readonly IAppDbContext _dbContext;

    public GetJobStatsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<JobStatsDto> Handle(GetJobStatsQuery request, CancellationToken cancellationToken)
    {
        var jobs = _dbContext.JobApplications.AsQueryable();

        var total = await jobs.CountAsync(cancellationToken);
        var applied = await jobs.CountAsync(x => x.Status == ApplicationStatus.Applied, cancellationToken);
        var interview = await jobs.CountAsync(x => x.Status == ApplicationStatus.Interview, cancellationToken);
        var accepted = await jobs.CountAsync(x => x.Status == ApplicationStatus.Accepted, cancellationToken);
        var rejected = await jobs.CountAsync(x => x.Status == ApplicationStatus.Rejected, cancellationToken);

        return new JobStatsDto
        {
            Total = total,
            Applied = applied,
            Interview = interview,
            Accepted = accepted,
            Rejected = rejected
        };
    }
}