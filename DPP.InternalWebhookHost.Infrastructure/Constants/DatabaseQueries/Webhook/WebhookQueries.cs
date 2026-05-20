namespace DPP.InternalWebhookHost.Infrastructure.Constants.DatabaseQueries.Webhook;
public static class WebhookQueries
{
    public const string WebhookLogSave = @"INSERT INTO [CoreTransaction].[dbo].[WebHookPayloads]
                                                ([Payload], [EndpointId] ) 
                                                VALUES
                                                (@Payload,@EndpointId);";


    public const string GetWebhooklLogs = @"SELECT  
                                                Id,
                                                EndpointId,
                                                DateTimeReceived 
                                            FROM WebhookPayloads WITH (NOLOCK)
                                            WHERE DateTimeReceived >= @StartDateTime
                                            AND DateTimeReceived <= @EndDateTime
                                            ORDER BY Id DESC
                                            OFFSET ((@PageNumber - 1) * @PageSize) ROWS
                                            FETCH NEXT @PageSize ROWS ONLY;";

	public const string GetWebhookDetails = @"SELECT TOP(1)  
                                                Id,
                                                EndpointId,
                                                Payload,
                                                DateTimeReceived 
                                            FROM WebhookPayloads WITH (NOLOCK)
                                            WHERE Id >= @Id";

}


