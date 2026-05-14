namespace DPP.InternalWebhookHost.Application.Operations.Queries.Requests;

public class GetWebhookReportQuery :  IRequest<IEnumerable<WebhookLogsResponse>>, IDateRange, IPagingParameter
{
	public DateTime FromDate { get; set; }
	public DateTime ToDate { get; set; }
	public int PageNumber { get ; set; }
	public int PageSize { get; set; }
}
