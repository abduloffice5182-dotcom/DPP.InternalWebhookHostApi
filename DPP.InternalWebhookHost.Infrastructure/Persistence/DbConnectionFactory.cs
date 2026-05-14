namespace DPP.InternalWebhookHost.Infrastructure.Persistence;

public class DbConnectionFactory : IDbConnectionFactory
{
	private readonly string _coreTransaction; 

	public DbConnectionFactory(IConfiguration configuration)
	{
		_coreTransaction = configuration.GetConnectionString("CoreTransaction")!; 
	}

	public async Task<IDbConnection> GetCoreTransactionConnection(CancellationToken cancellationToken)
	{
		var connection = new SqlConnection(_coreTransaction);
		await connection.OpenAsync(cancellationToken);

		return connection;
	}
}
