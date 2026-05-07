

namespace DPP.InternalWebhookHost.Domain.Common.Response;
public class ApiResponse
{
	public bool Success { get; set; }
	public int HttpStatusCode { get; set; }
	public string? Message { get; set; } = null;
	public dynamic Response { get; set; } = null;
}

