namespace DPP.InternalWebhookHost.Application.Operations.Queries.Handlers;
public class GetWebhookReportHandler : IRequestHandler<GetWebhookReportQuery, IEnumerable<WebhookLogsResponse>>
{

	private readonly IWebhookRepository repository;

	public GetWebhookReportHandler(IWebhookRepository repository)
	{
		this.repository = repository;
	}

	public async Task<IEnumerable<WebhookLogsResponse>> Handle(
	GetWebhookReportQuery req,
	CancellationToken cancellationToken)
	{
		var request = await repository.GetWebhookReportAsync(
			new WebhookLogRequest(
				req.FromDate,
				req.ToDate,
				req.PageNumber,
				req.PageSize),
			cancellationToken);

		var response = request.Select(x =>
		{
			using var doc = JsonDocument.Parse(x.Payload);

			return new WebhookLogsResponse
			{
				Id = x.Id,
				DateTimeReceived = x.DateTimeReceived,  
				Payload = doc.RootElement.Clone()
			};
		});

		return response;
	}
}
