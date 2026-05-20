using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Application.Operations.Queries.Handlers
{
	public class GetWebhookDetailsQueryHandler : IRequestHandler<GetWebhookDetailsQuery, WebhookDetails?>
	{ 
		readonly IWebhookRepository webhookRepository;
		public GetWebhookDetailsQueryHandler(IWebhookRepository webhookRepository)
		{
			this.webhookRepository = webhookRepository;
		}
		public async Task<WebhookDetails?> Handle(GetWebhookDetailsQuery request, CancellationToken cancellationToken)
		{
			return await webhookRepository.GetWebhookDetailsAsync(new WebhookDetailsRequest(request.Id), cancellationToken);
		}
	}
}
