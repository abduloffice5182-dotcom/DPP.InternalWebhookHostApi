using DPP.InternalWebhookHost.Rabbitmq.Interface;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace DPP.InternalWebhookHost.Rabbitmq.Services
{
	public class RabbitMqProducerService
   : IRabbitMqProducerService
	{
		private readonly IConfiguration configuration;

		private readonly ConnectionFactory factory;

		public RabbitMqProducerService(
			IConfiguration configuration)
		{
			this.configuration = configuration;

			factory = new ConnectionFactory
			{
				HostName = configuration["RabbitMQ:Host"], 
				Port = int.Parse(
						configuration["RabbitMQ:Port"]!), 
				UserName = configuration["RabbitMQ:Username"], 
				Password = configuration["RabbitMQ:Password"]
			
			};
			factory.AutomaticRecoveryEnabled = true;
		}

		public async Task PublishAsync<T>(
			string routingKey,
			T message)
		{
			await using var connection =
				await factory.CreateConnectionAsync();

			await using var channel =
				await connection.CreateChannelAsync();

			var exchange =
				configuration["RabbitMQ:Exchange"]!;

			// Declare Topic Exchange
			await channel.ExchangeDeclareAsync(
				exchange: exchange,
				type: ExchangeType.Topic,
				durable: true);

			var json =
				JsonSerializer.Serialize(message);

			var body =
				Encoding.UTF8.GetBytes(json);

			var properties =
				new BasicProperties
				{
					Persistent = true
				};

			await channel.BasicPublishAsync(
				exchange: exchange,
				routingKey: routingKey,
				mandatory: true,
				basicProperties: properties,
				body: body);
		}
	}
}
