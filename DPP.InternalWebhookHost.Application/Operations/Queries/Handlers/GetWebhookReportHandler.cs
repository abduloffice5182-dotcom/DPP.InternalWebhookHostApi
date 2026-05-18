namespace DPP.InternalWebhookHost.Application.Operations.Queries.Handlers;
public class GetWebhookReportHandler : IRequestHandler<GetWebhookReportQuery, IEnumerable<WebhookLogs>>
{

	private readonly IWebhookRepository repository;

	public GetWebhookReportHandler(IWebhookRepository repository)
	{
		this.repository = repository;
	}

	public async Task<IEnumerable<WebhookLogs>> Handle( GetWebhookReportQuery req, CancellationToken cancellationToken)
	{
		return await repository.GetWebhookReportAsync(
			new WebhookLogRequest(
				req.FromDate,
				req.ToDate,
				req.PageNumber,
				req.PageSize),
			cancellationToken);

       
    }
}
