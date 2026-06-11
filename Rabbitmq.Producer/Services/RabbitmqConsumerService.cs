using DPP.InternalWebhookHost.Infrastructure.Interfaces;
using DPP.InternalWebhookHost.Rabbitmq.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace DPP.InternalWebhookHost.Rabbitmq.Services;

public class RabbitmqConsumers : BackgroundService
{
	private readonly IConfiguration configuration;
	private readonly IServiceScopeFactory scopeFactory;
	private readonly ILogger<RabbitmqConsumers> logger;

	private IConnection? connection;
	private IChannel? consumerChannel;

	private string consumerTag = string.Empty;

	public RabbitmqConsumers(
		IConfiguration configuration,
		IServiceScopeFactory scopeFactory,
		ILogger<RabbitmqConsumers> logger)
	{
		this.configuration = configuration;
		this.scopeFactory = scopeFactory;
		this.logger = logger;
	}

	public override async Task StartAsync(
		CancellationToken cancellationToken)
	{
		await InitializeRabbitMqAsync();

		await base.StartAsync(cancellationToken);
	}

	private async Task InitializeRabbitMqAsync()
	{
		var factory = new ConnectionFactory
		{
			HostName = configuration["RabbitMQ:Host"],
			Port = int.Parse(configuration["RabbitMQ:Port"]!),
			UserName = configuration["RabbitMQ:Username"],
			Password = configuration["RabbitMQ:Password"],
			AutomaticRecoveryEnabled = true,
			TopologyRecoveryEnabled = true,
			NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
			RequestedHeartbeat = TimeSpan.FromSeconds(30)
		};

		connection = await factory.CreateConnectionAsync();
		consumerChannel = await connection.CreateChannelAsync();
		RegisterRabbitMqEvents();
		var exchange = configuration["RabbitMQ:Exchange"]!;
		var queue = configuration["RabbitMQ:WebhookCreatedQueue"]!;
		var routingKey = configuration["RabbitMQ:WebhookCreatedQueueRoutingKey"]!;
		var retryExchange = $"{exchange}.retry"; 
		var retryQueue = $"{queue}.retry";
		 
		// MAIN EXCHANGE
		await consumerChannel.ExchangeDeclareAsync(
			exchange: exchange,
			type: ExchangeType.Topic,
			durable: true);

		// RETRY EXCHANGE
		await consumerChannel.ExchangeDeclareAsync(
			exchange: retryExchange,
			type: ExchangeType.Direct,
			durable: true); 

		// RETRY QUEUE
		await consumerChannel.QueueDeclareAsync(
			queue: retryQueue,
			durable: true,
			exclusive: false,
			autoDelete: false,
			arguments: new Dictionary<string, object?>
			{
                // Retry after 30 seconds
                { "x-message-ttl", 30000 },

                // Return to main exchange
                { "x-dead-letter-exchange", exchange },

                // Return to original routing key
                { "x-dead-letter-routing-key", routingKey }
			});

		await consumerChannel.QueueBindAsync(
			queue: retryQueue,
			exchange: retryExchange,
			routingKey: routingKey);

		// MAIN QUEUE
		await consumerChannel.QueueDeclareAsync(
			queue: queue,
			durable: true,
			exclusive: false,
			autoDelete: false,
			arguments: new Dictionary<string, object?>
			{
                // Failed messages go to retry exchange
                { "x-dead-letter-exchange", retryExchange },

                // Retry routing key
                { "x-dead-letter-routing-key", routingKey }
			});

		await consumerChannel.QueueBindAsync(
			queue: queue,
			exchange: exchange,
			routingKey: routingKey);

		// LIMIT PARALLEL PROCESSING
		await consumerChannel.BasicQosAsync(
			prefetchSize: 0,
			prefetchCount: 30,
			global: false);

		logger.LogInformation(
			"RabbitMQ initialized successfully");
	}

