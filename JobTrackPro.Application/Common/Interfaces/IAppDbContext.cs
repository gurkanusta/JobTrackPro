using System.Linq;
using JobTrackPro.Domain.Entities;


namespace JobTrackPro.Application.Common.Interfaces;

public interface IAppDbContext
{
    IQueryable<JobApplication> JobApplications { get; }

    void AddJobApplication(JobApplication job);
    void UpdateJobApplication(JobApplication job);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
