using DPP.InternalWebhookHost.Domain.Entities.Request;
using DPP.InternalWebhookHost.Infrastructure.Interfaces;
using DPP.InternalWebhookHost.Infrastructure.Persistence;
using DPP.PartnerPaymentIntegration.Infrastructure;
namespace DPP.InternalWebhookHost.Infrastructure.Repositories;
public class WebhookRepository : IWebhookRepository
{
	private readonly IDbConnectionFactory dbConnection;
	private readonly ILogger<WebhookRepository> logger;

	public WebhookRepository(IDbConnectionFactory dbConnection, ILogger<WebhookRepository> logger)
	{
		this.dbConnection = dbConnection;
		this.logger = logger;
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
        //const string sql = @"
        //    SELECT COUNT(*) FROM WebhookPayloads 
        //    WHERE (@StartDateTime IS NULL OR DateTimeReceived >= @StartDateTime) 
        //      AND (@EndDateTime IS NULL OR DateTimeReceived <= @EndDateTime);

        //    SELECT Id, DateTimeReceived as ReceivedAt, Payload as Data 
        //    FROM WebhookPayloads 
        //    WHERE (@StartDateTime IS NULL OR DateTimeReceived >= @StartDateTime)
        //      AND (@EndDateTime IS NULL OR DateTimeReceived <= @EndDateTime) 
        //    ORDER BY DateTimeReceived DESC 
        //    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        //using var multi = await _dbConnection.QueryMultipleAsync(sql, new
        //{
        //    StartDateTime = start,
        //    EndDateTime = end,
        //    Offset = (pageNumber - 1) * pageSize,
        //    PageSize = pageSize
        //});
        using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);

        var results = await conn.QueryMultipleAsync(DbQueries.GetWebhoolLogs, new
        {
            StartDateTime = start,
            EndDateTime = end,
            Offset = (pageNumber - 1) * pageSize,
            PageSize = pageSize
        });

        var totalCount = await results.ReadFirstAsync<int>();
        var items = await results.ReadAsync<dynamic>();

        return (totalCount, items);
    }
}