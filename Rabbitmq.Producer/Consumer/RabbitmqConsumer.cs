using DPP.InternalWebhookHost.Infrastructure.Interfaces;
using DPP.InternalWebhookHost.Rabbitmq.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Rabbitmq.Consumer
{
	public class RabbitmqConsumers
	: BackgroundService
	{
		private readonly IConfiguration
			configuration;

		private readonly IServiceScopeFactory
			scopeFactory;

		private IConnection? connection;

		private IChannel? channel;

		public RabbitmqConsumers(
			IConfiguration configuration,
			IServiceScopeFactory scopeFactory)
		{
			this.configuration = configuration;
			this.scopeFactory = scopeFactory;
		}

		public override async Task StartAsync(
			CancellationToken cancellationToken)
		{
			var factory = new ConnectionFactory
			{
				HostName =
					configuration["RabbitMQ:Host"],

				Port =
					int.Parse(
						configuration["RabbitMQ:Port"]!),

				UserName =
					configuration["RabbitMQ:Username"],

				Password =
					configuration["RabbitMQ:Password"]
			};

			factory.AutomaticRecoveryEnabled = true;
			connection =
				await factory.CreateConnectionAsync();

			channel =
				await connection.CreateChannelAsync();

			var exchange =
				configuration["RabbitMQ:Exchange"]!;

			var queue =
				configuration[
					"RabbitMQ:WebhookCreatedQueue"]!;

			var routingKey =
				configuration[
					"RabbitMQ:WebhookCreatedQueueRoutingKey"]!;

			// Declare Topic Exchange
			await channel.ExchangeDeclareAsync(
				exchange: exchange,
				type: ExchangeType.Topic,
				durable: true);

			// Declare Queue
			await channel.QueueDeclareAsync(
				queue: queue,
				durable: true,
				exclusive: false,
				autoDelete: false);

			// Bind Queue To Exchange
			await channel.QueueBindAsync(
				queue: queue,
				exchange: exchange,
				routingKey: routingKey);

			// IMPORTANT
			// Prevent DB overload
			await channel.BasicQosAsync(
				prefetchSize: 0,
				prefetchCount: 10,
				global: false);

			await base.StartAsync(
				cancellationToken);
		}

		protected override async Task ExecuteAsync(
			CancellationToken stoppingToken)
		{
			var consumer =
				new AsyncEventingBasicConsumer(
					channel);

			consumer.ReceivedAsync +=
				async (sender, ea) =>
				{
					try
					{
						var json =
						Encoding.UTF8.GetString(
							ea.Body.ToArray());

						using var scope =
						scopeFactory.CreateScope();

						var repository =
						scope.ServiceProvider
							.GetRequiredService
							<IWebhookRepository>();

						var message = JsonConvert.DeserializeObject<SaveWebhookCommand>(json);
						await repository.WebhooklLogSave(new Domain.Entities.Request.Webhook.SaveWebhookPayloadsRequest(message.Payload, message.EndpointId)
						,
						stoppingToken);


						// SUCCESS ACK
						await channel.BasicAckAsync(
						deliveryTag:
							ea.DeliveryTag,
						multiple: false);
					}
					catch (Exception ex)
					{
						Console.WriteLine(
						ex.Message);

						// FAILURE
						await channel.BasicNackAsync(
						deliveryTag:
							ea.DeliveryTag,
						multiple: false,
						requeue: true);
					}
				};

			await channel.BasicConsumeAsync(
				queue:
					configuration[
						"RabbitMQ:WebhookCreatedQueue"]!,

				autoAck: false,

				consumer: consumer);
		}

		public override async Task StopAsync(
			CancellationToken cancellationToken)
		{
			if (channel is not null)
			{
				await channel.CloseAsync();
			}

			if (channel is not null)
			{
				await channel.CloseAsync();
			}

			await base.StopAsync(
				cancellationToken);
		}
	}
}
