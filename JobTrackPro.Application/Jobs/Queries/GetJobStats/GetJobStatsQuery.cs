using JobTrackPro.Application.Jobs.DTOs;

using MediatR;

namespace JobTrackPro.Application.Jobs.Queries.GetJobStats;

public record GetJobStatsQuery() : IRequest<JobStatsDto>;