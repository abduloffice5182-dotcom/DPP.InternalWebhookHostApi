using DPP.PartnerPaymentIntegration.Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.PartnerPaymentIntegration.Application.Operations.Fortellis.Queries.Requests
{
    public class GetInvoiceAmountRequest : IRequest<GetInvoiceAmountResponse>
    {
        [JsonProperty("invoiceId")]
        public string InvoiceId { get; set; }
        [JsonProperty("departmentCode")]
        public string DepartmentCode { get; set; }
    }
}
