namespace DPP.InternalWebhookHost.Infrastructure.Persistence;
public interface IDbConnectionFactory
{
	Task<IDbConnection> GetCoreTransactionConnection(CancellationToken cancellationToken);
}
