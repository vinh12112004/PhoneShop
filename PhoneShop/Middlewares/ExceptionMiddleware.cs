using PhoneShop.Model.APIResponse;
using System.Net;
using System.Text.Json;

namespace PhoneShop.Middleware
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
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new APIResponse
            {
                Status = false,
                Errors = new List<string>()
            };

            switch (exception)
            {
                case ArgumentException argEx:
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors.Add(argEx.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    response.Errors.Add("Unauthorized access");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case KeyNotFoundException:
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Errors.Add("Resource not found");
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Errors.Add("An internal server error occurred");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}