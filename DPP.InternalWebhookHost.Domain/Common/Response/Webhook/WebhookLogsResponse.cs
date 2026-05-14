using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Domain.Common.Response.Webhook
{
	public class WebhookLogsResponse
	{
		public string Id { get; set; }
		public DateTime DateTimeReceived { get; set; }	
		public string Payload { get; set; }
	}
}
