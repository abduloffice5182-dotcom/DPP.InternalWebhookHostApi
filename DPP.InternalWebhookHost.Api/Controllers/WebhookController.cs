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
		var response = await mediator.Send(request, cancellationToken);

		return Ok(new ApiResponse<IEnumerable<WebhookLogsResponse>>(null , response));
	}
	#endregion

	#region POST
	[HttpPost]
	[Route("{endpointId}")]
	public async Task<IActionResult> Post(string endpointId, CancellationToken cancellationToken)
	{
		if (!Request.HasJsonContentType())
		{
			return BadRequest(new ApiResponse<object>(
				Message: "Only application/json content type is supported.",
				Data: null));
		}

		string requestBody = string.Empty;
		Request.EnableBuffering();
		using (var reader = new StreamReader(Request.Body
			, Encoding.UTF8
			, detectEncodingFromByteOrderMarks: false
			, leaveOpen: true))
		{
			requestBody = await reader.ReadToEndAsync();
			Request.Body.Position = 0;
		}

		logger.LogInformation("Webhook Payload : {0}", requestBody);

		await mediator.Send(new SaveWebhookCommand
		{
			Payload = requestBody, 
			EndpointId = endpointId
		}, cancellationToken);

		return Ok(new ApiResponse<object>("Payload Recieved Successfully", null));
	}
	#endregion
}
