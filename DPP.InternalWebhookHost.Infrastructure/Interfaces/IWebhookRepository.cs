using DPP.InternalWebhookHost.Domain.Entities.Request;

namespace DPP.InternalWebhookHost.Infrastructure.Interfaces;
public interface IWebhookRepository
{
	Task<int> WebhoolLogSave(GetWebhookPayloadsRequest webhookLogRequest, CancellationToken cancellationToken);

	}
