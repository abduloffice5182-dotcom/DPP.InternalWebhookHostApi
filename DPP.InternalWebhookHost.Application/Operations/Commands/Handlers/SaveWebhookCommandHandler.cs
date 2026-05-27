using DPP.InternalWebhookHost.Rabbitmq.Interface;
using Microsoft.Extensions.Configuration;

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Handlers;
public class SaveWebhookCommandHandler : IRequestHandler<SaveWebhookCommand>
{
	readonly IRabbitMqProducerService rabbitMqProducer;
	readonly IConfiguration configuration;
	readonly IWebhookRepository webhookRepository;

	public SaveWebhookCommandHandler(IConfiguration configuration, IRabbitMqProducerService rabbitMqProducer, IWebhookRepository webhookRepository)
	{
		this.rabbitMqProducer = rabbitMqProducer;
		this.configuration = configuration;
		this.webhookRepository = webhookRepository;
	}

	public async Task Handle(SaveWebhookCommand request, CancellationToken cancellationToken)
	{
		if (configuration.GetValue<bool?>("RabbitMQ:EnableRabbitMQ") ?? false)
		{
			await rabbitMqProducer.PublishAsync(configuration["RabbitMQ:Exchange"]!, configuration["WebhookCreatedQueue"]!, configuration.GetValue<string>("RabbitMQ:WebhookCreatedQueueRoutingKey")!, request);

		}
		else
		{
			await webhookRepository.WebhooklLogSave(new SaveWebhookPayloadsRequest(
				 Payload: request.Payload,
				 EndpointId: request.EndpointId
				)
			 , cancellationToken);
		}

	}
}
