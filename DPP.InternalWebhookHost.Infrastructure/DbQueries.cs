namespace DPP.PartnerPaymentIntegration.Infrastructure
{
    public static class DbQueries
    {
        public const string WebhookLogSave = @"
            INSERT INTO [CoreTransaction].[dbo].[WebHookPayloads]
            (
                [DateReceived],
                [DateTimeReceived],
                [Payload]
            )
            VALUES
            (
                CAST(GETDATE() AS DATE),
                GETDATE(),
                @Payload
            );";



        public const string GetWebhoolLogs = @"
                SELECT COUNT(*) FROM WebhookPayloads WITH (NOLOCK)
                WHERE (@StartDateTime IS NULL OR DateTimeReceived >= @StartDateTime)
                AND (@EndDateTime IS NULL OR DateTimeReceived <= @EndDateTime);

                SELECT 
                        Id, 
                        DateTimeReceived AS ReceivedAt, 
                        Payload AS Data 
                FROM WebhookPayloads WITH (NOLOCK)
                WHERE (@StartDateTime IS NULL OR DateTimeReceived >= @StartDateTime)
                AND (@EndDateTime IS NULL OR DateTimeReceived <= @EndDateTime)
                ORDER BY DateTimeReceived DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
    }
}
