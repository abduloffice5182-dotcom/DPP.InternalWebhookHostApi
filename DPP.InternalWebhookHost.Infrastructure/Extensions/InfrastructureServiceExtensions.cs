
using DPP.InternalWebhookHost.Infrastructure.Helper;

namespace DPP.InternalWebhookHost.Infrastructure.Extensions;
public static class InfrastructureServiceExtensions
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
	{
		services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
		services.AddSingleton<IDbPollyPolicies, DbPollyPolicies>();

		services.AddScoped<IWebhookRepository, WebhookRepository>();
		 
		return services;
	}
}
