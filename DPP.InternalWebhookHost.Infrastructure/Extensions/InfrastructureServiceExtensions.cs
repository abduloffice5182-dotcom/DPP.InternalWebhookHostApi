namespace DPP.InternalWebhookHost.Infrastructure.Extensions;
public static class InfrastructureServiceExtensions
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
	{
		services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
		services.AddScoped<IWebhookRepository, WebhookRepository>();

		return services;
	}
}
