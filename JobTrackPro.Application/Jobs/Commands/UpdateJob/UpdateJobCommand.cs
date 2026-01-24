using JobTrackPro.Domain.Enums;
using MediatR;

namespace JobTrackPro.Application.Jobs.Commands.UpdateJob;

public record UpdateJobCommand(



    Guid Id,
    string CompanyName,
    string Position,
    ApplicationStatus Status,
    DateTime ApplicationDate,
    string? Notes
) : IRequest<Unit>;
