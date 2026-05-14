using Dapper;
using DPP.InternalWebhookHost.Domain.Common.Response.Webhook;
using DPP.InternalWebhookHost.Domain.Entities.Request.Webhook;
using DPP.InternalWebhookHost.Infrastructure.Constants.Configuration;
using DPP.InternalWebhookHost.Infrastructure.Constants.DatabaseQueries.Webhook;

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
		parameters.Add("@Endpoint", webhookLogRequest.EndpointId);

		return await conn.ExecuteScalarAsync<Guid>(WebhookQueries.WebhookLogSave,
		parameters, commandType: CommandType.Text, commandTimeout: configuration.GetValue<int?>(ApiConfigurationConstant.SqlConnectionTimeout) ?? 30);
	}

	public async Task<IEnumerable<WebhookLogsResponse>> GetWebhookReportAsync(WebhookLogRequest webhookLogRequest, CancellationToken cancellationToken)
	{
		using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);
		var parameters = new DynamicParameters();
		parameters.Add("@StartDateTime", webhookLogRequest.FromDate);
		parameters.Add("@EndDateTime", webhookLogRequest.ToDate);
		parameters.Add("@PageNumber", webhookLogRequest.PageNumber);
		parameters.Add("@PageSize", webhookLogRequest.PageSize);

		return await conn.QueryAsync<WebhookLogsResponse>
			(WebhookQueries.GetWebhooklLogs, parameters, commandType: CommandType.Text, commandTimeout: configuration.GetValue<int?>(ApiConfigurationConstant.SqlConnectionTimeout) ?? 30);
	}
}