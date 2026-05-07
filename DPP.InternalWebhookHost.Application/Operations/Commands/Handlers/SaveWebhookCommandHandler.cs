using DPP.InternalWebhookHost.Application.Operations.Commands.Requests;
using DPP.InternalWebhookHost.Domain.Entities.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Handlers
{
	public class SaveWebhookCommandHandler
	: IRequestHandler<SaveWebhookCommand, bool>
	{

		public SaveWebhookCommandHandler()
		{
		}

		public async Task<bool> Handle(
			SaveWebhookCommand request,
			CancellationToken cancellationToken)
		{
			var webhookLog = new WebhookLogRequest
			{
				Payload = request.Payload
			};

			//await _dbContext.WebhookLogs.AddAsync(webhookLog);
			//await _dbContext.SaveChangesAsync(cancellationToken);

			return true;
		}
	}
