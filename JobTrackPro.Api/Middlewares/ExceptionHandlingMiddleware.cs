using System.Text.Json;

using JobTrackPro.Application.Common.Exceptions;

using ValidationException = JobTrackPro.Application.Common.Exceptions.ValidationException;

namespace JobTrackPro.Api.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning("Validation error: {Errors}", ex.Errors);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "Validation error",
                status = 400,
                errors = ex.Errors
            }));
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning("Record not found: {Message}", ex.Message);

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                title = ex.Message,
                status = 404
            }));
        }
        catch (Exception ex)
        {
            
            logger.LogError(ex, "An unexpected error occurred");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                title = "An unexpected error occurred.",
                status = 500
            }));
        }
    }
}