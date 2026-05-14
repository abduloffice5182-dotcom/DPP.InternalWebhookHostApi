namespace DPP.InternalWebhookHost.Infrastructure.Constants.DatabaseQueries.Webhook;
public static class WebhookQueries
{
    public const string WebhookLogSave = @"INSERT INTO [CoreTransaction].[dbo].[WebHookPayloads]
                                                ( [Id],[Payload], [Endpoint] )
                                                OUTPUT INSERTED.Id
                                                VALUES
                                                (NEWID(), @Payload,@Endpoint);";


    public const string GetWebhooklLogs = @"SELECT 
                                                Id,
                                                DateTimeReceived AS ReceivedAt,
                                                Payload
                                            FROM WebhookPayloads WITH (NOLOCK)
                                            WHERE (@StartDateTime IS NULL 
                                                   OR DateTimeReceived >= @StartDateTime)
                                            AND (@EndDateTime IS NULL 
                                                 OR DateTimeReceived <= @EndDateTime)
                                            ORDER BY DateTimeReceived DESC
                                            OFFSET ((@PageNumber - 1) * @PageSize) ROWS
                                            FETCH NEXT @PageSize ROWS ONLY;";

}


