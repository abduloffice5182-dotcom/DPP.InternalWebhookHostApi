using DPP.PartnerPaymentIntegration.Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace DPP.PartnerPaymentIntegration.Application.Operations.Fortellis.Queries.Requests
{
    public class GetAmountByInvoiceIdQuery : IRequest<GetAmountByInvoiceIdResponse>
    {
        [JsonProperty("invoiceId")]
        public string InvoiceId { get; set; }

        [JsonProperty("departmentCode")]
        public string DepartmentCode { get; set; }
    }
}
