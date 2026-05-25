using Polly;
namespace DPP.InternalWebhookHost.Infrastructure.Helper;

public class DbPollyPolicies : IDbPollyPolicies
{ 
	public AsyncPolicy RetryPolicy()
	{
		return Policy
			   .Handle<SqlException>()
   .Or<TimeoutException>()
			   .WaitAndRetryAsync(
				   3,
				   retry => TimeSpan.FromSeconds(retry));

	}

	public AsyncPolicy CircuitBreakerPolicy()
	{
		return Policy
			.Handle<SqlException>()
	.Or<TimeoutException>()
				.CircuitBreakerAsync(
					5,
					TimeSpan.FromSeconds(30));
	}
}

