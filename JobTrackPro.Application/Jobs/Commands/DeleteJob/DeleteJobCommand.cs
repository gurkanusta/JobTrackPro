using MediatR;

namespace JobTrackPro.Application.Jobs.Commands.DeleteJob;


public record DeleteJobCommand(Guid Id) : IRequest<Unit>;
