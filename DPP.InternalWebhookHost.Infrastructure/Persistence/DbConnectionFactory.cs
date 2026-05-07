namespace DPP.InternalWebhookHost.Infrastructure.Persistence;

public class DbConnectionFactory : IDbConnectionFactory
{
	private readonly string _coreTransaction;
	private readonly ILogger<DbConnectionFactory> _logger;

	public DbConnectionFactory(IConfiguration configuration, ILogger<DbConnectionFactory> logger)
	{
		_coreTransaction = configuration.GetConnectionString("CoreTransaction")!;
		_logger = logger;
	}

	public async Task<IDbConnection> GetCoreTransactionConnection(CancellationToken cancellationToken)
	{
		try
		{
			var connection = new SqlConnection(_coreTransaction);
			await connection.OpenAsync(cancellationToken);
			return connection;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to open CoreTransaction DB connection.");
			throw;
		}
	}
}
