namespace DPP.InternalWebhookHost.Domain.Common.Response;
public record ApiResponse<T>
(
	bool Success,
	int StatusCode,
	string? Message,
	T? Data
);