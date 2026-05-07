using DPP.InternalWebhookHost.Application.Operations.Commands.Queries.Requests;
using DPP.InternalWebhookHost.Domain.Common.Response;
using DPP.InternalWebhookHost.Infrastructure.Interfaces;
using MediatR;
namespace DPP.InternalWebhookHost.Application.Operations.Commands.Handlers;
public class GetWebhookReportHandler : IRequestHandler<GetWebhookPayloadsRequest, ApiResponse>
{

	private readonly IWebhookRepository repository;

	public GetWebhookReportHandler(IWebhookRepository repository)
	{
		this.repository = repository;
	}

	public async Task<ApiResponse> Handle(GetWebhookPayloadsRequest request, CancellationToken cancellationToken)
	{
		DateTime? start = request.FilterStartDatetime == default ? null : request.FilterStartDatetime;
		DateTime? end = request.FilterEndDatetime == default ? null : request.FilterEndDatetime;

		// Call Infrastructure via the Interface
		var result = await repository.GetWebhookReportAsync(start, end, request.PageNumber, request.PageSize, cancellationToken);

		return new ApiResponse
		(
			Success: true,
			HttpStatusCode: 200,
			Response: new
			{
				TotalCount = result.TotalCount,
				Items = result.Items.ToList(),
				PageNumber = request.PageNumber,
				PageSize = request.PageSize
			},
			Message: "Webhook report retrieved successfully."
		);
	}

}
