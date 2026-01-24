using JobTrackPro.Domain.Enums;

namespace JobTrackPro.Application.Jobs.DTOs;

public record JobDto(
    Guid Id,
    string CompanyName,
    string Position,
    ApplicationStatus Status,
    DateTime ApplicationDate
);
