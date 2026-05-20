namespace DPP.InternalWebhookHost.Infrastructure.Repositories;

public class BaseRepository
{
	private readonly IDbConnectionFactory dbConnection;
	private readonly int sqlConnectionTimeout;
	public BaseRepository(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration)
	{
		this.dbConnection = dbConnectionFactory;
		sqlConnectionTimeout = configuration.GetValue<int?>(ApiConfigurationConstant.SqlConnectionTimeout) ?? 30;
	}

	protected async Task<int> ExecuteAsync(string sql, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
	{
		using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);

		return await conn.ExecuteAsync(sql,
		  param, commandType: commandType ?? CommandType.Text, commandTimeout: sqlConnectionTimeout);
	}

	protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
	{
		using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);

		return await conn.QueryAsync<T>(sql,
		  param, commandType: commandType ?? CommandType.Text, commandTimeout: sqlConnectionTimeout);
	}
	protected async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
	{
		using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);

		return await conn.QuerySingleOrDefaultAsync<T>(sql,
		  param, commandType: commandType ?? CommandType.Text, commandTimeout: sqlConnectionTimeout);

	}
}

