using DPP.InternalWebhookHost.Application.Operations.Commands.Requests;
using DPP.InternalWebhookHost.Domain.Entities.Request;
using DPP.InternalWebhookHost.Infrastructure.Interfaces;
using MediatR; 

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Handlers;
public class SaveWebhookCommandHandler
: IRequestHandler<SaveWebhookCommand, int>
{
	private readonly IWebhookRepository webhookRepository;

	public SaveWebhookCommandHandler(IWebhookRepository webhookRepository)
	{
		this.webhookRepository = webhookRepository;
	}

	public async Task<int> Handle(
		SaveWebhookCommand request,
		CancellationToken cancellationToken)
	{
		return await webhookRepository.WebhoolLogSave(new SaveWebhookPayloadsRequest(request.Payload), cancellationToken); 
	}
}
