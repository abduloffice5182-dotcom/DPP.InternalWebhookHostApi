using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Rabbitmq.Model
{
	public class SaveWebhookCommand 
	{
		public string Payload { get; set; } = "";
		public string EndpointId { get; set; } = "";
	}
}
