namespace DPP.InternalWebhookHost.Infrastructure.Repositories;
public class WebhookRepository : IWebhookRepository
{
	readonly IDbConnectionFactory dbConnection;
	readonly IConfiguration configuration;
	public WebhookRepository(IDbConnectionFactory dbConnection, IConfiguration configuration)
	{
		this.dbConnection = dbConnection;
		this.configuration = configuration;
	}
	public async Task<Guid> WebhooklLogSave(SaveWebhookPayloadsRequest webhookLogRequest,
		CancellationToken cancellationToken)
	{
		using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);
		var parameters = new DynamicParameters();
		parameters.Add("@Payload", webhookLogRequest.Payload);
		parameters.Add("@EndpointId", webhookLogRequest.EndpointId);

		return await conn.ExecuteScalarAsync<Guid>(WebhookQueries.WebhookLogSave,
		parameters, commandType: CommandType.Text, commandTimeout: configuration.GetValue<int?>(ApiConfigurationConstant.SqlConnectionTimeout) ?? 30);
	}

	public async Task<IEnumerable<WebhookLogs>> GetWebhookReportAsync(WebhookLogRequest webhookLogRequest, CancellationToken cancellationToken)
	{
		using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);
		var parameters = new DynamicParameters();
		parameters.Add("@StartDateTime", webhookLogRequest.FromDate);
		parameters.Add("@EndDateTime", webhookLogRequest.ToDate);
		parameters.Add("@PageNumber", webhookLogRequest.PageNumber);
		parameters.Add("@PageSize", webhookLogRequest.PageSize);

		return await conn.QueryAsync<WebhookLogs>
			(WebhookQueries.GetWebhooklLogs, parameters, commandType: CommandType.Text, commandTimeout: configuration.GetValue<int?>(ApiConfigurationConstant.SqlConnectionTimeout) ?? 30);
	}
}