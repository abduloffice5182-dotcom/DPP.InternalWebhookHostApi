using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Application.Operations.Commands.Requests
{
	public class SortWithPageParameter
	{
		public int PageSize { get; set; } = 100;
		public int PageNumber { get; set; } = 1;
		public string Search { get; set; } = null;
	}
}
