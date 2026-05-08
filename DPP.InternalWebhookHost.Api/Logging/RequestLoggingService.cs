using Newtonsoft.Json;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace DPP.InternalWebhookHost.Api.Logging
{
    public static class RequestLoggingService
    {
        public static bool ShouldEnableBuffering(HttpContext context)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(context.Request.ContentType))
                    return false;

                if ((context.Request.Method == HttpMethods.Post ||
                     context.Request.Method == HttpMethods.Put ||
                     context.Request.Method == HttpMethods.Patch)
                    && (context.Request.ContentType.Contains("application/json", StringComparison.OrdinalIgnoreCase)
                        || context.Request.ContentType.Contains("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase)))
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        public static async Task LogHttpRequestAsync(ILogger logger, HttpContext context, Exception? ex = null)
        {
            var routeParams = context.Request.RouteValues
                .Select(r => $"{r.Key}={r.Value}")
                .ToList();

            var queryParams = context.Request.Query
                .Select(q => $"{q.Key}={q.Value}")
                .ToList();

            var requestBodyType = string.Empty;
            var requestBodyContent = string.Empty;

            try
            {
                if (context.Request.HasFormContentType)
                {
                    requestBodyType = "Form Data";
                    var formDictionary = context.Request.Form.ToDictionary(f => f.Key, f => f.Value.ToString());
                    requestBodyContent = formDictionary.Count > 0
                        ? JsonConvert.SerializeObject(formDictionary, Formatting.Indented)
                        : "[Empty Body]";
                }
                else if (!string.IsNullOrWhiteSpace(context.Request.ContentType) &&
                         context.Request.ContentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
                {
                    requestBodyType = "JSON Body";
                    if (ShouldEnableBuffering(context))
                    {
                        requestBodyContent = await ReadRequestBodyAsync(context);
                    }
                    else
                    {
                        requestBodyContent = "[Unsupported or Empty Body]";
                    }

                }
                else
                {
                    requestBodyType = "Unsupported or Empty Body";
                    requestBodyContent = $"[No supported body content. Content-Type: {context.Request.ContentType ?? "None"}]";
                }
            }
            catch (Exception formEx)
            {
                requestBodyType = "Error Reading Body/Form";
                requestBodyContent = "[Error reading form or body]";
                logger.LogWarning(formEx, "Error occurred while reading the request body/form.");
            }

            var logMessage = "------ Incoming Request ------\n" +
                             "Method: {Method}\n" +
                             "Path: {Path}\n" +
                             "Route: {Route}\n" +
                             "Query: {Query}\n" +
                             "{RequestBodyType}: {RequestBodyContent}\n" +
                             "------------------------------";

            if (ex != null)
            {
                logger.LogError(ex, logMessage, context.Request.Method, context.Request.Path,
                    string.Join(", ", routeParams), string.Join(", ", queryParams),
                    requestBodyType, requestBodyContent);
            }
            else
            {
                logger.LogInformation(logMessage, context.Request.Method, context.Request.Path,
                    string.Join(", ", routeParams), string.Join(", ", queryParams),
                    requestBodyType, requestBodyContent);
            }
        }

        public static void LogHttpResponseAsync(ILogger logger, HttpContext context, object? responseObject)
        {
            var serializedResponse = string.Empty;

            try
            {
                serializedResponse = JsonConvert.SerializeObject(responseObject, Formatting.Indented);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to serialize response.");
                serializedResponse = "[Unserializable Response]";
            }

            if (serializedResponse.Length > 5000)
            {
                serializedResponse = serializedResponse.Substring(0, 5000) + "... [Truncated]";
            }

            var logMessage = "----- Outgoing Response -----\n" +
                             "Method: {Method}\n" +
                             "Path: {Path}\n" +
                             "Response: {ResponseContent}\n" +
                             "-----------------------------";

            logger.LogInformation(logMessage, context.Request.Method, context.Request.Path, serializedResponse);
        }

        private static async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (body.Length > 5000)
            {
                body = body.Substring(0, 5000) + "... [Truncated]";
            }

            return string.IsNullOrWhiteSpace(body) ? "[Empty Body]" : body;
        }
    }
}
