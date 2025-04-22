using System.Net;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using E_Commerce.Core.Shared;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Core.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
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

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new CommonResponse<object>
            {
                Errors = new List<Error>()
            };

            // You can customize the status code and error message based on the exception type
            switch (exception)
            {
                case ApplicationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Errors.Add(new Error { Code = response.StatusCode, Message = ex.Message });
                    break;
                case KeyNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Errors.Add(new Error { Code = response.StatusCode, Message = ex.Message });
                    break;
                case UnauthorizedAccessException ex:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Errors.Add(new Error { Code = response.StatusCode, Message = ex.Message });
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Errors.Add(new Error
                    {
                        Code = response.StatusCode,
                        Message = "Internal server error. Please try again later."
                    });
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}