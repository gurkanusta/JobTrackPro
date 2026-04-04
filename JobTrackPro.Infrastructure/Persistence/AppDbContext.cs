
using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Domain.Entities;
using JobTrackPro.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobTrackPro.Infrastructure.Persistence;


public class AppDbContext : IdentityDbContext<AppUser>, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<JobApplication> JobApplicationsSet { get; set; } = default!;

    
    IQueryable<JobApplication> IAppDbContext.JobApplications => JobApplicationsSet;

    public void AddJobApplication(JobApplication job) => JobApplicationsSet.Add(job);
    public void UpdateJobApplication(JobApplication job) => JobApplicationsSet.Update(job);

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.CompanyName)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Position)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Notes)
                .HasMaxLength(1000);

            entity.Property(x => x.JobUrl)
    .HasMaxLength(500);

            entity.Property(x => x.CompanyUrl)
                .HasMaxLength(500);

            entity.Property(x => x.InterviewDate)
                .IsRequired(false);

            entity.Property(x => x.UserId)
                .HasMaxLength(128)  
                .IsRequired();

            entity.Property(x => x.CreatedAt).IsRequired();
            entity.Property(x => x.UpdatedAt).IsRequired();

            
            entity.HasQueryFilter(x => !x.IsDeleted);
        });
    }
}