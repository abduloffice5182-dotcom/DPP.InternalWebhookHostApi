using DPP.InternalWebhookHost.Api.Logging;
using DPP.InternalWebhookHost.Domain.Common.ExceptionHandling;
using DPP.InternalWebhookHost.Domain.Common.Response;
using System.Net;
using System.Text.Json;

namespace DPP.PartnerPaymentIntegration.API.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // Enable buffering only for safe scenarios
            if (RequestLoggingService.ShouldEnableBuffering(context))
            {
                context.Request.EnableBuffering();
            }
            try
            {
                await _next(context);

                await HandlePipelineErrorsAsync(context);
            }
            catch (Exception ex)
            {
                await LogAndWriteErrorAsync(context, ex);
            }

        }
        private async Task HandlePipelineErrorsAsync(HttpContext context)
        {
            var statusMap = new Dictionary<int, HttpStatusCode>
            {
                { StatusCodes.Status401Unauthorized, HttpStatusCode.Unauthorized },
                { StatusCodes.Status403Forbidden, HttpStatusCode.Forbidden },
                { StatusCodes.Status500InternalServerError, HttpStatusCode.InternalServerError }
            };

            if (statusMap.TryGetValue(context.Response.StatusCode, out var statusCode))
            {
                _logger.LogError("----- {StatusCode} {StatusDescription} Response Returned by Pipeline -----\nPath: {Path}",
                    (int)statusCode, statusCode, context.Request.Path);

                await WriteErrorResponseAsync(context, statusCode);
            }

        }
        private async Task LogAndWriteErrorAsync(HttpContext context, Exception ex)
        {
            var (statusCode, customErrors) = ex switch
            {
                BadRequestCustomException badRequestEx => (HttpStatusCode.BadRequest, badRequestEx.Errors),
                UnAuthorizedCustomException unAuthorizedRequestEx => (HttpStatusCode.Unauthorized, unAuthorizedRequestEx.Errors),
                BadHttpRequestException => (HttpStatusCode.BadRequest, null),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, null),
                _ => (HttpStatusCode.BadRequest, null)
            };
            _logger.LogError("----- StatusCode StatusDescription Response Returned by Pipeline ----: Path");
            await RequestLoggingService.LogHttpRequestAsync(_logger, context, ex);
            await WriteErrorResponseAsync(context, statusCode, customErrors);
        }

        private async Task WriteErrorResponseAsync(HttpContext context, HttpStatusCode statusCode, List<string>? customMessages = null)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started. Skipping error response.");
                return;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorDetails = customMessages?.Count > 0 ? string.Join("; ", customMessages) : string.Empty;

            var errorResponse = new ApiErrorResponse
            {
                Errors = ApiErrorHandler.GetDefaultErrorMessages(statusCode, errorDetails)
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var result = System.Text.Json.JsonSerializer.Serialize(errorResponse, options);
            await context.Response.WriteAsync(result);

        }

    }
}
