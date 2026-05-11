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
	[Route("{endpointId}")]
	public async Task<IActionResult> Post(string endpointId, CancellationToken cancellationToken)
	{
		string requestBody = string.Empty;
		try
		{
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

			return Ok(new ApiResponse(true, (int)HttpStatusCode.OK, "Payload Recieved Successfully", $"Webhook Refernce Id : {response.ToString()}"));
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error while Saving Webhook Payload ,(dynamic) : {0}", requestBody);

			return Ok(new ApiResponse(false, (int)HttpStatusCode.InternalServerError, "Error while Saving Webhook Payload", null));
		}

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
