using DPP.InternalWebhookHost.Application.Common.Interfaces;
using DPP.InternalWebhookHost.Domain.Common.Response;
using MediatR;

namespace DPP.InternalWebhookHost.Application.Operations.Queries.Requests;

public class GetWebhookReportQuery : PagingParameters, IRequest<ApiResponse<object>> , ICommonFilterRequest
{
	public DateTime? FromDate { get; set; }
	public DateTime? ToDate { get; set; }

}
