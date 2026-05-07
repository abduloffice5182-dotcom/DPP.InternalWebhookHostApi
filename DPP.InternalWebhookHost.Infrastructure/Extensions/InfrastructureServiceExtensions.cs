namespace DPP.InternalWebhookHost.Infrastructure.Extensions;
public static class InfrastructureServiceExtensions
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
	{
		// Register all Repository
		services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
		services.AddTransient<IWebhookRepository, WebhookRepository>();

		return services;
	}
}
