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
	public async Task<IEnumerable<WebhookLogs>> GetReport([FromQuery] GetWebhookReportQuery request, CancellationToken cancellationToken)
	{
		return await mediator.Send(request, cancellationToken); 
	}
	#endregion
	#region GET
	[HttpGet]
	[Route("details")]
	public async Task<WebhookDetails?> GetDetails([FromQuery] GetWebhookDetailsQuery request, CancellationToken cancellationToken)
	{
		return await mediator.Send(request, cancellationToken);
	}
	#endregion

	#region POST
	[HttpPost]
	[Route("{endpointId}")]
	[Consumes("application/json", "application/*+json")]
	public async Task Post(string endpointId, CancellationToken cancellationToken)
	{
		string requestBody = string.Empty;
		Request.EnableBuffering();
		using (var reader = new StreamReader(Request.Body
			, Encoding.UTF8
			,  false 
			, leaveOpen: true))
		{
			requestBody =  await reader.ReadToEndAsync(); 
			JsonDocument.Parse(requestBody);
            await mediator.Send(new SaveWebhookCommand
            {
                Payload = requestBody,
                EndpointId = endpointId
            }, cancellationToken); 
            Request.Body.Position = 0;
        }   
	}
	#endregion
}
