
using JobTrackPro.Domain.Enums;



namespace JobTrackPro.Domain.Entities;

public class JobApplication
{
    public Guid Id { get; set; }= Guid.NewGuid();

    public string CompanyName { get; set; } = default!;

    public string Position { get; set; } = default!;

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Accepted;

    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

    public string? Notes { get; set; }

    public string UserId {  get; set; }
    public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }


    public bool IsDeleted { get; set; }
}