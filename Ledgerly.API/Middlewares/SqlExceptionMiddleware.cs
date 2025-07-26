using Ledgerly.Helpers;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;

namespace Ledgerly.API.Middlewares
{
    public class SqlExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public SqlExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SqlException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    code = ApiResponseStatus.InternalError.Value(),
                    message = ApiResponseStatus.InternalError.Description()
                };

                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
