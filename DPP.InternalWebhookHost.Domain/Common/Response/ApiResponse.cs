

namespace DPP.InternalWebhookHost.Domain.Common.Response;
public record ApiResponse
(
	bool Success,
	int HttpStatusCode,
	string? Message,
	dynamic Response
);