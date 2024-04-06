using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace StudentManager_BackEnd.Helpers
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SecurityTokenExpiredException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    message = "Token has expired"
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
            catch (SecurityTokenValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    message = ex.Message
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    message = ex.Message
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
        }
    }
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}

