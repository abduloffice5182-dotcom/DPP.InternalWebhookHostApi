using DPP.InternalWebhookHost.Domain.Entities.Request;
using DPP.Security.MultiAuth.Interfaces;

namespace DPP.PartnerPaymentIntegration.Infrastructure.Repositories
{
	public class WebhookRepository : IDepartmentRepository
	{
		private readonly IDbConnectionFactory _dbConnection;
		private readonly ILogger<WebhookRepository> _logger;

		public WebhookRepository(IDbConnectionFactory dbConnection, ILogger<WebhookRepository> logger)
		{
			_dbConnection = dbConnection;
			_logger = logger;
		}
		public async Task WebhoolLogSave(WebhookLogRequest webhookLogRequest, CancellationToken cancellationToken)
		{
			using var conn = await _dbConnection.GetCoreMerchantConnection(cancellationToken);
			var parameters = new DynamicParameters();
			parameters.Add("@Payload", webhookLogRequest.Payload);
			await conn.ExecuteAsync("", parameters, commandType: CommandType.Text);
		}
	}
}
