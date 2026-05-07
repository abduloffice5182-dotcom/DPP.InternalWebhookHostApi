namespace DPP.InternalWebhookHost.Infrastructure.Persistence;
public interface IDbConnectionFactory
{
	Task<IDbConnection> GetCoreMerchantConnection(CancellationToken cancellationToken);
	Task<IDbConnection> GetCoreTransactionConnection(CancellationToken cancellationToken);
}
