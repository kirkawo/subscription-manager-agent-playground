using SubscriptionManager.Application.Exceptions;
using System.Diagnostics;

namespace SubscriptionManager.Api.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred during request processing.");

            var (statusCode, message) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                FluentValidation.ValidationException validationException => (StatusCodes.Status400BadRequest, validationException.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };

            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new
            {
                statusCode,
                message,
                traceId
            });
        }
    }
}
