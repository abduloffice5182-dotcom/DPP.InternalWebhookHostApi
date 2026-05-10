 CREATE DATABASE [CoreTransaction]

 use [CoreTransaction]

 DROP TABLE IF EXISTS [CoreTransaction].[dbo].[WebHookPayloads]
 
 CREATE TABLE [CoreTransaction].[dbo].[WebHookPayloads]
 (
 Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL DEFAULT NEWID(),
 DATE DATE NOT NULL DEFAULT CAST(SYSDATETIME() AS DATE),
 DateTimeReceived DATETIME2(7) NOT NULL DEFAULT SYSDATETIME(),
 Payload NVARCHAR(MAX),
 QueryString NVARCHAR(MAX),
 Endpoint NVARCHAR(255) NOT NULL
 )

 SET @NewId = NEWID()

 SELECT * FROM [CoreTransaction].[dbo].[WebHookPayloads]

 --@"INSERT INTO [CoreTransaction].[dbo].[WebHookPayloads]
 --                                               ( [Payload] )
 --                                               VALUES
 --                                               ( @Payload );";

 --   public const string GetWebhooklLogs = @"SELECT Id, DateTimeReceived, Payload
 --                                               INTO #TempLogs FROM WebhookPayloads WITH (NOLOCK)
 --                                               WHERE (@StartDateTime IS NULL OR DateTimeReceived >= @StartDateTime)
 --                                               AND (@EndDateTime IS NULL OR DateTimeReceived <= @EndDateTime);
 --                                               SELECT COUNT(*) FROM #TempLogs;
 --                                               SELECT Id, DateTimeReceived AS ReceivedAt, Payload AS Data
 --                                               FROM #TempLogs
 --                                               ORDER BY DateTimeReceived DESC
 --                                               OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

 --                                               DROP TABLE IF EXISTS #TempLogs;";


 
SP_HELP 'WebhookPayloads'   

--CREATE INDEX IX_WebhookPayloads_DateTimeReceived ON WebhookPayloads(DateTimeReceived);


--CREATE NONCLUSTERED INDEX IX_WebhookPayloads_DateTimeReceived
--ON WebhookPayloads (DateTimeReceived DESC)
--INCLUDE (Id, QueryString, Endpoint);
