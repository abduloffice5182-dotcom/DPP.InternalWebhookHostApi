namespace DPP.InternalWebhookHost.Application.Validators;

public class GetWebhookReportValidator : AbstractValidator<GetWebhookReportQuery>
{
    public GetWebhookReportValidator()
    {
        this.ApplyDateRules();
        this.ApplyPagingRules();
    }
}