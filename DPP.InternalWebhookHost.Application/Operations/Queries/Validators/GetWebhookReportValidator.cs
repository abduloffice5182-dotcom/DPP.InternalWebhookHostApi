using FluentValidation;
using DPP.InternalWebhookHost.Application.Operations.Queries.Validators; 
using DPP.InternalWebhookHost.Application.Operations.Queries.Requests;

namespace DPP.InternalWebhookHost.Application.Operations.Queries.Validators;

public class GetWebhookReportValidator : AbstractValidator<GetWebhookReportQuery>
{
    public GetWebhookReportValidator()
    {
        this.ApplyDateRules();
        this.ApplyPagingRules();
    }
}