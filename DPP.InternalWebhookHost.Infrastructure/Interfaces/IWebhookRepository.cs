using DPP.InternalWebhookHost.Domain.Common.Response.Webhook;
using DPP.InternalWebhookHost.Domain.Entities.Request.Webhook;

namespace DPP.InternalWebhookHost.Infrastructure.Interfaces;
public interface IWebhookRepository
{
	Task<Guid> WebhooklLogSave(SaveWebhookPayloadsRequest webhookLogRequest, CancellationToken cancellationToken);
    Task<IEnumerable<WebhookLogsResponse>> GetWebhookReportAsync(WebhookLogRequest webhookLogRequest, CancellationToken cancellationToken);

}
