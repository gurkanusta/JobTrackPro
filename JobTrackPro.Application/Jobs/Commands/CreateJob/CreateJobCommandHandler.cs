using JobTrackPro.Application.Common.Exceptions;
using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Domain.Entities;

using MediatR;

namespace JobTrackPro.Application.Jobs.Commands.CreateJob;

public sealed class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Guid>
{
    private readonly IAppDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CreateJobCommandHandler(IAppDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        
        if (_currentUser.UserId is null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var job = new JobApplication
        {
            CompanyName = request.CompanyName.Trim(),
            Position = request.Position.Trim(),
            Status = request.Status,
            ApplicationDate = request.ApplicationDate ?? DateTime.UtcNow,
            Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),
            JobUrl = string.IsNullOrWhiteSpace(request.JobUrl) ? null : request.JobUrl.Trim(),     
            CompanyUrl = string.IsNullOrWhiteSpace(request.CompanyUrl) ? null : request.CompanyUrl.Trim(),
            InterviewDate = request.InterviewDate,
            UserId = _currentUser.UserId, 
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        _db.AddJobApplication(job);
        await _db.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}