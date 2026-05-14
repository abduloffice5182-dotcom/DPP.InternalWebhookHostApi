namespace DPP.InternalWebhookHost.Domain.Entities.Request.Webhook;


public record SaveWebhookPayloadsRequest(string Payload, string EndpointId);
