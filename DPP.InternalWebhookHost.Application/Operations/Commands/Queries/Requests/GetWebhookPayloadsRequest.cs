using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPP.InternalWebhookHost.Domain.Common.Response;

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Queries.Requests;

public class GetWebhookPayloadsRequest : SortWithPageParameter , IRequest<ApiResponse>
{
	public DateTime FilterStartDatetime { get; set; }
	public DateTime FilterEndDatetime { get; set; }

}
