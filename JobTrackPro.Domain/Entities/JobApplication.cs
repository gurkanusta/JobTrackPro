
using JobTrackPro.Domain.Enums;

namespace JobTrackPro.Domain.Entities;

public class JobApplication : BaseEntity   
{
    public string CompanyName { get; set; } = default!;
    public string Position { get; set; } = default!;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

    public string? JobUrl { get; set; }
    public string? CompanyUrl { get; set; }       
    public DateTime? InterviewDate { get; set; }
    public string? Notes { get; set; }
    public string UserId { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}