	private void RegisterRabbitMqEvents()
	{
		if (connection != null)
		{
			connection.ConnectionShutdownAsync +=
				async (sender, args) =>
				{
					logger.LogWarning(
						"RabbitMQ connection shutdown: {Reason}",
						args.ReplyText);

					await Task.CompletedTask;
				};

			connection.CallbackExceptionAsync +=
				async (sender, args) =>
				{
					logger.LogError(
						args.Exception,
						"RabbitMQ callback exception");

					await Task.CompletedTask;
				};
		}

		if (consumerChannel != null)
		{
			consumerChannel.ChannelShutdownAsync +=
				async (sender, args) =>
				{
					logger.LogWarning(
						"RabbitMQ channel shutdown: {Reason}",
						args.ReplyText);

					await Task.CompletedTask;
				};
		}
	}

	protected override async Task ExecuteAsync(
		CancellationToken stoppingToken)
	{
		if (consumerChannel == null)
		{
			throw new InvalidOperationException(
				"Consumer channel not initialized");
		}

		var consumer =
			new AsyncEventingBasicConsumer(
				consumerChannel);

		consumer.ReceivedAsync +=
			async (sender, ea) =>
			{
				try
				{
					if (stoppingToken.IsCancellationRequested)
					{
						return;
					}

					var json =
						Encoding.UTF8.GetString(
							ea.Body.ToArray());

					logger.LogInformation(
						"Message received. DeliveryTag: {DeliveryTag}",
						ea.DeliveryTag);

					using var scope =
						scopeFactory.CreateScope();

					var repository =
						scope.ServiceProvider
							.GetRequiredService<IWebhookRepository>();

					var message =
						JsonConvert.DeserializeObject
							<SaveWebhookCommand>(json);

					if (message == null)
					{
						throw new JsonException(
							"Invalid message payload");
					}

					await repository.WebhooklLogSave(
						new Domain.Entities.Request.Webhook
							.SaveWebhookPayloadsRequest(
								message.Payload,
								message.EndpointId),
						stoppingToken);

					await consumerChannel.BasicAckAsync(
						deliveryTag: ea.DeliveryTag,
						multiple: false);

					logger.LogInformation(
						"Message processed successfully");
				}
				catch (SqlException ex)
				{
					logger.LogWarning(
						ex,
						"Transient SQL failure");

					await HandleRetryAsync(ea);
				}
				catch (TimeoutException ex)
				{
					logger.LogWarning(
						ex,
						"Timeout occurred");

					await HandleRetryAsync(ea);
				}
				catch (IOException ex)
				{
					logger.LogWarning(
						ex,
						"IO/network failure");

					await HandleRetryAsync(ea);
				}
				catch (JsonException ex)
				{
					logger.LogError(
						ex,
						"Invalid JSON payload");
					await HandleRetryAsync(ea);
				}
				catch (Exception ex)
				{
					logger.LogError(
						ex,
						"Permanent failure");

					await HandleRetryAsync(ea);
				}
			};

		consumerTag =
			await consumerChannel.BasicConsumeAsync(
				queue: configuration[
					"RabbitMQ:WebhookCreatedQueue"]!,
				autoAck: false,
				consumer: consumer);

		logger.LogInformation(
			"Consumer started. ConsumerTag: {ConsumerTag}",
			consumerTag);

		await Task.Delay(
			Timeout.Infinite,
			stoppingToken);
	}
	 

	private async Task HandleRetryAsync(
		BasicDeliverEventArgs ea)
	{

		logger.LogInformation(
			"Moving message to retry queue");

		await consumerChannel!.BasicNackAsync(
			deliveryTag: ea.DeliveryTag,
			multiple: false,
			requeue: false);
	}


	public override async Task StopAsync(
		CancellationToken cancellationToken)
	{
		logger.LogInformation(
			"Stopping RabbitMQ consumer");

		if (consumerChannel != null)
		{
			if (!string.IsNullOrWhiteSpace(
				consumerTag))
			{
				await consumerChannel.BasicCancelAsync(
					consumerTag);
			}

			await consumerChannel.CloseAsync();

			await consumerChannel.DisposeAsync();
		}

		if (connection != null)
		{
			await connection.CloseAsync();

			await connection.DisposeAsync();
		}

		await base.StopAsync(cancellationToken);
	}
}