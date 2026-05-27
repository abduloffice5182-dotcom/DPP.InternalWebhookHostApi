using Azure.Core;
using DPP.InternalWebhookHost.Domain.Entities.Request.Webhook;

namespace DPP.InternalWebhookHost.Infrastructure.Repositories;
public class WebhookRepository : BaseRepository, IWebhookRepository
{
	public WebhookRepository(IDbConnectionFactory dbConnection, IConfiguration configuration, IDbPollyPolicies pollyPolicies) : base(dbConnection, configuration, pollyPolicies)
	{
	}
	public async Task WebhooklLogSave(SaveWebhookPayloadsRequest webhookLogRequest,
		CancellationToken cancellationToken)
	{
		await ExecuteAsync(WebhookQueries.WebhookLogSave,
			webhookLogRequest, cancellationToken: cancellationToken);
	}

	public async Task<WebhookListResponse> GetWebhookReportAsync(
		WebhookLogRequest webhookLogRequest,
		CancellationToken cancellationToken)
	{

		return await QueryMultipleAsync(WebhookQueries.GetWebhooklLogs,
			async multi =>
			{

				var logs = (await multi.ReadAsync<WebhookLogs>());

				var totalCount =
					await multi.ReadFirstAsync<int>();

				return new WebhookListResponse(logs, totalCount);
			}, webhookLogRequest, null, cancellationToken
			);
		//return await QueryMultipleAsync(
		//	WebhookQueries.GetWebhooklLogs,
		//	async multi =>
		//	{
		//		var logs =
		//			(await multi.ReadAsync<WebhookLogs>()).ToList();

		//		var totalCount =
		//			await multi.ReadFirstAsync<int>();

		//		return (logs, totalCount);
		//	},
		//	 webhookLogRequest,null,
		//	 cancellationToken);

		//var response = await QueryMultipleAsync(
		//	WebhookQueries.GetWebhooklLogs, webhookLogRequest, cancellationToken: cancellationToken);

		//var res =  response.Read<WebhookLogs>();
		//var tt = ( response.Read<long>()).FirstOrDefault();

		//return new WebhookListResponse
		//	(
		//	WebhookLogs: await response.ReadAsync<WebhookLogs>(),
		//	TotalCount: (await response.ReadAsync<long>()).FirstOrDefault()
		//	);

	}

	public async Task<WebhookDetails?> GetWebhookDetailsAsync(WebhookDetailsRequest webhookDetailsRequest, CancellationToken cancellationToken)
	{
		return await QueryFirstOrDefaultAsync<WebhookDetails>(WebhookQueries.GetWebhookDetails, webhookDetailsRequest, cancellationToken: cancellationToken);
	}
}