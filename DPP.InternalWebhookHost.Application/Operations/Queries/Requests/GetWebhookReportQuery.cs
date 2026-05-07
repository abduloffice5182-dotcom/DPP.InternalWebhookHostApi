using MediatR;
using DPP.InternalWebhookHost.Domain.Common.Response;

namespace DPP.InternalWebhookHost.Application.Operations.Queries.Requests;

public class GetWebhookReportQuery : SortWithPageParameter , IRequest<ApiResponse>
{
	public DateTime FilterStartDatetime { get; set; }
	public DateTime FilterEndDatetime { get; set; }

}
