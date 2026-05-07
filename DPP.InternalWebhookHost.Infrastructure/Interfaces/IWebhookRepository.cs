namespace DPP.InternalWebhookHost.Infrastructure.Interfaces;
public interface IWebhookRepository
{
	Task<int> WebhoolLogSave(SaveWebhookPayloadsRequest webhookLogRequest, CancellationToken cancellationToken);
    Task<(int TotalCount, IEnumerable<dynamic> Items)> GetWebhookReportAsync(DateTime? start, DateTime? end, int pageNumber, int pageSize, CancellationToken cancellationToken);

}
