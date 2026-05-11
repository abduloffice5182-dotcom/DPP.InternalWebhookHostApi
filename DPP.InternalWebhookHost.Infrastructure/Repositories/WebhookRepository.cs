using DPP.InternalWebhookHost.Infrastructure.Constants.DatabaseQueries.Webhook;

namespace DPP.InternalWebhookHost.Infrastructure.Repositories;
public class WebhookRepository : IWebhookRepository
{
    private readonly IDbConnectionFactory dbConnection;
    public WebhookRepository(IDbConnectionFactory dbConnection, ILogger<WebhookRepository> logger)
    {
        this.dbConnection = dbConnection;
    }
    public async Task<Guid> WebhooklLogSave(SaveWebhookPayloadsRequest webhookLogRequest, CancellationToken cancellationToken)
    {
        using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);
        var parameters = new DynamicParameters();
        parameters.Add("@Payload", webhookLogRequest.Payload, DbType.String);
        parameters.Add("@QueryString", webhookLogRequest.QueryString, DbType.String);
        parameters.Add("@Endpoint", webhookLogRequest.Endpoint, DbType.String);

        return await conn.ExecuteScalarAsync<Guid>(WebhookQueries.WebhookLogSave,
        parameters, commandType: CommandType.Text);
    }


    public async Task<(int TotalCount, IEnumerable<dynamic> Items)> GetWebhookReportAsync(DateTime? start, DateTime? end, int pageNumber, int pageSize, CancellationToken ct)
    {
        using var conn = await dbConnection.GetCoreTransactionConnection(ct);
        var parameters = new
        {
            StartDateTime = start,
            EndDateTime = end,
            Offset = (pageNumber - 1) * pageSize,
            PageSize = pageSize
        };

        using var multi = await conn.QueryMultipleAsync(WebhookQueries.GetWebhooklLogs, parameters);

        return (
            await multi.ReadFirstAsync<int>(),
            await multi.ReadAsync<dynamic>()
        );
    }
}