

namespace DPP.InternalWebhookHost.Domain.Common.Response;
public record ApiResponse
(
	bool Success,
	int StatusCode,
	string? Message,
	dynamic? Data
);