using JobTrackPro.Application.Jobs.DTOs;
using MediatR;

namespace JobTrackPro.Application.Jobs.Queries.GetById;



public record GetJobByIdQuery(Guid Id) : IRequest<JobDto>;
