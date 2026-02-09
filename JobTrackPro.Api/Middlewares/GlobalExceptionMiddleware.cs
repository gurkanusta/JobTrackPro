using System.Net;
using System.Text.Json;

namespace JobTrackPro.Api.Middlewares
{
    public sealed class GlobalExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Path}", context.Request.Path);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problem = new
                {
                    title = "Unexpected server error",
                    status = context.Response.StatusCode,
                    traceId = context.TraceIdentifier,
                    path = context.Request.Path.ToString()
                };

                var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);

                context.Response.Headers["X-Global-Exception"] = "true";
            }
        }
    }
}