namespace DPP.InternalWebhookHost.Infrastructure.Interfaces;

public interface IDbPollyPolicies
{
	AsyncPolicy RetryPolicy();
	AsyncPolicy CircuitBreakerPolicy();
}
