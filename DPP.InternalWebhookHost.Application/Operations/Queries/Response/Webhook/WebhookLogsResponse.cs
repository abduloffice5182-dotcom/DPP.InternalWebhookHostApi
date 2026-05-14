namespace DPP.InternalWebhookHost.Application.Operations.Queries.Response.Webhook;
public record WebhookLogsResponse
{ 
	public DateTime DateTimeReceived { get; set; }
	public JsonElement Payload { get; set; }
}
