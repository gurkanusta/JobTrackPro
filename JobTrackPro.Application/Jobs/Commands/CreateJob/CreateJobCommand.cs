using JobTrackPro.Domain.Enums;
using MediatR;

namespace JobTrackPro.Application.Jobs.Commands.CreateJob;

public record CreateJobCommand(
    string CompanyName,
    string Position,
    ApplicationStatus Status,
    DateTime ApplicationDate
) : IRequest<Guid>;
