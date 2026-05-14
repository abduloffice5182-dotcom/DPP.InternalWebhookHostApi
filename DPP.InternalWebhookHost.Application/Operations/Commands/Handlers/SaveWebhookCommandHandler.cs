namespace DPP.InternalWebhookHost.Application.Operations.Commands.Handlers;
public class SaveWebhookCommandHandler : IRequestHandler<SaveWebhookCommand>
{
	private readonly IWebhookRepository webhookRepository;

	public SaveWebhookCommandHandler(IWebhookRepository webhookRepository)
	{
		this.webhookRepository = webhookRepository;
	}

	public async Task Handle(SaveWebhookCommand request, CancellationToken cancellationToken)
	{ 
		 await webhookRepository.WebhooklLogSave(new SaveWebhookPayloadsRequest(request.Payload , request.EndpointId), cancellationToken);
	}
}
