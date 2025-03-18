using JobPortal1.O.DTOs.Common;
using System.Net;
using System.Text.Json;

namespace JobPortal1.O.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Continue with the next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex.Message}");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // ✅ Set default status code
            var statusCode = HttpStatusCode.InternalServerError;

            // ✅ Handle Specific Exceptions
            switch (exception)
            {
                case ArgumentNullException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
            }

            var response = new ApiResponse<string>(
                false,
                exception.Message,
                null
            );

            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
