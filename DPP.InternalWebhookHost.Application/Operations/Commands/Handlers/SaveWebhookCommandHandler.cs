using DPP.InternalWebhookHost.Application.Operations.Commands.Requests;
using DPP.InternalWebhookHost.Domain.Entities.Request;
using DPP.InternalWebhookHost.Infrastructure.Interfaces;
using MediatR;

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Handlers;
public class SaveWebhookCommandHandler: IRequestHandler<SaveWebhookCommand, Guid>
{
	private readonly IWebhookRepository webhookRepository;

	public SaveWebhookCommandHandler(IWebhookRepository webhookRepository)
	{
		this.webhookRepository = webhookRepository;
	}

	public async Task<Guid> Handle( SaveWebhookCommand request, CancellationToken cancellationToken)
	{
		return await webhookRepository.WebhooklLogSave(new SaveWebhookPayloadsRequest(request.Payload, request.QueryString, request.Endpoint), cancellationToken); 
	}
}
