namespace DPP.InternalWebhookHost.Application.Operations.Queries.Handlers;
public class GetWebhookReportHandler : IRequestHandler<GetWebhookReportQuery, WebhookListResponse>
{

	private readonly IWebhookRepository repository;

	public GetWebhookReportHandler(IWebhookRepository repository)
	{
		this.repository = repository;
	}
	 
	public async Task<WebhookListResponse> Handle( GetWebhookReportQuery req, CancellationToken cancellationToken)
	{
		return await repository.GetWebhookReportAsync(
			new WebhookLogRequest(
				req.FromDate,
				req.ToDate,
				req.PageNumber,
				req.PageSize,
				req.EndpointId),
			cancellationToken);

       
    }
}
