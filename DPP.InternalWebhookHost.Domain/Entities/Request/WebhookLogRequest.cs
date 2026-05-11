namespace DPP.InternalWebhookHost.Domain.Entities.Request;


public record SaveWebhookPayloadsRequest(string Payload, string? QueryString, string? Endpoint);
