using System.Text.Json;

namespace DPP.InternalWebhookHost.Domain.Entities.Response.Webhook;

public class WebhookLogs
{ 
	public DateTime DateTimeReceived { get; set; }
	public JsonElement Payload { get; set; }
}
