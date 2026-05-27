namespace DPP.InternalWebhookHost.Infrastructure.Interfaces;
public interface IWebhookRepository
{
	Task WebhooklLogSave(SaveWebhookPayloadsRequest webhookLogRequest, CancellationToken cancellationToken);
	Task<WebhookListResponse> GetWebhookReportAsync(
		 WebhookLogRequest webhookLogRequest, CancellationToken cancellationToken);
	Task<WebhookDetails?> GetWebhookDetailsAsync(WebhookDetailsRequest webhookDetailsRequest, CancellationToken cancellationToken);

}
