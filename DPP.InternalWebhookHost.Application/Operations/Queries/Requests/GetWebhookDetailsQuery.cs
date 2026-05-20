using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Application.Operations.Queries.Requests
{
	public class GetWebhookDetailsQuery : IRequest<WebhookLogs>
	{
		public int Id { get; set; }
	}
}
