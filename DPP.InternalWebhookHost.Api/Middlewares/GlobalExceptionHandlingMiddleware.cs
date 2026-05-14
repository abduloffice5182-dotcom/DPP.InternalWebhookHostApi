namespace DPP.InternalWebhookHost.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
{
	public async Task Invoke(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			await LogAndHandleExceptionAsync(context, ex);
		}
	}

	private async Task LogAndHandleExceptionAsync(
	HttpContext context,
	Exception ex)
	{
		context.Request.EnableBuffering();
		string requestBody = string.Empty;
		if (context.Request.ContentLength > 0)
		{
			context.Request.Body.Position = 0;
			using var reader = new StreamReader(
				context.Request.Body,
				Encoding.UTF8,
				detectEncodingFromByteOrderMarks: false,
				leaveOpen: true);
			requestBody = await reader.ReadToEndAsync();
			context.Request.Body.Position = 0;
		}
		var queryString = context.Request.QueryString.Value;
		var path = context.Request.Path;
		var headers = string.Join(
			Environment.NewLine,
			context.Request.Headers.Select(x =>
				$"{x.Key} : {x.Value}"));
		var userAgent =
			context.Request.Headers.UserAgent.ToString();
		var (statusCode, message) = ex switch
		{
			ValidationException valEx =>
				(
					StatusCodes.Status400BadRequest,
					string.Join(" | ",
						valEx.Errors.Select(e => e.ErrorMessage))
				),

			ArgumentException or
			InvalidOperationException or
			BadHttpRequestException =>
				(
					StatusCodes.Status400BadRequest,
					ex.Message
				),

			UnauthorizedAccessException =>
				(
					StatusCodes.Status401Unauthorized,
					"You are not authorized to perform this action."
				),

			_ =>
				(
					StatusCodes.Status500InternalServerError,
					"An unexpected internal server error occurred."
				)
		};

		logger.LogError(ex,
			@"Exception Occurred StatusCode : {StatusCode} , Message : {Message} ,Path : {Path},
QueryString : {QueryString},UserAgent : {UserAgent} ,Headers :{Headers},Payload :{Payload},StackTrace :
{StackTrace}", statusCode, message, path, queryString, userAgent, headers, requestBody, ex.StackTrace);

		await WriteErrorResponseAsync(
			context,
			statusCode,
			message);
	}

	private static Task WriteErrorResponseAsync(HttpContext context, int statusCode, string errorMessage)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = statusCode; 
		var response = new ApiResponse<object>(null, null, errorMessage);

		return context.Response.WriteAsJsonAsync(response);
	}
}