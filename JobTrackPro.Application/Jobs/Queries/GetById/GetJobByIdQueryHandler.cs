using JobTrackPro.Application.Common;
using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Application.Jobs.DTOs;
using JobTrackPro.Domain.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace JobTrackPro.Application.Jobs.Queries.GetById;

public record GetJobByIdQuery(Guid Id) : IRequest<Result<JobDto>>;

public sealed class GetJobByIdQueryHandler
    : IRequestHandler<GetJobByIdQuery, Result<JobDto>>
{
    private readonly IAppDbContext _db;

    public GetJobByIdQueryHandler(IAppDbContext db) => _db = db;

    public async Task<Result<JobDto>> Handle(
        GetJobByIdQuery request, CancellationToken ct)
    {
        var job = await _db.JobApplications
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (job is null)
            return Result<JobDto>.Failure("Job application not found.");

        return Result<JobDto>.Success(new JobDto(
            job.Id,
            job.CompanyName,
            job.Position,
            job.Status,
            job.ApplicationDate
        ));
    }
}