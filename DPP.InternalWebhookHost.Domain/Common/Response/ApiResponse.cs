namespace DPP.InternalWebhookHost.Domain.Common.Response;
public record ApiResponse<T>
(
	string? Message,
	T? Data
);