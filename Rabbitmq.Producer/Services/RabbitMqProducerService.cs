using DPP.InternalWebhookHost.Rabbitmq.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace DPP.InternalWebhookHost.Rabbitmq.Services
{
	public class RabbitMqProducerService
   : IRabbitMqProducerService
	{
		private readonly IConfiguration configuration;

		private IConnection connection;
		readonly ILogger<RabbitMqProducerService> logger;

		public RabbitMqProducerService(
			IConfiguration configuration, ILogger<RabbitMqProducerService> logger, IConnection connection)
		{
			this.configuration = configuration;
			this.logger = logger;
			this.connection = connection;

			this.connection.ConnectionShutdownAsync += (_, e) =>
			{
				logger.LogWarning(
					"RabbitMQ Connection Shutdown. ReplyCode: {ReplyCode}, ReplyText: {ReplyText}",
					e.ReplyCode,
					e.ReplyText);

				return Task.CompletedTask;
			};
			connection.CallbackExceptionAsync += (_, e) =>
			{
				logger.LogError(
					e.Exception,
					"RabbitMQ Callback Exception");

				return Task.CompletedTask;
			};

		}

		public async Task PublishAsync<T>(
			string exchange,
				string queue,
			string routingKey, 
			T message)
		{  
			await using var channel = await connection.CreateChannelAsync(); 
			
			await CreateExchangeAndQueues(channel, exchange, queue, routingKey);
			 
			var json = JsonSerializer.Serialize(message); 
			var body = Encoding.UTF8.GetBytes(json); 
			var properties =
				new BasicProperties
				{
					Persistent = true
				};

			logger.LogInformation("RabbitMQ Message Publishing...");
			await channel.BasicPublishAsync(
				exchange: exchange,
				routingKey: routingKey,
				mandatory: true,
				basicProperties: properties,
				body: body);

			logger.LogInformation("RabbitMQ Message Published...");
		}
		 
		private async Task<IChannel> CreateExchangeAndQueues(IChannel channel, string exchange, string queue, string routingkey)
		{
			await channel.ExchangeDeclareAsync(
				exchange: exchange,
				type: ExchangeType.Topic,
				durable: true);
			  

			return channel;
		}
	}
}
