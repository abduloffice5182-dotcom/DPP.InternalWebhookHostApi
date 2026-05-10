using MediatR;

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Requests;

public class SaveWebhookCommand : IRequest<Guid>
{
	public string Payload { get; set; }
	public string? QueryString { get; set; }
	public string? Endpoint { get; set; }
}

