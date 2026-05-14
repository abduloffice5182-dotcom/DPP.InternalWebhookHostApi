namespace DPP.InternalWebhookHost.Api.Controllers;
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class WebhookController : BaseController
{ 
	private readonly IMediator mediator;
	public WebhookController(ILogger<WebhookController> logger, IMediator mediator)
	{ 
		this.mediator = mediator;
	}

	#region GET
	[HttpGet]
	[Route("report")]
	public async Task<IActionResult> GetReport([FromQuery] GetWebhookReportQuery request, CancellationToken cancellationToken)
	{
		var response = await mediator.Send(request, cancellationToken);

		return Success<IEnumerable<WebhookLogsResponse>>(response); 
	}
	#endregion

	#region POST
	[HttpPost]
	[Route("{endpointId}")]
	public async Task<IActionResult> Post(string endpointId, CancellationToken cancellationToken)
	{ 
		if (!Request.HasJsonContentType())
		{
			return Failure("Only application/json content type is supported."); 
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
		await mediator.Send(new SaveWebhookCommand
		{
			Payload = requestBody, 
			EndpointId = endpointId
		}, cancellationToken);

		return Success("Payload Recieved Successfully"); 
	}
	#endregion
}
