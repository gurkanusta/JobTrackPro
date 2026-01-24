using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Application.Jobs.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobTrackPro.Application.Jobs.Queries.GetById;

public sealed class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery, JobDto>
{
    private readonly IAppDbContext _db;

    public GetJobByIdQueryHandler(IAppDbContext db) => _db = db;



    public async Task<JobDto> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await _db.JobApplications

            .Where(x => x.Id == request.Id && !x.IsDeleted)
            .Select(x => new JobDto(x.Id, x.CompanyName, x.Position, x.Status, x.ApplicationDate))
            .FirstOrDefaultAsync(cancellationToken);


        if (job is null) throw new InvalidOperationException("Job not found.");

        return job;
    }
}
