using DPP.InternalWebhookHost.Application.Operations.Commands.Requests;
using DPP.InternalWebhookHost.Domain.Entities.Request;
using DPP.InternalWebhookHost.Infrastructure.Interfaces; 
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		var webhookLog = new GetWebhookPayloadsRequest
		{
			Payload = request.Payload
		};

		return await webhookRepository.WebhoolLogSave(webhookLog, cancellationToken); ;
	}
}
