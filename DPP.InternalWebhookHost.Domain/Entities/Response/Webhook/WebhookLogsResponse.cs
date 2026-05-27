using System.Text.Json;

namespace DPP.InternalWebhookHost.Domain.Entities.Response.Webhook;

public class WebhookLogs
{ 
	public long Id { get; set; }
	public DateTime DateTimeReceived { get; set; }
	//public JsonElement Payload { get; set; }
	public string EndpointId { get; set; }
}
 
public record WebhookListResponse (IEnumerable<WebhookLogs> WebhookLogs , long TotalCount);

public class WebhookDetails
{
	public int Id { get; set; }
	public DateTime DateTimeReceived { get; set; }
	public JsonElement Payload { get; set; }
	public string EndpointId { get; set; }
}
