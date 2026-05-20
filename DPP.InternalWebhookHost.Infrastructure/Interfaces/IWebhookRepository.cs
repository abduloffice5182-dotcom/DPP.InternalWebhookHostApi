namespace DPP.InternalWebhookHost.Infrastructure.Interfaces;
public interface IWebhookRepository
{
	Task WebhooklLogSave(SaveWebhookPayloadsRequest webhookLogRequest, CancellationToken cancellationToken);
    Task<IEnumerable<WebhookLogs>> GetWebhookReportAsync(WebhookLogRequest webhookLogRequest, CancellationToken cancellationToken);
	Task<WebhookLogs?> GetWebhookDetailsAsync(WebhookDetailsRequest webhookDetailsRequest, CancellationToken cancellationToken);

}
