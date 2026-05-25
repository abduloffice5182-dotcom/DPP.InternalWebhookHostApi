using Polly;
using Polly.Extensions.Http;

namespace DPP.InternalWebhookHost.Api.Extension
{
	public static class RetryPolicyServiceExtension
	{
		public static void AddCustomRetryPolicy(this IServiceCollection services)
		{
			 
		}

		// Retry Policy
		static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
		{
			return HttpPolicyExtensions
				.HandleTransientHttpError()
				.WaitAndRetryAsync(
					retryCount: 3,
					sleepDurationProvider: retryAttempt =>
						TimeSpan.FromSeconds(retryAttempt),
					onRetry: (outcome, timespan, retryAttempt, context) =>
					{
						Console.WriteLine(
							$"Retry {retryAttempt} after {timespan.TotalSeconds}s");
					});
		}
	}
}
