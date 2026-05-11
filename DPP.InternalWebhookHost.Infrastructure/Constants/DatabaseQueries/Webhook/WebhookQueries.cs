namespace DPP.InternalWebhookHost.Infrastructure.Constants.DatabaseQueries.Webhook;
public static class WebhookQueries
{
    public const string WebhookLogSave = @"INSERT INTO [CoreTransaction].[dbo].[WebHookPayloads]
                                                ( [Id],[Payload],[QueryString], [Endpoint] )
                                                OUTPUT INSERTED.Id
                                                VALUES
                                                ( NEWID(), @Payload, @QueryString, @Endpoint );";


    public const string GetWebhooklLogs = @"SELECT Id, DateTimeReceived AS ReceivedAt, Payload AS Data, QueryString, Endpoint
                                            INTO #TempFilteredLogs
                                            FROM WebhookPayloads WITH (NOLOCK)
                                            WHERE (@StartDateTime IS NULL OR DateTimeReceived >= @StartDateTime)
                                            AND (@EndDateTime IS NULL OR DateTimeReceived <= @EndDateTime);

                                            SELECT COUNT(*) FROM #TempFilteredLogs;

                                            SELECT * FROM #TempFilteredLogs
                                            ORDER BY ReceivedAt DESC
                                            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                                            DROP TABLE IF EXISTS #TempFilteredLogs;";

}


