using DPP.InternalWebhookHost.Infrastructure.Helper;
using Polly;
using Polly.Retry;

namespace DPP.InternalWebhookHost.Infrastructure.Repositories;

public class BaseRepository
{
	private readonly IDbConnectionFactory dbConnection;
	private readonly int sqlConnectionTimeout; 
	readonly IDbPollyPolicies pollyPolicies;
	public BaseRepository(IDbConnectionFactory dbConnectionFactory, IConfiguration configuration,IDbPollyPolicies pollyPolicies)
	{
		this.dbConnection = dbConnectionFactory;
		sqlConnectionTimeout = configuration.GetValue<int?>(ApiConfigurationConstant.SqlConnectionTimeout) ?? 30;
		this.pollyPolicies = pollyPolicies;
  
	}

	protected async Task<int> ExecuteAsync(string sql, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
	{
		var policy = Policy.WrapAsync(
	 pollyPolicies.RetryPolicy(),
	 pollyPolicies.CircuitBreakerPolicy());

		return await policy.ExecuteAsync(async () =>
		{
			using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken); 

			return await conn.ExecuteAsync(sql,
			  param, commandType: commandType ?? CommandType.Text, commandTimeout: sqlConnectionTimeout);
		});  
	}

	protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
	{
		var policy = Policy.WrapAsync(
 pollyPolicies.RetryPolicy(),
 pollyPolicies.CircuitBreakerPolicy());

		return await policy.ExecuteAsync(async () =>
		{
			using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);

			return await conn.QueryAsync<T>(sql,
			  param, commandType: commandType ?? CommandType.Text, commandTimeout: sqlConnectionTimeout);
		});
	}
	protected async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, CommandType? commandType = null, CancellationToken cancellationToken = default)
	{

		var policy = Policy.WrapAsync(
 pollyPolicies.RetryPolicy(),
 pollyPolicies.CircuitBreakerPolicy());

		return await policy.ExecuteAsync(async () =>
		{
			using var conn = await dbConnection.GetCoreTransactionConnection(cancellationToken);

			return await conn.QuerySingleOrDefaultAsync<T>(sql,
			  param, commandType: commandType ?? CommandType.Text, commandTimeout: sqlConnectionTimeout);
		}); 
	}


	protected async Task<T> QueryMultipleAsync<T>(
		string sql,
		Func<SqlMapper.GridReader, Task<T>> handler,
		object? param = null,
		CommandType? commandType = null,
		CancellationToken cancellationToken = default)
	{
		var policy = Policy.WrapAsync(
			pollyPolicies.RetryPolicy(),
			pollyPolicies.CircuitBreakerPolicy());

		return await policy.ExecuteAsync(async () =>
		{
			 using var conn =
				await dbConnection.GetCoreTransactionConnection(cancellationToken);

			using var multi = await conn.QueryMultipleAsync(
				sql,
				param,
				commandType: commandType ?? CommandType.Text,
				commandTimeout: sqlConnectionTimeout);

			return await handler(multi);
		});
	}
}

