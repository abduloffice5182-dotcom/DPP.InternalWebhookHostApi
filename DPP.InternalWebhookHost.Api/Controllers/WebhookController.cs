namespace DPP.InternalWebhookHost.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class WebhookController : ControllerBase
{
	public ILogger<WebhookController> logger { get; set; }
	private readonly IMediator mediator;
	public WebhookController(ILogger<WebhookController> logger, IMediator mediator)
	{
		this.logger = logger;
		this.mediator = mediator;
	}

	#region GET
	[HttpGet]
	[Route("report")]
	public async Task<IActionResult> GetReport([FromQuery] GetWebhookReportQuery request, CancellationToken cancellationToken)
	{
		try
		{

            if (request.FromDate > request.ToDate)
                return BadRequest(new { Message = "Start date cannot be greater than end date." });
            if (request.ToDate.TimeOfDay == TimeSpan.Zero)
                request.ToDate = request.ToDate.Date.AddDays(1).AddTicks(-1);

            var query = new GetWebhookReportQuery
			{
                FromDate = request.FromDate,
                ToDate = request.ToDate,
				PageNumber = request.PageNumber,
				PageSize = request.PageSize
			};

			var response = await mediator.Send(query, cancellationToken);
			return Ok(response);

		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Get Reports has failed ,(GetWebhookReportQuery) :{Message}", request);
            return StatusCode(500, new ApiResponse(false, 500, "Error while fetching the webhook logs", null));
        }

	}
	#endregion

	#region POST
	[HttpPost]
	[Route("save")]
	public async Task<IActionResult> Post(CancellationToken cancellationToken)
	{
		string requestBody = string.Empty;
		try
		{
			Request.EnableBuffering();
			using (var reader = new StreamReader(Request.Body
				,Encoding.UTF8
				,detectEncodingFromByteOrderMarks: false
				,leaveOpen: true))
			{
				requestBody = await reader.ReadToEndAsync();
				Request.Body.Position = 0;
			}

			logger.LogInformation("Webhook Payload : {0}", requestBody);
			if (string.IsNullOrWhiteSpace(requestBody))
			{
				return Ok(new ApiResponse(Success: false, (int)HttpStatusCode.BadRequest, "Payload is Empty", null));
			}

			var response = await mediator.Send(new SaveWebhookCommand
			{
				Payload = requestBody
			}, cancellationToken);

			return Ok(new ApiResponse(true, (int)HttpStatusCode.OK, "Payload Recieved Successfully", response));
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error while Saving Webhook Payload ,(dynamic) : {0}", requestBody);

			return Ok(new ApiResponse(false, (int)HttpStatusCode.InternalServerError, "Error while Saving Webhook Payload", null));
		}

	}
	#endregion
}
