using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Application.Common.Exceptions;
using JobTrackPro.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobTrackPro.Application.Jobs.Commands.DeleteJob;

public record DeleteJobCommand(Guid Id) : IRequest<Unit>;

public sealed class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand, Unit>
{
    private readonly IAppDbContext _db;

    public DeleteJobCommandHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<Unit> Handle(
        DeleteJobCommand request,
        CancellationToken cancellationToken)
    {
        var job = await _db.JobApplications
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(JobApplication), request.Id);

        job.Delete();
        await _db.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}