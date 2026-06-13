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
WHERE
    (@StartDateTime IS NULL
        OR DateTimeReceived >= @StartDateTime)
AND (@EndDateTime IS NULL
        OR DateTimeReceived <= @EndDateTime)
AND (@Endpoint IS NULL
        OR EndpointId LIKE @Endpoint + '%')
ORDER BY DateTimeReceived DESC
OFFSET (@PageNumber - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;

SELECT COUNT(Id) AS TotalCount
FROM WebhookPayloads WITH (NOLOCK)
WHERE
    (@StartDateTime IS NULL
        OR DateTimeReceived >= @StartDateTime)
AND (@EndDateTime IS NULL
        OR DateTimeReceived <= @EndDateTime)
AND (@Endpoint IS NULL
        OR EndpointId LIKE @Endpoint + '%');
";

	public const string GetWebhookDetails = @"SELECT TOP(1)  
                                                Id,
                                                EndpointId,
                                                Payload,
                                                DateTimeReceived 
                                            FROM WebhookPayloads WITH (NOLOCK)
                                            WHERE Id = @Id";

}


