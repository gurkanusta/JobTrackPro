using MediatR;
using JobTrackPro.Application.Common.Models;
using JobTrackPro.Application.Jobs.DTOs;
using JobTrackPro.Domain.Enums;

namespace JobTrackPro.Application.Jobs.Queries.GetJobs;

public record GetJobsQuery(
    string? Search,
    ApplicationStatus? Status,
    int Page = 1,
    int PageSize = 10
) : IRequest<PagedResult<JobDto>>;