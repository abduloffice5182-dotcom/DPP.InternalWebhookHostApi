namespace DPP.InternalWebhookHost.Infrastructure.Constants.DatabaseQueries.Webhook;
public static class WebhookQueries
{
    public const string WebhookLogSave = @"INSERT INTO [CoreTransaction].[dbo].[WebHookPayloads]
                                                ([Payload], [EndpointId] ) 
                                                VALUES
                                                (@Payload,@EndpointId);";


    public const string GetWebhooklLogs = @"SELECT  
                                                DateTimeReceived,
                                                Payload
                                            FROM WebhookPayloads WITH (NOLOCK)
                                            WHERE DateTimeReceived >= @StartDateTime
                                            AND DateTimeReceived <= @EndDateTime
                                            ORDER BY Id DESC
                                            OFFSET ((@PageNumber - 1) * @PageSize) ROWS
                                            FETCH NEXT @PageSize ROWS ONLY;";

}


