using DPP.InternalWebhookHost.Application.Operations.Commands.Queries.Requests;
using DPP.InternalWebhookHost.Domain.Common.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Handlers
{
    public class GetWebhookReportHandler : IRequestHandler<GetWebhookPayloadsRequest, ApiResponse>
    {
        private readonly IDbConnection _dbConnection;

        public GetWebhookReportHandler(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ApiResponse> Handle(GetWebhookPayloadsRequest request, CancellationToken cancellationToken)
        {
         
            DateTime? start = request.FilterStartDatetime == default ? null : request.FilterStartDatetime;
            DateTime? end = request.FilterEndDatetime == default ? null : request.FilterEndDatetime;

            var parameters = new
            {
                StartDateTime = start,
                EndDateTime = end,
                Offset = (request.PageNumber - 1) * request.PageSize,
                PageSize = request.PageSize
            };

            //using var multi = await _dbConnection.QueryMultipleAsync(
            //    "dbo.usp_GetWebhookReport",
            //    parameters,
            //    commandType: CommandType.StoredProcedure
            //);

            const string sql = @" SELECT COUNT(*) FROM WebhookPayloads  WHERE (@StartDateTime IS NULL OR DateTimeReceived >= @StartDateTime) AND (@EndDateTime IS NULL OR DateTimeReceived <= @EndDateTime);
              SELECT Id, DateTimeReceived as ReceivedAt, Payload as Data FROM WebhookPayloads WHERE (@StartDateTime IS NULL OR DateTimeReceived >= @StartDateTime)
          AND (@EndDateTime IS NULL OR DateTimeReceived <= @EndDateTime) ORDER BY DateTimeReceived DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            // 3. Execute using QueryMultipleAsync
            using var multi = await _dbConnection.QueryMultipleAsync(sql, new
            {
                StartDateTime = start,
                EndDateTime = end,
                Offset = (request.PageNumber - 1) * request.PageSize,
                PageSize = request.PageSize
            });


            var totalCount = await multi.ReadFirstAsync<int>();
            //var items = await multi.ReadAsync<dynamic>();
            var items = (await multi.ReadAsync<dynamic>()).ToList();

            return new ApiResponse
            {
                Success = true,
                Response = new
                {
                    TotalCount = totalCount,
                    Items = items,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                },
                Message = "Webhook report retrieved successfully."
            };
        }
    }
}
