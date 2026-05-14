using DPP.InternalWebhookHost.Domain.Common.Response; // For ApiResponse<T>
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using FluentValidation;

namespace DPP.InternalWebhookHost.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            context.Request.EnableBuffering();
            await next(context);
            await HandlePipelineErrorsAsync(context);
        }
        catch (Exception ex)
        {
            await LogAndHandleExceptionAsync(context, ex);
        }
    }

    private async Task HandlePipelineErrorsAsync(HttpContext context)
    {
        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized ||
            context.Response.StatusCode == StatusCodes.Status403Forbidden)
        {
			if (!context.Response.HasStarted)
            {
                var message = context.Response.StatusCode == 401 ? "Unauthorized Access" : "Forbidden Access";
                await WriteErrorResponseAsync(context, context.Response.StatusCode, message);
            }
        }
    }

    private async Task LogAndHandleExceptionAsync(HttpContext context, Exception ex)
    {
        logger.LogError(ex, "Exception caught in middleware: {0} at {1}", ex, context.Request.Path);

        var (statusCode, message) = ex switch
        {
            ValidationException valEx => (StatusCodes.Status400BadRequest, string.Join(" | ", valEx.Errors.Select(e => e.ErrorMessage))),
            ArgumentException or InvalidOperationException => (StatusCodes.Status400BadRequest, ex.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "You are not authorized to perform this action."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected internal server error occurred.")
        };

        await WriteErrorResponseAsync(context, statusCode, message);
    }

    private static Task WriteErrorResponseAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new ApiResponse<object>( 
            Message: message,
            Data: null
        );

        return context.Response.WriteAsJsonAsync(response);
    }
}