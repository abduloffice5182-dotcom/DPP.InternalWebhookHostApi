
using DPP.InternalWebhookHost.Rabbitmq.Consumer;
using DPP.InternalWebhookHost.Rabbitmq.Interface;
using DPP.InternalWebhookHost.Rabbitmq.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace DPP.InternalWebhookHost.Rabbitmq.Extension
{
	public static class RabbitmqServiceCollectionExtension
	{
		public static IServiceCollection AddCustomRabbitmq(this IServiceCollection services)
		{
			services.AddSingleton<IRabbitMqProducer,
	RabbitMqProducer>();

			services.AddHostedService
	<RabbitmqConsumers>();

			return services;
		}
	}
}
