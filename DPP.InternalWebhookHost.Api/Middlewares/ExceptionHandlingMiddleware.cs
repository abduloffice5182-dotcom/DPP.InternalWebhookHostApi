using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DPP.InternalWebhookHost.Api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during {Method} request for {Path}. Message: {Message}",
            context.Request.Method,
            context.Request.Path,
            ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            FluentValidation.ValidationException valEx => (400, string.Join(" ", valEx.Errors.Select(e => e.ErrorMessage))),
            ArgumentException => (400, exception.Message),
            _ => (500, "An internal error occurred.ex :{exception.Message}")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var response = new
        {
            success = false,
            statusCode = statusCode,
            message = message,
            data = (object?)null
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}
