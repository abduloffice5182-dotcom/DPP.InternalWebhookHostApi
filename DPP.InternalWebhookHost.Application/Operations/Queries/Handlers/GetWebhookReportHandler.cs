using DPP.InternalWebhookHost.Application.Operations.Queries.Requests;
using DPP.InternalWebhookHost.Domain.Common.Response;
using DPP.InternalWebhookHost.Domain.Common.Response.Webhook;
using DPP.InternalWebhookHost.Domain.Entities.Request.Webhook;
using DPP.InternalWebhookHost.Infrastructure.Interfaces;
using MediatR;
namespace DPP.InternalWebhookHost.Application.Operations.Queries.Handlers;
public class GetWebhookReportHandler : IRequestHandler<GetWebhookReportQuery, IEnumerable<WebhookLogsResponse>>
{

	private readonly IWebhookRepository repository;

	public GetWebhookReportHandler(IWebhookRepository repository)
	{
		this.repository = repository;
	}

	public async Task<IEnumerable<WebhookLogsResponse>> Handle(GetWebhookReportQuery req, CancellationToken cancellationToken)
	{ 
		return await repository.GetWebhookReportAsync(new WebhookLogRequest(req.FromDate, req.ToDate, req.PageNumber, req.PageSize), cancellationToken); 
	}

}
