namespace DPP.InternalWebhookHost.Application.Operations.Queries.Handlers;
public class GetWebhookReportHandler : IRequestHandler<GetWebhookReportQuery, IEnumerable<WebhookLogsResponse>>
{

	private readonly IWebhookRepository repository;

	public GetWebhookReportHandler(IWebhookRepository repository)
	{
		this.repository = repository;
	}

	public async Task<IEnumerable<WebhookLogsResponse>> Handle( GetWebhookReportQuery req, CancellationToken cancellationToken)
	{
		var request = await repository.GetWebhookReportAsync(
			new WebhookLogRequest(
				req.FromDate,
				req.ToDate,
				req.PageNumber,
				req.PageSize),
			cancellationToken);

        return request.Select(x => new WebhookLogsResponse
        {
            Id = x.Id,
            DateTimeReceived = x.DateTimeReceived,
            Payload = JsonSerializer.Deserialize<JsonElement>(x.Payload)
        });
    }
}
