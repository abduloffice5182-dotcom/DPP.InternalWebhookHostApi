namespace DPP.InternalWebhookHost.Application.Operations.Queries.Requests;
public class SortWithPageParameter
{
	public int PageSize { get; set; } = 100;
	public int PageNumber { get; set; } = 1;
}
