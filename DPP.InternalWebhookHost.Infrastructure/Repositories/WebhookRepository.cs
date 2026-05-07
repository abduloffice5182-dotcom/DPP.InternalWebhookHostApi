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
	public async Task<int> WebhoolLogSave(GetWebhookPayloadsRequest webhookLogRequest, CancellationToken cancellationToken)
	{
		using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);
		var parameters = new DynamicParameters();
		parameters.Add("@Payload", webhookLogRequest.Payload, DbType.String);

		return await conn.ExecuteAsync(DbQueries.WebhookLogSave,
		parameters, commandType: CommandType.Text);
	}
 
}