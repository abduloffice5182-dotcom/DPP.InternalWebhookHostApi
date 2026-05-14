namespace DPP.InternalWebhookHost.Application.Operations.Queries.Response.Webhook;
public record WebhookLogsResponse
{
	public Guid Id { get; set; }
	public DateTime DateTimeReceived { get; set; }
	public JsonElement Payload { get; set; }
}
