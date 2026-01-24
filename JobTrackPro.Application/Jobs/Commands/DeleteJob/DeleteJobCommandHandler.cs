using MediatR;
using Microsoft.EntityFrameworkCore;
using JobTrackPro.Application.Common.Interfaces;

namespace JobTrackPro.Application.Jobs.Commands.DeleteJob;

public sealed class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand, Unit>
{
    private readonly IAppDbContext _db;

    public DeleteJobCommandHandler(IAppDbContext db) => _db = db;

    public async Task<Unit> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _db.JobApplications

            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (job is null || job.IsDeleted)


            throw new InvalidOperationException("Job not found.");

        job.IsDeleted = true;
        job.UpdatedAt = DateTime.UtcNow;


        _db.UpdateJobApplication(job);
        await _db.SaveChangesAsync(cancellationToken);

        return Unit.Value; 
    }
}
