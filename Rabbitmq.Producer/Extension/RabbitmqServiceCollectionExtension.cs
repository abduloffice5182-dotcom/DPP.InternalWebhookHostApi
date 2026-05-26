using DPP.InternalWebhookHost.Rabbitmq.Interface;
using DPP.InternalWebhookHost.Rabbitmq.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DPP.InternalWebhookHost.Rabbitmq.Extension
{
	public static class RabbitmqServiceCollectionExtension
	{
		public static IServiceCollection AddCustomRabbitmq(this IServiceCollection services)
		{
			services.AddSingleton<IRabbitMqProducerService,
	RabbitMqProducerService>();

			services.AddHostedService
	<RabbitmqConsumers>();

			return services;
		}
	}
}
