using DPP.InternalWebhookHost.Rabbitmq.Interface;
using Microsoft.Extensions.Configuration;

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Handlers;
public class SaveWebhookCommandHandler : IRequestHandler<SaveWebhookCommand>
{
	readonly IRabbitMqProducer rabbitMqProducer;
	readonly IConfiguration configuration;

	public SaveWebhookCommandHandler(IConfiguration configuration, IRabbitMqProducer rabbitMqProducer)
	{
		this.rabbitMqProducer = rabbitMqProducer;
		this.configuration = configuration;
	}

	public async Task Handle(SaveWebhookCommand request, CancellationToken cancellationToken)
	{ 
		await rabbitMqProducer.PublishAsync(configuration.GetValue<string>("RabbitMQ:WebhookCreatedQueueRoutingKey")!, request); 
	}
}
