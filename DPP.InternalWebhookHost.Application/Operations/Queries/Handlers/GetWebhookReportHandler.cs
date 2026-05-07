using DPP.InternalWebhookHost.Application.Operations.Queries.Requests;
using DPP.InternalWebhookHost.Domain.Common.Response;
using DPP.InternalWebhookHost.Infrastructure.Interfaces;
using MediatR;
namespace DPP.InternalWebhookHost.Application.Operations.Queries.Handlers;
public class GetWebhookReportHandler : IRequestHandler<GetWebhookReportQuery, ApiResponse>
{

	private readonly IWebhookRepository repository;

	public GetWebhookReportHandler(IWebhookRepository repository)
	{
		this.repository = repository;
	}

	public async Task<ApiResponse> Handle(GetWebhookReportQuery request, CancellationToken cancellationToken)
	{
		DateTime? start = request.FilterStartDatetime == default ? null : request.FilterStartDatetime;
		DateTime? end = request.FilterEndDatetime == default ? null : request.FilterEndDatetime;

		var result = await repository.GetWebhookReportAsync(start, end, request.PageNumber, request.PageSize, cancellationToken);

		return new ApiResponse
		(
			Success: true,
			HttpStatusCode: 200,
			Response: new
			{
				result.TotalCount,
				Items = result.Items.ToList(),
				request.PageNumber,
				request.PageSize
			},
			Message: "Webhook report retrieved successfully."
		);
	}

}
