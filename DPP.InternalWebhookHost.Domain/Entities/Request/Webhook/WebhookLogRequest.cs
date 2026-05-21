

namespace DPP.InternalWebhookHost.Domain.Entities.Request.Webhook;

public record WebhookLogRequest(DateTime StartDateTime, DateTime EndDateTime, int PageNumber, int PageSize, string? Endpoint =null);
public record WebhookDetailsRequest(int Id);