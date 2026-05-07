using DPP.InternalWebhookHost.Domain.Entities.Request;
using DPP.InternalWebhookHost.Infrastructure.Interfaces;
using DPP.InternalWebhookHost.Infrastructure.Persistence;
namespace DPP.InternalWebhookHost.Infrastructure.Repositories;
public class WebhookRepository : IWebhookRepository
{
	private readonly IDbConnectionFactory _dbConnection;
	private readonly ILogger<WebhookRepository> _logger;

	public WebhookRepository(IDbConnectionFactory dbConnection, ILogger<WebhookRepository> logger)
	{
		_dbConnection = dbConnection;
		_logger = logger;
	}
	public async Task<int> WebhoolLogSave(WebhookLogRequest webhookLogRequest, CancellationToken cancellationToken)
	{
		using var conn = await _dbConnection.GetCoreMerchantConnection(cancellationToken);
		var parameters = new DynamicParameters();
		parameters.Add("@Payload", webhookLogRequest.Payload);

		return 1;
		//return await conn.ExecuteAsync(DbQueries.WebhoookLogSave,
		//	parameters, commandType: CommandType.Text);
	}
}