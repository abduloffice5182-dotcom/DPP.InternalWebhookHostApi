

namespace DPP.InternalWebhookHost.Domain.Entities.Request.Webhook;

public record WebhookLogRequest(DateTime FromDate , DateTime ToDate , int PageNumber , int PageSize); 