using MediatR;
using DPP.InternalWebhookHost.Domain.Common.Response;

namespace DPP.InternalWebhookHost.Application.Operations.Queries.Requests;

public class GetWebhookReportQuery : PagingParameters, IRequest<ApiResponse>
{
	public DateTime FromDate { get; set; }
	public DateTime ToDate { get; set; }

}
