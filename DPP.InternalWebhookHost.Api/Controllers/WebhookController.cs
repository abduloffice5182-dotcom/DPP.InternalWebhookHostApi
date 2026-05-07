using Asp.Versioning;
using DPP.InternalWebhookHost.Application.Operations.Commands.Queries.Requests;
using DPP.InternalWebhookHost.Application.Operations.Commands.Requests;
using DPP.InternalWebhookHost.Domain.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
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
	[Route("get")]
	public async Task<IActionResult> Get([FromBody] GetWebhookPayloadsRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var query = new GetWebhookPayloadsRequest
			{
				FilterStartDatetime = request.FilterStartDatetime,
				FilterEndDatetime = request.FilterEndDatetime,
				PageNumber = request.PageNumber,
				PageSize = request.PageSize
			};

			var response = await mediator.Send(query, cancellationToken);

			return Ok(response);

		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error while Saving Webhook Payload {0}", ex);
		}
		return Ok();
	}
	#endregion

	#region POST
	[HttpPost]
	[Route("post")]
	public async Task<IActionResult> Post(CancellationToken cancellationToken)
	{
		string requestBody = string.Empty;
		try
		{
			Request.EnableBuffering();
			using (var reader = new StreamReader(
				Request.Body,
				Encoding.UTF8,
				detectEncodingFromByteOrderMarks: false,
				leaveOpen: true))
			{
				requestBody = await reader.ReadToEndAsync();
				Request.Body.Position = 0;
			}

			logger.LogInformation("Webhook Payload : {0}", requestBody);
			if (string.IsNullOrWhiteSpace(requestBody))
			{
				return Ok(new ApiResponse { Success = false, HttpStatusCode = (int)HttpStatusCode.InternalServerError, Message = "Payload is Empty" });
			}

			var response = await mediator.Send(new SaveWebhookCommand
			{
				Payload = requestBody
			}, cancellationToken);

			return Ok(new ApiResponse { Success = true, HttpStatusCode = (int)HttpStatusCode.OK, Message = "Payload Recieved Successfully", Response = response });
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error while Saving Webhook Payload {0}", requestBody);
			return Ok(new ApiResponse { Success = false, HttpStatusCode = (int)HttpStatusCode.InternalServerError, Message = "Error while Saving Webhook Payload" });
		}

	}
	#endregion
}
