using System.Diagnostics;

using MediatR;

using Microsoft.Extensions.Logging;

namespace JobTrackPro.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var name = typeof(TRequest).Name;

        logger.LogInformation("→ {RequestName} started: {@Request}", name, request);

        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();

        logger.LogInformation("← {RequestName} completed: {ElapsedMs}ms",
            name, sw.ElapsedMilliseconds);

        return response;
    }
}