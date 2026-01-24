using JobTrackPro.Application.Jobs.DTOs;
using MediatR;

namespace JobTrackPro.Application.Jobs.Queries.GetJobs;

public record GetJobsQuery() : IRequest<List<JobDto>>;
