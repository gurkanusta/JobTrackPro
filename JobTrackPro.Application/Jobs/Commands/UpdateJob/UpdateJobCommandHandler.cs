using JobTrackPro.Application.Common.Interfaces;
using MediatR;

namespace JobTrackPro.Application.Jobs.Commands.UpdateJob;

public sealed class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, Unit>
{
    private readonly IAppDbContext _db;

    public UpdateJobCommandHandler(IAppDbContext db)
    {
        _db = db;


    }

    public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        var job = _db.JobApplications.FirstOrDefault(x => x.Id == request.Id);

        if (job is null || job.IsDeleted)
            throw new InvalidOperationException("Job not found.");



        job.CompanyName = request.CompanyName.Trim();
        job.Position = request.Position.Trim();
        job.Status = request.Status;
        job.ApplicationDate = request.ApplicationDate;

        job.Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim();
        job.UpdatedAt = DateTime.UtcNow;


        _db.UpdateJobApplication(job);

        await _db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
