using DPP.InternalWebhookHost.Domain.Entities.Request.Webhook;

namespace DPP.InternalWebhookHost.Infrastructure.Repositories;
public class WebhookRepository : BaseRepository, IWebhookRepository
{
	public WebhookRepository(IDbConnectionFactory dbConnection, IConfiguration configuration) : base(dbConnection, configuration)
	{
	}
	public async Task WebhooklLogSave(SaveWebhookPayloadsRequest webhookLogRequest,
		CancellationToken cancellationToken)
	{
		await ExecuteAsync(WebhookQueries.WebhookLogSave,
			webhookLogRequest, cancellationToken: cancellationToken);
	}

	public async Task<IEnumerable<WebhookLogs>> GetWebhookReportAsync(WebhookLogRequest webhookLogRequest, CancellationToken cancellationToken)
	{  
		return await QueryAsync<WebhookLogs>(
			WebhookQueries.GetWebhooklLogs, webhookLogRequest,cancellationToken:cancellationToken);
	}

	public async Task<WebhookLogs?> GetWebhookDetailsAsync(WebhookDetailsRequest webhookDetailsRequest, CancellationToken cancellationToken)
	{
		return await QueryFirstOrDefaultAsync<WebhookLogs>(WebhookQueries.GetWebhooklLogs, webhookDetailsRequest, cancellationToken: cancellationToken);
	}
}