using Newtonsoft.Json;

namespace DPP.InternalWebhookHost.Domain.Common.Response;


public class ApiErrorResponse
{
	[JsonProperty("errors")]
	public ApiError Errors { get; set; }
}
public class ApiError
{
	[JsonProperty("message")]
	public string Message { get; set; }
	[JsonProperty("details")]
	public string Details { get; set; }
}
