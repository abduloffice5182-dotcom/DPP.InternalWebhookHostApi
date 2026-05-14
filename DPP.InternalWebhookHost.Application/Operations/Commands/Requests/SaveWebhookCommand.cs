namespace DPP.InternalWebhookHost.Application.Operations.Commands.Requests;
public class SaveWebhookCommand : IRequest<Guid>
{
	public string Payload { get; set; } 
	public string EndpointId { get; set; }
}

