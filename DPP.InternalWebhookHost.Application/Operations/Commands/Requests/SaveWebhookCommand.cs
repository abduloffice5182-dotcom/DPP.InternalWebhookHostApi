using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Requests
{
	public class SaveWebhookCommand : IRequest<bool>
	{
		public string Payload { get; set; }
	}
}
