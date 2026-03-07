namespace JobTrackPro.Application.Jobs.DTOs;

public class JobStatsDto
{
    public int Total { get; set; }
    public int Applied { get; set; }
    public int Interview { get; set; }
    public int Accepted { get; set; }
    public int Rejected { get; set; }
}