using MediatR;

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Requests;

public class SaveWebhookCommand : IRequest<int>
{
	public string Payload { get; set; }
}

