using DPP.InternalWebhookHost.Rabbitmq.Interface;
using DPP.InternalWebhookHost.Rabbitmq.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DPP.InternalWebhookHost.Rabbitmq.Extension
{
	public static class RabbitmqServiceCollectionExtension
	{
		public static IServiceCollection AddCustomRabbitmq(this IServiceCollection services ,IConfiguration configuration)
		{
			services.AddSingleton<IConnection>(sp =>
			{
				var factory = new ConnectionFactory
				{
					HostName = configuration["RabbitMQ:Host"],
					Port = int.Parse(
						configuration["RabbitMQ:Port"]!),
					UserName = configuration["RabbitMQ:Username"],
					Password = configuration["RabbitMQ:Password"],
					ConsumerDispatchConcurrency = 1, 
					AutomaticRecoveryEnabled = true,
					NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
					TopologyRecoveryEnabled = true,
					RequestedHeartbeat = TimeSpan.FromSeconds(30)
				};
				return  factory.CreateConnectionAsync().GetAwaiter().GetResult();
			}); 

			services.AddSingleton<IRabbitMqProducerService,
	RabbitMqProducerService>();

			services.AddHostedService
	<RabbitmqConsumers>();

			return services;
		}
	}
}
