using DPP.InternalWebhookHost.Api.Modal;
using Newtonsoft.Json;
using System.Threading;

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
		return Ok(response);
	}
	#endregion

	#region POST
	[HttpPost]
	[Route("{endpointId}")]
	public async Task<IActionResult> Post(string endpointId, CancellationToken cancellationToken)
	{
		string requestBody = string.Empty;
		if (Request.HasFormContentType)
		{
			requestBody = await HandleMultiPartFormData(cancellationToken);
		}
		else
		{
			Request.EnableBuffering();
			using (var reader = new StreamReader(Request.Body
				, Encoding.UTF8
				, detectEncodingFromByteOrderMarks: false
				, leaveOpen: true))
			{
				requestBody = await reader.ReadToEndAsync();
				Request.Body.Position = 0;
			}
		}

		logger.LogInformation("Webhook Payload : {0}", requestBody);

		var response = await mediator.Send(new SaveWebhookCommand
		{
			Payload = requestBody,
			QueryString = Request.QueryString.Value,
			Endpoint = $"{Request.Path.Value}"
		}, cancellationToken);

		return Ok(new ApiResponse<object>(true, (int)HttpStatusCode.OK, "Payload Recieved Successfully", $"Webhook Refernce Id : {response.ToString()}"));
	} 
	#endregion

	async Task<string> HandleMultiPartFormData(CancellationToken cancellationToken)
	{
		string requestBody = string.Empty;
		List<FileModel> savedFiles = new();
		var form = await Request.ReadFormAsync(cancellationToken);


		foreach (var file in form.Files)
		{
			if (file.Length > 0)
			{
				var uploadsFolder = Path.Combine(
					Directory.GetCurrentDirectory(),
					"Uploads");

				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}

				var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

				var filePath = Path.Combine(
					uploadsFolder,
					fileName);

				// S3 or Microsoft Blob Storage
				using (var stream = new FileStream(
					filePath,
					FileMode.Create))
				{
					await file.CopyToAsync(
						stream,
						cancellationToken);
				}

				savedFiles.Add(new FileModel()
				{
					FileName = fileName,
					FileExtension = Path.GetExtension(file.FileName),
					FilePath = filePath
				});
			}
		}

		// Form fields
		var formFields = form.ToDictionary(
			x => x.Key,
			x => x.Value.ToString());

		// Final request object
		var requestObject = new
		{
			FormFields = formFields,
			Files = savedFiles,
			contentType = Request.ContentType,
			contentLength = Request.ContentLength,
		};

		// Convert to JSON
		requestBody =
			JsonConvert.SerializeObject(requestObject);

		return requestBody;


	}
}
