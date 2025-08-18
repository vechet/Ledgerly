using Ledgerly.Helpers;
using System.Text.Json;

namespace Ledgerly.API.Middlewares
{
    public class HandleApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public HandleApiExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == ApiResponseStatus.Unauthorized.Value())
            {
                handleStatus(context, ApiResponseStatus.Unauthorized);
            }
            else if (context.Response.StatusCode == ApiResponseStatus.MethodNotAllow.Value())
            {
                handleStatus(context, ApiResponseStatus.MethodNotAllow);
            }
            else if (context.Response.StatusCode == ApiResponseStatus.NotFound.Value())
            {
                handleStatus(context, ApiResponseStatus.NotFound);
            }
            else if (context.Response.StatusCode == ApiResponseStatus.Forbidden.Value())
            {
                handleStatus(context, ApiResponseStatus.Forbidden);
            }
        }

        private async void handleStatus(HttpContext context, ApiResponseStatus statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode.Value();

            var errorResponse = new
            {
                code = statusCode.Value(),
                message = statusCode.Description()
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
