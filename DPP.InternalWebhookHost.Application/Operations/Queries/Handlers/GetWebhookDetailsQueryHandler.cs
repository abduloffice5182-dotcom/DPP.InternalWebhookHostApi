using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Application.Operations.Queries.Handlers
{
	public class GetWebhookDetailsQueryHandler : IRequestHandler<GetWebhookDetailsQuery,WebhookLogs>
	{ 
		readonly IWebhookRepository webhookRepository;
		public GetWebhookDetailsQueryHandler(IWebhookRepository webhookRepository)
		{
			this.webhookRepository = webhookRepository;
		}
		public async Task<WebhookLogs> Handle(GetWebhookDetailsQuery request, CancellationToken cancellationToken)
		{
			return new WebhookLogs();
		}
	}
}
