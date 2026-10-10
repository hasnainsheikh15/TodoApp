using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using TodoApi.Exceptions;

namespace TodoApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
                
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception) {

            var statusCode = exception is AppException appException ? appException.StatusCode : (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                success = false,
                 message = exception is AppException
        ? exception.Message
        : "An unexpected error occurred.",
                statusCode

            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }


}
