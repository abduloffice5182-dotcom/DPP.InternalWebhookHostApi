using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Requests;

public class GetWebhookPayloadsRequest : SortWithPageParameter
{
	public DateTime FilterStartDatetime { get; set; }
	public DateTime FilterEndDatetime { get; set; }

}


