namespace DPP.InternalWebhookHost.Domain.Entities.Response.Webhook;

public class WebhookLogs
{
	public Guid Id { get; set; }
	public DateTime DateTimeReceived { get; set; }
	public string Payload { get; set; }
}
