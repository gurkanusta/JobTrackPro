using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Domain.Entities;
using MediatR;

namespace JobTrackPro.Application.Jobs.Commands.CreateJob;

public sealed class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Guid>
{
    private readonly IAppDbContext _db;

    public CreateJobCommandHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var job = new JobApplication
        {
            Id = Guid.NewGuid(), 
            CompanyName = request.CompanyName.Trim(),
            Position = request.Position.Trim(),
            Status = request.Status,
            ApplicationDate = request.ApplicationDate ?? DateTime.UtcNow,



            UserId = "test-user",

            Notes = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            
        };

        _db.AddJobApplication(job);
        await _db.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}
