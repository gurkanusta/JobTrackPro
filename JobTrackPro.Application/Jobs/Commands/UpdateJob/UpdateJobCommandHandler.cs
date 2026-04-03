
using JobTrackPro.Application.Common.Exceptions;
using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Domain.Entities;



using MediatR;


using Microsoft.EntityFrameworkCore;

namespace JobTrackPro.Application.Jobs.Commands.UpdateJob;

public sealed class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, Unit>
{
    private readonly IAppDbContext _db;

    public UpdateJobCommandHandler(IAppDbContext db) => _db = db;

    public async Task<Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        

        var job = await _db.JobApplications
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(JobApplication), request.Id);
        

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