using DPP.InternalWebhookHost.Domain.Common.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Queries.Requests
{
    public record GetWebhookReportQuery(
            DateTime FilterStartDatetime,
            DateTime FilterEndDatetime,
            int PageNumber,
            int PageSize
        ) : IRequest<ApiResponse>;
}
