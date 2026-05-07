namespace DPP.InternalWebhookHost.Infrastructure.Repositories;
public class WebhookRepository : IWebhookRepository
{
	private readonly IDbConnectionFactory dbConnection;
	public WebhookRepository(IDbConnectionFactory dbConnection, ILogger<WebhookRepository> logger)
	{
		this.dbConnection = dbConnection;
	}
	public async Task<int> WebhoolLogSave(SaveWebhookPayloadsRequest webhookLogRequest, CancellationToken cancellationToken)
	{
		using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);
		var parameters = new DynamicParameters();
		parameters.Add("@Payload", webhookLogRequest.Payload, DbType.String);

		return await conn.ExecuteAsync(DbQueries.WebhookLogSave,
		parameters, commandType: CommandType.Text);
	}


    public async Task<(int TotalCount, IEnumerable<dynamic> Items)> GetWebhookReportAsync( DateTime? start,DateTime? end,int pageNumber , int pageSize , CancellationToken cancellationToken)
    {
        using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);
        int validPageSize = pageSize > 0 ? pageSize : 10;
        int validOffset = (pageNumber > 0 ? pageNumber - 1 : 0) * validPageSize;
       
        var results = await conn.QueryMultipleAsync(DbQueries.GetWebhoolLogs, new
        {
            StartDateTime = start,
            EndDateTime = end,
            Offset = validOffset,
            PageSize = validPageSize
        });

        var totalCount =  results.Read<int>().FirstOrDefault();
        var items =  results.Read<dynamic>();

        return (totalCount, items);
    }
}