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
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ApiResponseStatus.Unauthorized.Value();

                var errorResponse = new
                {
                    code = ApiResponseStatus.Unauthorized.Value(),
                    message = ApiResponseStatus.Unauthorized.Description()
                };

                var jsonErrorResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonErrorResponse);
            }
            else if (context.Response.StatusCode == ApiResponseStatus.MethodNotAllow.Value())
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ApiResponseStatus.MethodNotAllow.Value();

                var errorResponse = new
                {
                    code = ApiResponseStatus.MethodNotAllow.Value(),
                    message = ApiResponseStatus.MethodNotAllow.Description()
                };

                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonResponse);
            }
            else if (context.Response.StatusCode == ApiResponseStatus.NotFound.Value())
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ApiResponseStatus.NotFound.Value();

                var errorResponse = new
                {
                    code = ApiResponseStatus.NotFound.Value(),
                    message = ApiResponseStatus.NotFound.Description()
                };

                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
